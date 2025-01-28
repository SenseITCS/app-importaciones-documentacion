using Microsoft.EntityFrameworkCore;

namespace ImportacionesApp.API.Data
{
    public class ProyectosClaudeContext : DbContext
    {
        public ProyectosClaudeContext(DbContextOptions<ProyectosClaudeContext> options)
            : base(options)
        {
        }

        public DbSet<CreedencialesApps> CreedencialesApps { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CreedencialesApps>(entity =>
            {
                entity.ToTable("CreedencialesApps");
                
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Contrasena)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Fuente)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UltimaModificacion)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => new { e.Cedula, e.Fuente })
                    .IsUnique();
            });
        }
    }

    public class CreedencialesApps
    {
        public int Id { get; set; }
        public required string Cedula { get; set; }
        public required string Contrasena { get; set; }
        public required string Fuente { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime UltimaModificacion { get; set; }
    }
}