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

        public DbSet<DirPartyTable> DirPartyTable { get; set; }
        public DbSet<Company> BiCompanyViewPbi { get; set; }

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

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToView("BICOMPANYVIEW_PBI");
                
                entity.HasKey(e => e.ID);
                
                entity.Property(e => e.ID)
                    .HasColumnName("ID");

                entity.Property(e => e.NAME)
                    .HasColumnName("NAME");

                entity.Property(e => e.STATUSCOMPANY)
                    .HasColumnName("STATUSCOMPANY");

                entity.Property(e => e.COMPANYCLASSIFICATION)
                    .HasColumnName("COMPANYCLASSIFICATION");
            });
        }
    }

    public class DirPartyTable
    {
        public long RecId { get; set; }
        public string VATNum_FE { get; set; }
        public string Name { get; set; }
    }
}