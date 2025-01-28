using Microsoft.EntityFrameworkCore;
using ImportacionesApp.API.Models;

namespace ImportacionesApp.API.Data
{
    public class GIAXContext : DbContext
    {
        public GIAXContext(DbContextOptions<GIAXContext> options)
            : base(options)
        {
        }

        public DbSet<DirPartyTable> DirPartyTable { get; set; } = null!;
        public DbSet<Company> BiCompanyViewPbi { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DirPartyTable>(entity =>
            {
                entity.ToTable("DirPartyTable");
                
                entity.HasKey(e => e.RecId);
                
                entity.Property(e => e.VATNum_FE)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(e => e.VATNum_FE);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToView("BICOMPANYVIEW_PBI");
                
                entity.HasKey(e => e.ID);
                
                entity.Property(e => e.ID)
                    .HasColumnName("ID")
                    .IsRequired();

                entity.Property(e => e.NAME)
                    .HasColumnName("NAME")
                    .IsRequired();

                entity.Property(e => e.STATUSCOMPANY)
                    .HasColumnName("STATUSCOMPANY")
                    .IsRequired();

                entity.Property(e => e.COMPANYCLASSIFICATION)
                    .HasColumnName("COMPANYCLASSIFICATION")
                    .IsRequired();
            });
        }
    }

    public class DirPartyTable
    {
        public long RecId { get; set; }
        public required string VATNum_FE { get; set; }
        public required string Name { get; set; }
    }
}