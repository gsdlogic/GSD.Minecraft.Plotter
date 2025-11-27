// <copyright file="MarkersPageViewModel.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;
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

        appState.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(AppState.CurrentWorld))
            {
                this.Markers = appState.CurrentWorld.Markers;
                this.Title = $"Markers - {appState.CurrentWorld.Name}";
                this.OnPropertyChanged(nameof(this.Markers));
            }
        };

        this.Markers = appState.CurrentWorld.Markers;
        this.Title = $"Markers - {appState.CurrentWorld.Name}";

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
    /// Gets the collection of markers associated with the current world.
    /// </summary>
    public ObservableCollection<MarkerViewModel> Markers { get; private set; }

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
            X = 0,
            Y = 0,
            Z = 0,
            Name = $"Marker {this.Markers.Count + 1}",
            FillColor = Colors.White,
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
}