using Microsoft.EntityFrameworkCore;

namespace ImportacionesApp.API.Data
{
    public class GIAXContext : DbContext
    {
        public GIAXContext(DbContextOptions<GIAXContext> options)
            : base(options)
        {
        }

        public DbSet<DirPartyTable> DirPartyTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DirPartyTable>(entity =>
            {
                entity.ToTable("DirPartyTable");
                
                entity.HasKey(e => e.RecId);
                
                entity.Property(e => e.VATNum_FE)
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .HasMaxLength(100);

                entity.HasIndex(e => e.VATNum_FE);
            });
        }
    }

    public class DirPartyTable
    {
        public long RecId { get; set; }
        public string VATNum_FE { get; set; }
        public string Name { get; set; }
        // Otros campos necesarios pueden ser agregados aquí
    }
}