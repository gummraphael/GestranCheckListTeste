using Microsoft.EntityFrameworkCore;

namespace GestranChecklist.Infrastructure
{
    public class GestranChecklistDbContext : DbContext
    {
        public GestranChecklistDbContext(DbContextOptions<GestranChecklistDbContext> options) 
            : base(options) 
        { 

        }

        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<ChecklistItem> ChecklistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Checklist>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ChecklistItem>()
                .Property(ci => ci.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
