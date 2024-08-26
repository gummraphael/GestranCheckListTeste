using GestranChecklist.Core.Entities;
using GestranChecklist.Core.Enum;

public class Checklist : BaseEntity
{
    public string PlacaVeiculo { get; set; }
    public TipoChecklistEnum TipoChecklist { get; set; }
    public string ExecutorId { get; set; } 
    public string SupervisorId { get; set; } 
    public bool Aprovado { get; set; } = false;
    public StatusEnum Status { get; set; } = StatusEnum.ParaExecutar;
    public DateTime? DataExecucao { get; set; } 
    public ICollection<ChecklistItem> Itens { get; set; } 

    
    public Checklist()
    {
        Itens = new List<ChecklistItem>();
    }

    
    public Checklist(string placaVeiculo, TipoChecklistEnum tipoChecklist, string executorId, string supervisorId, bool aprovado, StatusEnum status, DateTime? dataExecucao)
    {
        PlacaVeiculo = placaVeiculo;
        ExecutorId = executorId;
        SupervisorId = supervisorId;
        Aprovado = aprovado;
        Status = status;
        DataExecucao = dataExecucao;
        Itens = new List<ChecklistItem>(); 
    }
}
