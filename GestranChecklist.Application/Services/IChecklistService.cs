using GestranChecklist.Application.Dtos;

public interface IChecklistService
{
    Task<ResultViewModel<ChecklistDto>> CriarChecklist(ChecklistDto model);
    Task<ResultViewModel> AdicionarItemAoChecklist(int checklistId, ChecklistItemDto itemModel, string executorId);
    Task<ResultViewModel<ChecklistDto>> ObterChecklist(int id);
    Task<ResultViewModel> AprovarChecklist(int id, string supervisorId);
    Task<ResultViewModel<List<ChecklistDto>>> ListarChecklists();
}
