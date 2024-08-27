public interface IChecklistRepository
{
    Task<Checklist> AdicionarChecklist(Checklist checklist);
    Task<Checklist> ObterChecklistPorId(int id);
    Task<List<Checklist>> ListarChecklists();
    Task AtualizarChecklist(Checklist checklist);
    Task<bool> ExisteChecklistParaVeiculo(string executorId);
}
