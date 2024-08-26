using GestranChecklist.Application.Models;
using GestranChecklist.Core.Enum;
using Microsoft.IdentityModel.Tokens;

public class ChecklistService : IChecklistService
{
    private readonly IChecklistRepository _checklistRepository;

    public ChecklistService(IChecklistRepository checklistRepository)
    {
        _checklistRepository = checklistRepository;
    }

    public async Task<ResultViewModel<Checklist>> CriarChecklist(ChecklistModel model)
    {
        if (await _checklistRepository.ExisteChecklistParaVeiculo(model.PlacaVeiculo))
        {
            return ResultViewModel<Checklist>.Error("Já existe um checklist em aberto para este veículo.");
        }

        //model.Status = model.Itens.IsNullOrEmpty() ? model.Status : StatusEnum.EmExecucao;

        var checklist = model.CastToEntity();
        checklist.DataExecucao = DateTime.UtcNow;

        if (checklist.ExecutorId != checklist.SupervisorId)
        {
            checklist.Aprovado = false;
        }

        if (model.Status == StatusEnum.Concluido && model.Itens.IsNullOrEmpty())
        {
            model.Status = StatusEnum.EmExecucao;
        }

        var resultado = await _checklistRepository.AdicionarChecklist(checklist);
        return ResultViewModel<Checklist>.Success(resultado);
    }

    public async Task<ResultViewModel> AdicionarItemAoChecklist(int checklistId, ChecklistItemModel itemModel, string executorId)
    {
        var checklist = await _checklistRepository.ObterChecklistPorId(checklistId);
        if (checklist == null)
        {
            return ResultViewModel.Error("Checklist não encontrado.");
        }

        if (checklist.ExecutorId != executorId && checklist.Status == StatusEnum.EmExecucao)
        {
            /// Verificar regra aqui
            return ResultViewModel.Error("Somente o executor que criou o checklist pode adicionar itens.");
        }

        var item = itemModel.CastToEntity();
        checklist.Itens.Add(item);

        await _checklistRepository.AtualizarChecklist(checklist);
        return ResultViewModel.Success();
    }

    public async Task<ResultViewModel<Checklist>> ObterChecklist(int id)
    {
        var checklist = await _checklistRepository.ObterChecklistPorId(id);
        if (checklist == null)
        {
            return ResultViewModel<Checklist>.Error("Checklist não encontrado.");
        }
        return ResultViewModel<Checklist>.Success(checklist);
    }

    public async Task<ResultViewModel> AprovarChecklist(int id, string supervisorId)
    {
        var checklist = await _checklistRepository.ObterChecklistPorId(id);
        if (checklist == null)
        {
            return ResultViewModel.Error("Checklist não encontrado.");
        }

        var itensComRiscoAlto = checklist.Itens
            .Where(x => x.NilvelDeRisco == RiscoEnum.Alto)
            .Select(x => new ChecklistItemModel
            {
                Nome = x.Nome,
                Observacao = x.Observacao
            });

        if (itensComRiscoAlto.Any())
        {
            var mensagemErro = "Não foi possível aprovar o checklist, pois existem itens com alto risco de criticidade:\n\n";

            foreach (var item in itensComRiscoAlto)
            {
                mensagemErro += $"- Nome do item: {item.Nome}\n- Observação: {item.Observacao}\n\n";
            }

            return ResultViewModel<ChecklistItem>.Error(mensagemErro);
        }

        checklist.Aprovado = true;
        checklist.Status = StatusEnum.Concluido;
        checklist.SupervisorId = supervisorId;
        await _checklistRepository.AtualizarChecklist(checklist);
        return ResultViewModel.Success();
    }

    public async Task<ResultViewModel<List<Checklist>>> ListarChecklists()
    {
        var checklists = await _checklistRepository.ListarChecklists();
        return ResultViewModel<List<Checklist>>.Success(checklists);
    }
}
