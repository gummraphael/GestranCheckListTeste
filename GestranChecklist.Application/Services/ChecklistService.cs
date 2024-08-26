using GestranChecklist.Application.Dtos;
using GestranChecklist.Core.Enum;
using Microsoft.IdentityModel.Tokens;

public class ChecklistService : IChecklistService
{
    private readonly IChecklistRepository _checklistRepository;

    public ChecklistService(IChecklistRepository checklistRepository)
    {
        _checklistRepository = checklistRepository;
    }

    public async Task<ResultViewModel<ChecklistDto>> CriarChecklist(ChecklistDto model)
    {
        var checklist = model.CastToEntity();

        //Verifica se já existe um checklist em aberto para o veículo
        if (await _checklistRepository.ExisteChecklistParaVeiculo(checklist.PlacaVeiculo))
        {
            return ResultViewModel<ChecklistDto>.Error("Já existe um checklist em aberto para este veículo.");
        }

        IEnumerable<ChecklistItemDto> itensComRiscoAlto = ItensComAltoRisco(checklist);

        //Não será possível concluir um checklist se existir algum item com alto risco
        if (itensComRiscoAlto.Any() && checklist.Status == StatusEnum.Concluido)
        {
            var mensagemErro = "Não foi possível aprovar o checklist, pois existem itens com alto risco de criticidade:\n\n";

            foreach (var item in itensComRiscoAlto)
            {
                mensagemErro += $"- Nome do item: {item.Nome}\n- Observação: {item.Observacao}\n\n";
            }

            return ResultViewModel<ChecklistDto>.Error(mensagemErro);
        }

        //Não é possível criar um checklist sem que ao menos um item exista na lista
        if (checklist.Status == StatusEnum.Concluido && checklist.Itens.IsNullOrEmpty())
        {
            return ResultViewModel<ChecklistDto>.Error("Não é possível concluir um Checklist sem itens para ser verificados.");
        }

        //Se for o próprio supervisor que está executando o checklist,
        //então a propriedade Aprovado, poderá ser qualquer valor (true ou false)
        if (checklist.ExecutorId != checklist.SupervisorId)
        {
            checklist.Aprovado = false;
        }

        checklist.DataExecucao = DateTime.UtcNow;

        var resultado = await _checklistRepository.AdicionarChecklist(checklist);

        model.ExecutorId = resultado.ExecutorId;
        model.SupervisorId = resultado.SupervisorId;
        model.TipoChecklist = resultado.TipoChecklist;

        return ResultViewModel<ChecklistDto>.Success(model);
    }

    public async Task<ResultViewModel> AdicionarItemAoChecklist(int checklistId, ChecklistItemDto itemModel, string executorId)
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

        IEnumerable<ChecklistItemDto> itensComRiscoAlto = ItensComAltoRisco(checklist);

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

    public async Task<ResultViewModel<ChecklistDto>> AtualizarChecklist(ChecklistDto checklistModel)
    {
        var checklist = await _checklistRepository.ObterChecklistPorId(1);

        if (checklist == null)
        {
            return ResultViewModel<ChecklistDto>.Error("Checklist não encontrado.");
        }

        checklist = checklistModel.CastToEntity();

        //checklist.Update(checklistModel.PlacaVeiculo, checklistModel.ExecutorId, checklistModel.SupervisorId, 
        //                 checklistModel.Aprovado, checklistModel.Status, checklistModel.DataExecucao);

        await _checklistRepository.AtualizarChecklist(checklist);

        return ResultViewModel<ChecklistDto>.Success(checklistModel);
    }

    private static IEnumerable<ChecklistItemDto> ItensComAltoRisco(Checklist checklist)
    {
        return checklist.Itens
                    .Where(x => x.NilvelDeRisco == RiscoEnum.Alto)
                    .Select(x => new ChecklistItemDto
                    {
                        Nome = x.Nome,
                        Observacao = x.Observacao
                    });
    }

    Task<ResultViewModel<ChecklistDto>> IChecklistService.ObterChecklist(int id)
    {
        throw new NotImplementedException();
    }

    Task<ResultViewModel<List<ChecklistDto>>> IChecklistService.ListarChecklists()
    {
        throw new NotImplementedException();
    }
}
