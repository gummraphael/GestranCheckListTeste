using GestranChecklist.Core.Entities;
using GestranChecklist.Core.Enum;

public class ChecklistItem : BaseEntity
{
    public string Nome { get; set; }
    public string Observacao { get; set; }
    public RiscoEnum NilvelDeRisco { get; set; }

    public ChecklistItem() {}

    public ChecklistItem(string nome, string observacao, RiscoEnum nivelDeRisco)
    {
        Nome = nome;
        Observacao = observacao;
        NilvelDeRisco = nivelDeRisco;
    }
}
