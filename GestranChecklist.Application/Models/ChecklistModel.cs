using GestranChecklist.Core.Enum;

namespace GestranChecklist.Application.Models
{
    public class ChecklistModel
    {
        public string PlacaVeiculo { get; set; }
        public TipoChecklistEnum TipoChecklist { get; set; }
        public string ExecutorId { get; set; }
        public string SupervisorId { get; set; }
        public bool Aprovado { get; set; } = false;
        public StatusEnum Status { get; set; }
        public DateTime DataExecucao { get; set; }

        public IEnumerable<ChecklistItemModel> Itens { get; set; } = new List<ChecklistItemModel>();

        public Checklist CastToEntity()
        {
            var checklist = new Checklist(PlacaVeiculo, TipoChecklist, ExecutorId, SupervisorId, Aprovado, Status, DataExecucao);

            foreach (var item in Itens)
            {
                checklist.Itens.Add(item.CastToEntity());
            }

            return checklist;
        }
    }
}
