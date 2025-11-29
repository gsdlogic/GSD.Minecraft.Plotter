// <copyright file="WorldsPageViewModel.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;
using GSD.Minecraft.Plotter.Services;
using GSD.Minecraft.Plotter.Views;

/// <summary>
/// Represents the view model for managing and interacting with a collection of worlds.
/// </summary>
public class WorldsPageViewModel : ViewModelBase
{
    /// <summary>
    /// The application state that provides access to the current world and its markers.
    /// </summary>
    private readonly AppState appState;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldsPageViewModel" /> class.
    /// </summary>
    /// <param name="appState">The application state that provides access to the current world and its markers.</param>
    public WorldsPageViewModel(AppState appState)
    {
        this.appState = appState ?? throw new ArgumentNullException(nameof(appState));
        this.appState.CurrentWorldChanged += this.OnCurrentWorldChanged;
        this.appState.WorldsChanged += this.OnWorldsChanged;

        this.CreateWorldCommand = new Command(this.CreateWorld);
        this.EditWorldCommand = new Command<WorldViewModel>(this.EditWorld);

        this.UpdateWorlds();
        this.UpdateTitle();
    }

    /// <summary>
    /// Gets the command that creates a new world and adds it to the collection of worlds.
    /// </summary>
    public ICommand CreateWorldCommand { get; }

    /// <summary>
    /// Gets the command that allows editing an existing marker in the collection of markers.
    /// </summary>
    public ICommand EditWorldCommand { get; }

    /// <summary>
    /// Gets or sets the title of the map page, which dynamically reflects the name of the current world.
    /// </summary>
    public string Title
    {
        get => this.GetValue<string>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets the collection of worlds managed by this view model.
    /// </summary>
    public ObservableCollection<WorldViewModel> Worlds { get; } = [];

    /// <summary>
    /// Creates a new world, initializes its properties, and navigates to the edit page for further customization.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private async void CreateWorld()
    {
        var markerViewModel = new WorldViewModel
        {
            Name = $"World {this.Worlds.Count + 1}",
        };

        var pageViewModel = new EditWorldPageViewModel(this.appState, markerViewModel);
        var page = new EditWorldPage(pageViewModel);

        await Shell.Current.CurrentPage.Navigation.PushModalAsync(page).ConfigureAwait(true);
    }

    /// <summary>
    /// Edits the specified marker in the current world.
    /// </summary>
    /// <param name="markerViewModel">The marker to be edited, represented by a <see cref="WorldViewModel" /> instance.</param>
    /// ReSharper disable once AsyncVoidMethod
    private async void EditWorld(WorldViewModel markerViewModel)
    {
        var pageViewModel = new EditWorldPageViewModel(this.appState, markerViewModel);
        var page = new EditWorldPage(pageViewModel);

        await Shell.Current.CurrentPage.Navigation.PushModalAsync(page).ConfigureAwait(true);
    }

    /// <summary>
    /// Occurs when the current world in the application state changes.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An <see cref="EventArgs" /> containing no event data.</param>
    private void OnCurrentWorldChanged(object sender, EventArgs e)
    {
        this.UpdateTitle();
    }

    /// <summary>
    /// Occurs when the collection of worlds in the application state is modified.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An <see cref="EventArgs" /> that contains no event data.</param>
    private void OnWorldsChanged(object sender, EventArgs e)
    {
        this.UpdateWorlds();
    }

    /// <summary>
    /// Updates the title of the map page to reflect the name of the current world.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private async void UpdateTitle()
    {
        var world = await this.appState.GetCurrentWorldAsync().ConfigureAwait(true);
        this.Title = world.Name;
    }

    /// <summary>
    /// Updates the collection of worlds by synchronizing it with the current application state.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private async void UpdateWorlds()
    {
        var worlds = await this.appState.GetWorldsAsync().ConfigureAwait(false);

        this.Worlds.Clear();

        foreach (var world in worlds)
        {
            this.Worlds.Add(world.ToViewModel());
        }
    }
}