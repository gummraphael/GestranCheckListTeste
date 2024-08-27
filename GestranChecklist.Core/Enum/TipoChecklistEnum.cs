using System.Runtime.Serialization;

namespace GestranChecklist.Core.Enum
{
    public enum TipoChecklistEnum
    {
        [EnumMember(Value = "Entrada")]
        Entrada,
        [EnumMember(Value = "Saída")]
        Saida
    }
}
