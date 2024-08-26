using GestranChecklist.Core.Enum;

namespace GestranChecklist.Application.Models
{
    public class ChecklistItemModel
    {
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public RiscoEnum NilvelDeRisco { get; set; }

        public ChecklistItem CastToEntity()
        {
            return new ChecklistItem(Nome, Observacao, NilvelDeRisco);
        }
    }
}
