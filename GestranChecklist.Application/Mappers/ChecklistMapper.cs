using GestranChecklist.Application.Dtos;

public static class ChecklistMapper
{
    public static ChecklistDto ToDto(this Checklist entity)
    {
        return new ChecklistDto
        {
            PlacaVeiculo = entity.PlacaVeiculo,
            ExecutorId = entity.ExecutorId,
            SupervisorId = entity.SupervisorId,
            Aprovado = entity.Aprovado,
            Status = entity.Status,
            Itens = entity.Itens.Select(item => new ChecklistItemDto
            {
                Nome = item.Nome,
                Observacao = item.Observacao,
                NivelDeRisco = item.NivelDeRisco
            }).ToList()
        };
    }

    public static Checklist ToEntity(this ChecklistDto dto)
    {
        return new Checklist
        {
            PlacaVeiculo = dto.PlacaVeiculo,
            TipoChecklist = dto.TipoChecklist,
            ExecutorId = dto.ExecutorId,
            SupervisorId = dto.SupervisorId,
            Aprovado = dto.Aprovado,
            Status = dto.Status,
            DataExecucao = DateTime.Now,
            Itens = dto.Itens.Select(item => new ChecklistItem
            {
                Nome = item.Nome,
                Observacao = item.Observacao,
                NivelDeRisco = item.NivelDeRisco
            }).ToList()
        };
    }

    private static void UpdateItemsFromDto(Checklist entity, ICollection<ChecklistItemDto> itemDtos)
    {
        foreach (var itemDto in itemDtos)
        {
            //Verifica se o item já existe na lista
            var existingItem = entity.Itens.FirstOrDefault(i => i.Id == itemDto.Id);

            if (existingItem != null)
            {
                //Atualiza o item já existir no banco
                existingItem.Nome = itemDto.Nome;
                existingItem.Observacao = itemDto.Observacao;
                existingItem.NivelDeRisco = itemDto.NivelDeRisco;
            }
            else
            {
                //Se o item não existir, insere na lista e no banco
                var newItem = new ChecklistItem
                {
                    Nome = itemDto.Nome,
                    Observacao = itemDto.Observacao,
                    NivelDeRisco = itemDto.NivelDeRisco,
                };

                entity.Itens.Add(newItem);
            }
        }
    }

    public static void UpdateEntityFromDto(Checklist entity, ChecklistDto dto)
    {
        entity.PlacaVeiculo = dto.PlacaVeiculo;
        entity.ExecutorId = dto.ExecutorId;
        entity.TipoChecklist = dto.TipoChecklist;
        entity.SupervisorId = dto.SupervisorId;
        entity.Aprovado = dto.Aprovado;
        entity.Status = dto.Status;
        entity.DataExecucao = DateTime.Now;

        UpdateItemsFromDto(entity, dto.Itens);
    }
}
