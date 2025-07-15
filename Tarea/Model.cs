using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public class ModelContext : DbContext
{


    public string DbPath { get; private set; }

    public DbSet<Provincia> Provincias { get; set; }
    public DbSet<Canton> Cantones { get; set; }
    public DbSet<Distrito> Distritos { get; set; }

    public ModelContext()
    {
        var folder = Path.Combine(Directory.GetCurrentDirectory(), "data");
        Directory.CreateDirectory(folder);
        DbPath = Path.Combine(folder, "CR.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Provincia>()
        .ToTable("Provincia")
        .Property(p => p.Nombre)
        .HasColumnName("ProvinciaNombre");

        modelBuilder.Entity<Canton>()
        .ToTable("Canton")
        .Property(p => p.Nombre)
        .HasColumnName("CantonNombre");

        modelBuilder.Entity<Distrito>()
        .ToTable("Distrito")
        .Property(p => p.Nombre)
        .HasColumnName("DistritoNombre");
    }
}


public class Provincia
{
    [Key]
    public int ProvinciaPK { get; set; }
    public string Nombre { get; set; }
}

public class Canton
{
    [Key]
    public int CantonPK { get; set; }
    public string Nombre { get; set; }

    [ForeignKey("Provincia")]
    public int ProvinciaFK { get; set; }
    public Provincia Provincia { get; set; }
}

public class Distrito
{
    [Key]
    public int DistritoPK { get; set; }
    public string Nombre { get; set; }

    [ForeignKey("Canton")]
    public int CantonFK { get; set; }
    public Canton Canton { get; set; }
}