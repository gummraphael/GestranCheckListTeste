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

    public async Task<ResultViewModel<ChecklistDto>> CriarChecklist(ChecklistDto dto)
    {
        var checklist = dto.ToEntity();

        //Verifica se já existe um checklist em aberto para o veículo
        if (await _checklistRepository.ExisteChecklistParaVeiculo(checklist.PlacaVeiculo))
        {
            return ResultViewModel<ChecklistDto>.Error("Já existe um checklist em aberto para este veículo.");
        }

        //Não é possível criar um checklist com o status de Concluído sem que ao menos um item exista na lista
        if (checklist.Status == StatusEnum.Concluido && checklist.Itens.IsNullOrEmpty())
        {
            return ResultViewModel<ChecklistDto>.Error("Não é possível concluir um Checklist sem itens para ser verificados.");
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

        //Se for o próprio supervisor que está executando o checklist,
        //então a propriedade Aprovado, poderá ser qualquer valor (true ou false)
        if (checklist.ExecutorId != checklist.SupervisorId)
        {
            checklist.Aprovado = false;
        }

        checklist.DataExecucao = DateTime.UtcNow;

        var resultado = await _checklistRepository.AdicionarChecklist(checklist);

        var entityToDto = resultado.ToDto();

        return ResultViewModel<ChecklistDto>.Success(entityToDto);
    }

    public async Task<ResultViewModel> AdicionarItemAoChecklist(int checklistId, ChecklistItemDto checklistItemDto, string executorId)
    {
        var checklist = await _checklistRepository.ObterChecklistPorId(checklistId);
        if (checklist == null)
        {
            return ResultViewModel.Error("Checklist não encontrado.");
        }

        if (checklist.ExecutorId != executorId && checklist.Status == StatusEnum.EmExecucao)
        {
            return ResultViewModel.Error("Somente o executor que criou o checklist pode adicionar itens.");
        }

        var item = checklistItemDto.ToEntity();
        checklist.Itens.Add(item);

        await _checklistRepository.AtualizarChecklist(checklist);
        return ResultViewModel.Success();
    }

    public async Task<ResultViewModel<ChecklistDto>> ObterChecklist(int id)
    {
        var checklist = await _checklistRepository.ObterChecklistPorId(id);
        if (checklist == null)
        {
            return ResultViewModel<ChecklistDto>.Error("Checklist não encontrado.");
        }
        return ResultViewModel<ChecklistDto>.Success(checklist.ToDto());
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

            return ResultViewModel<ChecklistItemDto>.Error(mensagemErro);
        }

        checklist.Aprovado = true;
        checklist.Status = StatusEnum.Concluido;
        checklist.SupervisorId = supervisorId;
        await _checklistRepository.AtualizarChecklist(checklist);
        return ResultViewModel.Success();
    }

    public async Task<ResultViewModel<List<ChecklistDto>>> ListarChecklists()
    {
        var checklists = await _checklistRepository.ListarChecklists();
        var checklistDtos = checklists.Select(c => c.ToDto()).ToList();

        return ResultViewModel<List<ChecklistDto>>.Success(checklistDtos);
    }

    public async Task<ResultViewModel<ChecklistDto>> AtualizarChecklist(int id, ChecklistDto checklistDto)
    {
        var checklist = await _checklistRepository.ObterChecklistPorId(id);

        if (checklist == null)
        {
            return ResultViewModel<ChecklistDto>.Error("Checklist não encontrado.");
        }

        ChecklistMapper.UpdateEntityFromDto(checklist, checklistDto);

        await _checklistRepository.AtualizarChecklist(checklist);

        return ResultViewModel<ChecklistDto>.Success(checklist.ToDto());
    }

    private static IEnumerable<ChecklistItemDto> ItensComAltoRisco(Checklist checklist)
    {
        return checklist.Itens
                    .Where(x => x.NivelDeRisco == RiscoEnum.Alto)
                    .Select(x => new ChecklistItemDto
                    {
                        Nome = x.Nome,
                        Observacao = x.Observacao
                    });
    }
}
