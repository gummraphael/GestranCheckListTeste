using GestranChecklist.Core.Enum;

namespace GestranChecklist.Application.Dtos
{
    public class ChecklistItemDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public RiscoEnum NivelDeRisco { get; set; }
    }
}
