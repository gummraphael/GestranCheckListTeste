using System.Runtime.Serialization;

namespace GestranChecklist.Core.Enum
{
    public enum StatusEnum
    {
        [EnumMember(Value = "Para Executar")]
        ParaExecutar,
        [EnumMember(Value = "Em Execução")]
        EmExecucao,
        [EnumMember(Value = "Concluído")]
        Concluido
    }
}
