using System.Runtime.Serialization;

namespace GestranChecklist.Core.Enum
{
    public enum RiscoEnum
    {
        [EnumMember(Value = "Baixo")]
        Baixo,
        [EnumMember(Value = "Médio")]
        Medio,
        [EnumMember(Value = "Alto")]
        Alto
    }
}
