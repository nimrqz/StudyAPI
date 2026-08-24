using Microsoft.EntityFrameworkCore;
using StudyAPI.Models;

namespace StudyAPI.Data;

public class StudyDbContext : DbContext
{
    public StudyDbContext(DbContextOptions<StudyDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<StudyTask> StudyTasks => Set<StudyTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Color).HasMaxLength(7);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
        });

        modelBuilder.Entity<StudyTask>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Notes).HasMaxLength(5000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");

            entity.HasOne(e => e.Category)
                .WithMany(c => c.StudyTasks)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "C# Básico",
                Description = "Fundamentos da linguagem C#",
                Color = "#3498DB",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Category
            {
                Id = 2,
                Name = "ASP.NET Core",
                Description = "Desenvolvimento de APIs e aplicações web",
                Color = "#2ECC71",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Category
            {
                Id = 3,
                Name = "Entity Framework Core",
                Description = "ORM para acesso a dados",
                Color = "#E74C3C",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
