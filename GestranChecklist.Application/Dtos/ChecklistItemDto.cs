using GestranChecklist.Core.Enum;

namespace GestranChecklist.Application.Dtos
{
    public class ChecklistItemDto
    {
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public RiscoEnum NivelDeRisco { get; set; }

        public ChecklistItem CastToEntity()
        {
            return new ChecklistItem(Nome, Observacao, NivelDeRisco);
        }
    }
}
