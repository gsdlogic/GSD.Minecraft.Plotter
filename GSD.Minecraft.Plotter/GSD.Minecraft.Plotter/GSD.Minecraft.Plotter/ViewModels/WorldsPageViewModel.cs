// <copyright file="WorldsViewModel.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;

/// <summary>
/// Represents the view model for managing and interacting with a collection of worlds.
/// </summary>
public class WorldsPageViewModel : ViewModelBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorldsPageViewModel" /> class.
    /// </summary>
    public WorldsPageViewModel()
    {
        this.SelectWorldCommand = new Command<WorldViewModel>(this.OnSelectWorld);
        this.CreateWorldCommand = new Command(this.OnCreateWorld);
    }

    /// <summary>
    /// Gets the command that creates a new world.
    /// </summary>
    public ICommand CreateWorldCommand { get; }

    /// <summary>
    /// Gets the command used to select a specific world from the collection of worlds.
    /// </summary>
    public ICommand SelectWorldCommand { get; }

    /// <summary>
    /// Gets the collection of worlds managed by this view model.
    /// </summary>
    public ObservableCollection<WorldViewModel> Worlds { get; } = [];

    /// <summary>
    /// Handles the creation of a new world.
    /// </summary>
    private void OnCreateWorld()
    {
        // TODO: create a new world
    }

    /// <summary>
    /// Handles the selection of a world.
    /// </summary>
    /// <param name="world">The <see cref="WorldViewModel" /> representing the selected world.</param>
    private void OnSelectWorld(WorldViewModel world)
    {
        // TODO: switch MapView.World
    }
}