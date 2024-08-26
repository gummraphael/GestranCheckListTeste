using GestranChecklist.Application.Models;

public interface IChecklistService
{
    Task<ResultViewModel<Checklist>> CriarChecklist(ChecklistModel model);
    Task<ResultViewModel> AdicionarItemAoChecklist(int checklistId, ChecklistItemModel itemModel, string executorId);
    Task<ResultViewModel<Checklist>> ObterChecklist(int id);
    Task<ResultViewModel> AprovarChecklist(int id, string supervisorId);
    Task<ResultViewModel<List<Checklist>>> ListarChecklists();
}
