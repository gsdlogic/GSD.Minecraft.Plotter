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
    /// The cardinal directions.
    /// </summary>
    private readonly string[] directions =
    [
        "N", "NNE", "NE", "ENE",
        "E", "ESE", "SE", "SSE",
        "S", "SSW", "SW", "WSW",
        "W", "WNW", "NW", "NNW",
    ];

    /// <summary>
    /// Indicates whether a new world has been loaded or initialized.
    /// </summary>
    private bool newWorld;

    /// <summary>
    /// Initializes a new instance of the <see cref="MapPageViewModel" /> class.
    /// </summary>
    /// <param name="appState">The current application state used to synchronize the map's drawable elements.</param>
    public MapPageViewModel(AppState appState)
    {
        this.appState = appState ?? throw new ArgumentNullException(nameof(appState));
        this.appState.CurrentWorldChanged += this.OnCurrentWorldChanged;
        this.appState.MarkersChanged += this.OnMarkersChanged;

        this.OverworldCommand = new Command(() => this.SetLayout(new OverworldMapLayout()));
        this.NetherCommand = new Command(() => this.SetLayout(new NetherMapLayout()));
        this.PinCommand = new Command(this.PinSelectedMarker);
        this.ToggleAlignmentCommand = new Command(this.ToggleAlignmentButtons);
        this.ZoomToChunkCommand = new Command(this.ZoomToChunk);
        this.ZoomToExtentsCommand = new Command(this.ZoomToExtents);
        this.MoveMarkerCommand = new Command(this.MoveMarker);

        this.Camera = new Camera();
        this.Layout = new OverworldMapLayout();
        this.Drawable = new MapDrawable(this);

        this.UpdateMarkers();
        this.UpdateTitle();
    }

    /// <summary>
    /// Gets the camera view model that manages viewport transformations, zooming, and panning
    /// for rendering and interacting with the 2D map.
    /// </summary>
    public Camera Camera { get; }

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
            this.UpdatePinnedMarker();
        }
    }

    /// <summary>
    /// Gets the collection of markers.
    /// </summary>
    public ObservableCollection<MarkerViewModel> Markers { get; } = [];

    /// <summary>
    /// Gets the command to move the marker.
    /// </summary>
    public ICommand MoveMarkerCommand { get; }

    /// <summary>
    /// Gets the command to set the layout relative to the Nether map.
    /// </summary>
    public ICommand NetherCommand { get; }

    /// <summary>
    /// Gets the command to set the layout relative to the Overworld map.
    /// </summary>
    public ICommand OverworldCommand { get; }

    /// <summary>
    /// Gets the command to pin the selected marker.
    /// </summary>
    public ICommand PinCommand { get; }

    /// <summary>
    /// Gets or sets the currently selected marker on the map.
    /// </summary>
    public MarkerViewModel PinnedMarker
    {
        get => this.GetValue<MarkerViewModel>();
        set
        {
            this.SetValue(value);
            this.UpdatePinnedMarker();
        }
    }

    /// <summary>
    /// Gets or sets the currently selected marker on the map.
    /// </summary>
    public MarkerViewModel SelectedMarker
    {
        get => this.GetValue<MarkerViewModel>();
        set
        {
            this.SetValue(value);
            this.UpdatePinnedMarker();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the chunk alignment buttons are displayed on the map.
    /// </summary>
    public bool ShowAlignmentButtons
    {
        get => this.GetValue<bool>();
        set
        {
            this.SetValue(value);

            if (value)
            {
                this.ShowPinnedCoordinates = false;
            }
            else if ((this.PinnedMarker != null) && !this.ShowPinnedCoordinates)
            {
                this.ShowPinnedCoordinates = true;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the pinned coordinates should be displayed.
    /// </summary>
    public bool ShowPinnedCoordinates
    {
        get => this.GetValue<bool>();
        set
        {
            this.SetValue(value);

            if (value)
            {
                this.ShowAlignmentButtons = false;
            }
        }
    }

    /// <summary>
    /// Gets or sets the title of the map page, which dynamically reflects the name of the current world.
    /// </summary>
    public string Title
    {
        get => this.GetValue<string>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets the command that toggles the visibility of alignment buttons on the map page.
    /// </summary>
    public ICommand ToggleAlignmentCommand { get; }

    /// <summary>
    /// Gets the command that zooms the map view to the currently selected chunk.
    /// </summary>
    public ICommand ZoomToChunkCommand { get; }

    /// <summary>
    /// Gets the command that zooms the map view to fit all drawable elements within the visible area.
    /// </summary>
    public ICommand ZoomToExtentsCommand { get; }

    /// <summary>
    /// Handles the initialization logic when the map page is loaded.
    /// This method centers the map on a predefined world coordinate and scales it to fit a specific radius.
    /// </summary>
    public void OnLoad()
    {
        this.ZoomToExtents();
    }

    /// <summary>
    /// Selects or interacts with an element on the map at the specified world coordinates.
    /// </summary>
    /// <param name="worldX">The X-coordinate in world space where the interaction occurs.</param>
    /// <param name="worldY">The Y-coordinate in world space where the interaction occurs.</param>
    public void Pick(float worldX, float worldY)
    {
        var radius = 1;

        while (this.Camera.Zoom * radius < 16)
        {
            radius *= 2;
        }

        var markers = this.Markers.Where(m =>
            {
                var point = this.Layout.GetMapCoordinate(m);
                return (Math.Abs(point.X - worldX) <= radius) &&
                       (Math.Abs(point.Y - worldY) <= radius);
            })
            .ToList();

        switch (markers.Count)
        {
            case 0:
                this.SelectedMarker = null;
                break;

            case 1:
                this.SelectedMarker = markers[0];
                break;

            default:
            {
                var index = (markers.IndexOf(this.SelectedMarker) + 1) % markers.Count;
                this.SelectedMarker = markers[index];
                break;
            }
        }

        this.SelectedMarker ??= new MarkerViewModel
        {
            Name = "World Coordinate",
            X = worldX,
            Y = 0,
            Z = worldY,
        };

        this.Camera.Invalidate();
    }

    /// <summary>
    /// Updates the calculations for the pinned marker.
    /// </summary>
    public void UpdatePinnedMarker()
    {
        const int Slices = 360 / 16;

        var marker = this.SelectedMarker;
        var pinned = this.PinnedMarker;

        if (marker == null)
        {
            return;
        }

        if (pinned == null)
        {
            marker.Distance = 0.0f;
            marker.Bearing = 0.0f;
            marker.Direction = null;
            marker.DistanceBearing = null;
            return;
        }

        var markerPoint = this.Layout.GetMapCoordinate(marker);
        var pinnedPoint = this.Layout.GetMapCoordinate(pinned);

        marker.Distance = (float)Math.Sqrt(Math.Pow(markerPoint.X - pinnedPoint.X, 2) + Math.Pow(markerPoint.Y - pinnedPoint.Y, 2));
        marker.Bearing = (float)(Math.Atan2(markerPoint.X - pinnedPoint.X, pinnedPoint.Y - markerPoint.Y) * (180.0 / Math.PI));
        marker.Direction = this.directions[(((int)marker.Bearing + 360) / Slices) % 16];
        marker.DistanceBearing = $"{marker.Distance}, {marker.Bearing}° {marker.Direction}";
    }

    /// <summary>
    /// Moves the selected marker in the cardinal direction.
    /// </summary>
    /// <param name="obj">An integer in the range of 0 to 8.</param>
    /// ReSharper disable once AsyncVoidMethod
    private async void MoveMarker(object obj)
    {
        var marker = this.SelectedMarker;

        if ((marker == null) ||
            obj is not string value ||
            !int.TryParse(value, out var direction))
        {
            return;
        }

        var point = this.Layout.GetMapCoordinate(marker);
        var chunkX = (float)Math.Floor(point.X / 16.0f) * 16.0f;
        var chunkY = (float)Math.Floor(point.Y / 16.0f) * 16.0f;

        switch (direction)
        {
            case 0:
                this.Layout.SetMapCoordinate(marker, chunkX, chunkY);
                break;

            case 1:
                this.Layout.SetMapCoordinate(marker, chunkX + 8.0f, chunkY);
                break;

            case 2:
                this.Layout.SetMapCoordinate(marker, chunkX + 15.0f, chunkY);
                break;

            case 3:
                this.Layout.SetMapCoordinate(marker, chunkX, chunkY + 8.0f);
                break;

            case 4:
                this.Layout.SetMapCoordinate(marker, chunkX + 8.0f, chunkY + 8.0f);
                break;

            case 5:
                this.Layout.SetMapCoordinate(marker, chunkX + 15.0f, chunkY + 8.0f);
                break;

            case 6:
                this.Layout.SetMapCoordinate(marker, chunkX, chunkY + 15.0f);
                break;

            case 7:
                this.Layout.SetMapCoordinate(marker, chunkX + 8.0f, chunkY + 15.0f);
                break;

            case 8:
                this.Layout.SetMapCoordinate(marker, chunkX + 15.0f, chunkY + 15.0f);
                break;
        }

        if (this.Markers.Any(m => m.Id == marker.Id))
        {
            await this.appState.AddOrUpdateMarkerAsync(marker.ToModel()).ConfigureAwait(false);
            this.SelectedMarker = this.Markers.FirstOrDefault(m => m.Id == marker.Id);
        }

        this.Camera.Invalidate();
    }

    /// <summary>
    /// Occurs when the current world in the application state changes.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An <see cref="EventArgs" /> containing no event data.</param>
    private void OnCurrentWorldChanged(object sender, EventArgs e)
    {
        this.UpdateTitle();
        this.newWorld = true;
    }

    /// <summary>
    /// Occurs when the collection of markers in the application state is modified.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An <see cref="EventArgs" /> that contains no event data.</param>
    private void OnMarkersChanged(object sender, EventArgs e)
    {
        this.UpdateMarkers();

        if (this.newWorld)
        {
            this.ZoomToExtents();
            this.newWorld = false;
        }
    }

    /// <summary>
    /// Pins the selected marker.
    /// </summary>
    private void PinSelectedMarker()
    {
        this.PinnedMarker = this.PinnedMarker == this.SelectedMarker ? null : this.SelectedMarker;
        this.ShowPinnedCoordinates = true;
        this.Camera.Invalidate();
    }

    /// <summary>
    /// Updates the map layout and adjusts the camera to maintain the current view's center position.
    /// </summary>
    /// <param name="layout">The new map layout to apply.</param>
    private void SetLayout(IMapLayout layout)
    {
        if (layout == this.Layout)
        {
            return;
        }

        var scaleRatio = layout.Scale / this.Layout.Scale;
        this.Layout = layout;

        this.Camera.ScaleCenter(scaleRatio);
    }

    /// <summary>
    /// Toggles the visibility of alignment buttons on the map page.
    /// </summary>
    private void ToggleAlignmentButtons()
    {
        this.ShowAlignmentButtons = !this.ShowAlignmentButtons;
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

        if (this.Markers.Count > 0)
        {
            this.SelectedMarker = this.Markers[0];
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

    /// <summary>
    /// Centers the camera on the chunk containing the currently selected marker and adjusts the zoom level.
    /// </summary>
    private void ZoomToChunk()
    {
        var point = this.Layout.GetMapCoordinate(this.SelectedMarker);
        var chunkX = (float)Math.Floor(point.X / 16.0f) * 16.0f;
        var chunkY = (float)Math.Floor(point.Y / 16.0f) * 16.0f;

        this.Camera.CenterAndFit(chunkX + 8.0f, chunkY + 8.0f, 10);
    }

    /// <summary>
    /// Adjusts the map view to ensure all markers are visible within the viewport.
    /// </summary>
    private void ZoomToExtents()
    {
        if (this.Markers.Count == 0)
        {
            this.Camera.CenterAndFit(0.0f, 0.0f, 64.0f);
            return;
        }

        var minX = this.Markers.Min(m => this.Layout.GetMapCoordinate(m).X);
        var minY = this.Markers.Min(m => this.Layout.GetMapCoordinate(m).Y);
        var maxX = this.Markers.Max(m => this.Layout.GetMapCoordinate(m).X);
        var maxY = this.Markers.Max(m => this.Layout.GetMapCoordinate(m).Y);

        var radiusX = (maxX - minX) / 2.0f;
        var radiusY = (maxY - minY) / 2.0f;
        var centerX = minX + radiusX;
        var centerY = minY + radiusY;
        var radius = Math.Max(radiusX, radiusY);

        this.Camera.CenterAndFit(centerX, centerY, radius * 1.5f);
    }
}