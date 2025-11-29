// <copyright file="MarkersPageViewModel.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;
using GSD.Minecraft.Plotter.Services;
using GSD.Minecraft.Plotter.Views;

/// <summary>
/// Represents the view model for managing markers in the Minecraft Plotter application.
/// </summary>
public class MarkersPageViewModel : ViewModelBase
{
    /// <summary>
    /// The application state that provides access to the current world and its markers.
    /// </summary>
    private readonly AppState appState;

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkersPageViewModel" /> class.
    /// </summary>
    /// <param name="appState">The application state that provides access to the current world and its markers.</param>
    public MarkersPageViewModel(AppState appState)
    {
        this.appState = appState ?? throw new ArgumentNullException(nameof(appState));
        this.appState.CurrentWorldChanged += this.OnCurrentWorldChanged;
        this.appState.MarkersChanged += this.OnMarkersChanged;

        this.UpdateMarkers();
        this.UpdateTitle();

        this.ImportCommand = new Command(this.ImportMarkers);
        this.AddMarkerCommand = new Command(this.AddMarker);
        this.EditMarkerCommand = new Command<MarkerViewModel>(this.EditMarker);
    }

    /// <summary>
    /// Gets the command that adds a new marker to the collection of markers.
    /// </summary>
    public ICommand AddMarkerCommand { get; }

    /// <summary>
    /// Gets the command that allows editing an existing marker in the collection of markers.
    /// </summary>
    public ICommand EditMarkerCommand { get; }

    /// <summary>
    /// Gets the command that handles the import functionality for markers.
    /// </summary>
    public ICommand ImportCommand { get; }

    /// <summary>
    /// Gets the collection of markers associated with the current world.
    /// </summary>
    public ObservableCollection<MarkerViewModel> Markers { get; } = [];

    /// <summary>
    /// Gets or sets the title of the map page, which dynamically reflects the name of the current world.
    /// </summary>
    public string Title
    {
        get => this.GetValue<string>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Adds a new marker to the collection of markers in the current world.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private async void AddMarker()
    {
        var markerViewModel = new MarkerViewModel
        {
            Name = $"Marker {this.Markers.Count + 1}",
        };

        var pageViewModel = new EditMarkerPageViewModel(this.appState, markerViewModel);
        var page = new EditMarkerPage(pageViewModel);

        await Shell.Current.CurrentPage.Navigation.PushModalAsync(page).ConfigureAwait(true);
    }

    /// <summary>
    /// Edits the specified marker in the current world.
    /// </summary>
    /// <param name="markerViewModel">The marker to be edited, represented by a <see cref="MarkerViewModel" /> instance.</param>
    /// ReSharper disable once AsyncVoidMethod
    private async void EditMarker(MarkerViewModel markerViewModel)
    {
        var pageViewModel = new EditMarkerPageViewModel(this.appState, markerViewModel);
        var page = new EditMarkerPage(pageViewModel);

        await Shell.Current.CurrentPage.Navigation.PushModalAsync(page).ConfigureAwait(true);
    }

    /// <summary>
    /// Imports markers into the application by retrieving and processing marker data.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private async void ImportMarkers()
    {
        var options = new PickOptions
        {
            PickerTitle = "Select CSV file",
            FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, [".csv"] },
                    { DevicePlatform.MacCatalyst, ["public.comma-separated-values-text"] },
                    { DevicePlatform.iOS, ["public.comma-separated-values-text"] },
                    { DevicePlatform.Android, ["text/csv"] },
                }),
        };

        var result = await FilePicker.Default.PickAsync(options).ConfigureAwait(false);

        if (result == null)
        {
            return;
        }

        var stream = await result.OpenReadAsync().ConfigureAwait(true);
        await using var streamAsyncDisposable = stream.ConfigureAwait(false);

        await this.appState.ImportMarkersAsync(stream).ConfigureAwait(false);
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
    /// Updates the collection of markers to reflect the current state of the application.
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
    /// Updates the title of the markers page to reflect the name of the current world.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private async void UpdateTitle()
    {
        var world = await this.appState.GetCurrentWorldAsync().ConfigureAwait(false);
        this.Title = world.Name;
    }
}