// <copyright file="MapPageViewModel.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using System.Windows.Input;
using GSD.Minecraft.Plotter.Services;

/// <summary>
/// Represents the view model for the map, providing data and commands to support map-related functionality.
/// </summary>
public class MapPageViewModel : ViewModelBase
{
    /// <summary>
    /// The current application state used to synchronize the map's drawable elements.
    /// </summary>
    private readonly AppState appState;

    /// <summary>
    /// Initializes a new instance of the <see cref="MapPageViewModel" /> class.
    /// </summary>
    /// <param name="appState">The current application state used to synchronize the map's drawable elements.</param>
    public MapPageViewModel(AppState appState)
    {
        this.appState = appState ?? throw new ArgumentNullException(nameof(appState));
        this.appState.CurrentWorldChanged += this.OnCurrentWorldChanged;
        this.appState.MarkersChanged += this.OnMarkersChanged;

        this.OverworldCommand = new Command(() => this.MapLayout = new OverworldMapLayout());
        this.NetherCommand = new Command(() => this.MapLayout = new NetherMapLayout());
        this.MapLayout = new OverworldMapLayout();

        this.UpdateMarkers();
        this.UpdateTitle();
    }

    /// <summary>
    /// Gets the map drawable.
    /// </summary>
    public MapDrawable MapDrawable { get; } = new();

    /// <summary>
    /// Gets or sets the current map layout used for plotting markers on the map.
    /// </summary>
    public IMapLayout MapLayout
    {
        get => this.GetValue<IMapLayout>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets the command to set the layout relative to the Nether map.
    /// </summary>
    public ICommand NetherCommand { get; }

    /// <summary>
    /// Gets the command to set the layout relative to the Overworld map.
    /// </summary>
    public ICommand OverworldCommand { get; }

    /// <summary>
    /// Gets or sets the title of the map page, which dynamically reflects the name of the current world.
    /// </summary>
    public string Title
    {
        get => this.GetValue<string>();
        set => this.SetValue(value);
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
    /// Occurs when the collection of markers in the application state is modified.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An <see cref="EventArgs" /> that contains no event data.</param>
    private void OnMarkersChanged(object sender, EventArgs e)
    {
        this.UpdateMarkers();
    }

    /// <summary>
    /// Updates the markers displayed on the map by synchronizing them with the current state of the application.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private async void UpdateMarkers()
    {
        var markers = await this.appState.GetMarkersAsync().ConfigureAwait(false);

        this.MapDrawable.Markers.Clear();

        foreach (var marker in markers)
        {
            this.MapDrawable.Markers.Add(marker.ToViewModel());
        }
    }

    /// <summary>
    /// Updates the title of the map page to reflect the name of the current world.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private async void UpdateTitle()
    {
        var world = await this.appState.GetCurrentWorldAsync().ConfigureAwait(true);
        this.Title = $"Map - {world.Name}";
    }
}