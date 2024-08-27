using GestranChecklist.Core.Enum;
using GestranChecklist.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class ChecklistRepository : IChecklistRepository
{
    private readonly GestranChecklistDbContext _context;

    public ChecklistRepository(GestranChecklistDbContext context)
    {
        _context = context;
    }

    public async Task<Checklist> AdicionarChecklist(Checklist checklist)
    {
        await _context.Checklists.AddAsync(checklist);
        await _context.SaveChangesAsync();
        return checklist;
    }

    public async Task<Checklist?> ObterChecklistPorId(int id)
    {
        return await _context.Checklists
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Checklist>> ListarChecklists()
    {
        return await _context.Checklists.Include(c => c.Itens).ToListAsync();
    }

    public async Task AtualizarChecklist(Checklist checklist)
    {
        _context.Entry(checklist).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteChecklistParaVeiculo(string placaVeiculo)
    {
        return await _context.Checklists.AnyAsync(c => c.PlacaVeiculo == placaVeiculo && c.Status != StatusEnum.Concluido);
    }
}
