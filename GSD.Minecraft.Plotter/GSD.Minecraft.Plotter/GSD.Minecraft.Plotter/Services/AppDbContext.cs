// <copyright file="AppDbContext.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Services;

using GSD.Minecraft.Plotter.Models;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents the application's database context, providing access to the database entities
/// and configuring their relationships and constraints.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext" /> class
    /// using the specified options.
    /// </summary>
    /// <param name="options">The options to be used by the <see cref="AppDbContext" />.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the collection of <see cref="Marker" /> entities in the database.
    /// </summary>
    public DbSet<Marker> Markers { get; set; }

    /// <summary>
    /// Gets or sets the collection of <see cref="Setting" /> entities in the database.
    /// </summary>
    public DbSet<Setting> Settings { get; set; }

    /// <summary>
    /// Gets or sets the collection of <see cref="World" /> entities in the database.
    /// </summary>
    public DbSet<World> Worlds { get; set; }

    /// <summary>
    /// Configures the model relationships, constraints, and properties for the database context.
    /// </summary>
    /// <param name="modelBuilder">An instance of <see cref="ModelBuilder" /> used to define the model for the database context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.Key);

            entity.Property(e => e.Key)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<World>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(w => w.Seed)
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Marker>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(m => m.World)
                .WithMany(w => w.Markers)
                .HasForeignKey(m => m.WorldId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);
        });
    }
}