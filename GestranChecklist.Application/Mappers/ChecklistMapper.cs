using GestranChecklist.Application.Dtos;

public static class ChecklistMapper
{
    public static ChecklistDto ToDto(this Checklist entity)
    {
        return new ChecklistDto


        {
            Id = entity.Id,
            PlacaVeiculo = entity.PlacaVeiculo,
            Descricao = entity.Descricao
        };
    }

    public static Checklist ToEntity(this ChecklistDto dto)
    {
        return new Checklist
        {
            Id = dto.Id,
            PlacaVeiculo = dto.PlacaVeiculo,
            Descricao = dto.Descricao,
            // Defina DataCriacao conforme necessário, por exemplo, DateTime.Now ou um valor específico
        };
    }
}
