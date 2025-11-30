// <copyright file="MapPageViewModel.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using System.Collections.ObjectModel;
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

        this.OverworldCommand = new Command(() => this.Layout = new OverworldMapLayout());
        this.NetherCommand = new Command(() => this.Layout = new NetherMapLayout());

        this.Zoom = 1.0f;
        this.Layout = new OverworldMapLayout();
        this.Drawable = new MapDrawable(this);

        this.UpdateMarkers();
        this.UpdateTitle();
    }

    /// <summary>
    /// Occurs when the map view requires invalidation.
    /// </summary>
    public event EventHandler InvalidateRequested;

    /// <summary>
    /// Gets or sets the X-component for the center of the map relative to the view.
    /// </summary>
    public float CenterX
    {
        get => this.GetValue<float>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets or sets the Y-component for the center of the map relative to the view.
    /// </summary>
    public float CenterY
    {
        get => this.GetValue<float>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets the map drawable.
    /// </summary>
    public MapDrawable Drawable { get; }

    /// <summary>
    /// Gets or sets the current map layout used for plotting markers on the map.
    /// </summary>
    public IMapLayout Layout
    {
        get => this.GetValue<IMapLayout>();
        set
        {
            this.SetValue(value);
            this.OnInvalidateRequested();
        }
    }

    /// <summary>
    /// Gets the collection of markers.
    /// </summary>
    public ObservableCollection<MarkerViewModel> Markers { get; } = [];

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
    /// Gets or sets the height of the viewport used for rendering the map.
    /// </summary>
    public float ViewHeight { get; set; }

    /// <summary>
    /// Gets or sets the width of the viewport used for rendering the map.
    /// </summary>
    public float ViewWidth { get; set; }

    /// <summary>
    /// Gets or sets the zoom level.
    /// </summary>
    public float Zoom
    {
        get => this.GetValue<float>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Centers the map at the specified coordinates.
    /// </summary>
    /// <param name="x">The X-coordinate to center the map on.</param>
    /// <param name="y">The Y-coordinate to center the map on.</param>
    public void CenterMap(float x, float y)
    {
        this.CenterX = x;
        this.CenterY = y;

        this.OnInvalidateRequested();
    }

    /// <summary>
    /// Adjusts the zoom level and repositions the map's center based on the specified scale, origin, and view dimensions.
    /// </summary>
    /// <param name="scale">The zoom scale factor to apply.</param>
    /// <param name="originX">The X-coordinate of the zoom origin in the view.</param>
    /// <param name="originY">The Y-coordinate of the zoom origin in the view.</param>
    public void ZoomAndScale(float scale, float originX, float originY)
    {
        var worldX = (originX - (this.ViewWidth / 2f) - this.CenterX) / this.Zoom;
        var worldY = (originY - (this.ViewHeight / 2f) - this.CenterY) / this.Zoom;

        this.Zoom *= scale;
        this.Zoom = Math.Min(Math.Max(this.Zoom, 0.001f), 50.0f);

        this.CenterX = originX - (worldX * this.Zoom) - (this.ViewWidth / 2f);
        this.CenterY = originY - (worldY * this.Zoom) - (this.ViewHeight / 2f);

        this.OnInvalidateRequested();
    }

    /// <summary>
    /// Raises the <see cref="InvalidateRequested" /> event to notify subscribers that the map view requires redrawing.
    /// </summary>
    protected virtual void OnInvalidateRequested()
    {
        this.InvalidateRequested?.Invoke(this, EventArgs.Empty);
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

        this.Markers.Clear();

        foreach (var marker in markers)
        {
            this.Markers.Add(marker.ToViewModel());
        }
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
}