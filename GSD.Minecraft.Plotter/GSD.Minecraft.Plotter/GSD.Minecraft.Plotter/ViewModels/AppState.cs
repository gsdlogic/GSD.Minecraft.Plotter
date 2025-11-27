// <copyright file="AppState.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using System.Collections.ObjectModel;
using GSD.Minecraft.Plotter.Services;

/// <summary>
/// Represents the application state, providing properties and functionality to manage and interact with the current state of the application.
/// </summary>
public class AppState : ViewModelBase
{
    /// <summary>
    /// The <see cref="AppDbContext" /> instance used to manage and interact with the application's database.
    /// </summary>
    private readonly AppDbContext dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppState" /> class with the specified database context.
    /// </summary>
    /// <param name="dbContext">The <see cref="AppDbContext" /> instance used to manage and interact with the application's database.</param>
    public AppState(AppDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        this.CurrentWorld = new WorldViewModel
        {
            Name = "Default World",
        };

        this.Worlds.Add(this.CurrentWorld);
    }

    /// <summary>
    /// Gets or sets the current world being managed or interacted with in the application.
    /// </summary>
    public WorldViewModel CurrentWorld
    {
        get => this.GetValue<WorldViewModel>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets the collection of markers.
    /// </summary>
    public ObservableCollection<MarkerViewModel> Markers { get; } = [];

    /// <summary>
    /// Gets the collection of worlds available in the application.
    /// </summary>
    public ObservableCollection<WorldViewModel> Worlds { get; } = [];
}