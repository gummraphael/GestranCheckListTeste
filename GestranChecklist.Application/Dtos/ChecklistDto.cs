using GestranChecklist.Core.Enum;

namespace GestranChecklist.Application.Dtos
{
    public class ChecklistDto
    {
        public string PlacaVeiculo { get; set; }
        public TipoChecklistEnum TipoChecklist { get; set; }
        public string ExecutorId { get; set; }
        public string SupervisorId { get; set; }
        public bool Aprovado { get; set; } = false;
        public StatusEnum Status { get; set; }

        public ICollection<ChecklistItemDto> Itens { get; set; } = new List<ChecklistItemDto>();
    }
}
