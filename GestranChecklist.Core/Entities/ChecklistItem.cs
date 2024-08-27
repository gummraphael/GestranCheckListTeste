using GestranChecklist.Core.Entities;
using GestranChecklist.Core.Enum;

public class ChecklistItem : BaseEntity
{
    public string Nome { get; set; }
    public string Observacao { get; set; }
    public RiscoEnum NivelDeRisco { get; set; }

    public ChecklistItem() {}
}
