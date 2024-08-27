using GestranChecklist.Application.Dtos;

public static class ChecklistItemMapper
{
    public static ChecklistItemDto ToDto(this ChecklistItem entity)
    {
        return new ChecklistItemDto
        {
            Nome = entity.Nome,
            Observacao = entity.Observacao,
            NivelDeRisco = entity.NivelDeRisco
        };
    }

    public static ChecklistItem ToEntity(this ChecklistItemDto dto)
    {
        return new ChecklistItem
        {
            Nome = dto.Nome,
            Observacao = dto.Observacao,
            NivelDeRisco= dto.NivelDeRisco
        };
    }
}
