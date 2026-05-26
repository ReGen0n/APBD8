namespace APBD8.Data;

using Microsoft.EntityFrameworkCore;
using APBD8.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<PC> PCs { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<ComponentManufacturer> ComponentManufacturers { get; set; }
    public DbSet<PCComponent> PCComponents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PC>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Weight).IsRequired();
            entity.Property(e => e.Warranty).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.Stock).IsRequired();
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code).HasMaxLength(10).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500).IsRequired();

            entity.HasOne(e => e.ComponentManufacturer)
                .WithMany(e => e.Components)
                .HasForeignKey(e => e.ComponentManufacturerId);

            entity.HasOne(e => e.ComponentType)
                .WithMany(e => e.Components)
                .HasForeignKey(e => e.ComponentTypeId);
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<ComponentManufacturer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Abbreviation).HasMaxLength(30).IsRequired();
            entity.Property(e => e.FullName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.FoundationDate).IsRequired();
        });

        modelBuilder.Entity<PCComponent>(entity =>
        {
            entity.HasKey(e => new { e.PCId, e.ComponentCode });

            entity.Property(e => e.Amount).IsRequired();

            entity.HasOne(e => e.PC)
                .WithMany(e => e.PCComponents)
                .HasForeignKey(e => e.PCId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Component)
                .WithMany(e => e.PCComponents)
                .HasForeignKey(e => e.ComponentCode);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Processor" },
            new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Card" },
            new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Memory" }
        );

        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer
            {
                Id = 1,
                Abbreviation = "INTEL",
                FullName = "Intel Corporation",
                FoundationDate = new DateTime(1968, 7, 18)
            },
            new ComponentManufacturer
            {
                Id = 2,
                Abbreviation = "NVIDIA",
                FullName = "NVIDIA Corporation",
                FoundationDate = new DateTime(1993, 4, 5)
            },
            new ComponentManufacturer
            {
                Id = 3,
                Abbreviation = "KINGSTON",
                FullName = "Kingston Technology",
                FoundationDate = new DateTime(1987, 10, 17)
            }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component
            {
                Code = "CPU001",
                Name = "Intel Core i7",
                Description = "High performance processor",
                ComponentManufacturerId = 1,
                ComponentTypeId = 1
            },
            new Component
            {
                Code = "GPU001",
                Name = "NVIDIA RTX 4070",
                Description = "Gaming graphics card",
                ComponentManufacturerId = 2,
                ComponentTypeId = 2
            },
            new Component
            {
                Code = "RAM001",
                Name = "Kingston Fury 16GB",
                Description = "DDR5 memory module",
                ComponentManufacturerId = 3,
                ComponentTypeId = 3
            }
        );

        modelBuilder.Entity<PC>().HasData(
            new PC
            {
                Id = 1,
                Name = "Gaming Beast X",
                Weight = 12.5,
                Warranty = 36,
                CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0),
                Stock = 5
            },
            new PC
            {
                Id = 2,
                Name = "Office Mini Pro",
                Weight = 4.2,
                Warranty = 24,
                CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0),
                Stock = 12
            },
            new PC
            {
                Id = 3,
                Name = "Student Basic PC",
                Weight = 6.8,
                Warranty = 12,
                CreatedAt = new DateTime(2026, 3, 20, 10, 15, 0),
                Stock = 20
            }
        );

        modelBuilder.Entity<PCComponent>().HasData(
            new PCComponent { PCId = 1, ComponentCode = "CPU001", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "GPU001", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "RAM001", Amount = 2 },
            new PCComponent { PCId = 2, ComponentCode = "CPU001", Amount = 1 },
            new PCComponent { PCId = 2, ComponentCode = "RAM001", Amount = 1 },
            new PCComponent { PCId = 3, ComponentCode = "RAM001", Amount = 1 }
        );
    }
}