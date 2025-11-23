// <copyright file="MapPageViewModel.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Represents the view model for the map, providing data and commands to support map-related functionality.
/// </summary>
public class MapPageViewModel : ViewModelBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MapPageViewModel" /> class.
    /// </summary>
    /// <param name="appState">The current application state used to synchronize the map's drawable elements.</param>
    public MapPageViewModel(AppState appState)
    {
        ArgumentNullException.ThrowIfNull(appState);

        appState.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(AppState.CurrentWorld))
            {
                this.MapDrawable.Markers = appState.CurrentWorld.Markers;
            }
        };

        this.MapDrawable.Markers = appState.CurrentWorld.Markers;
    }

    /// <summary>
    /// Gets or sets the map drawable.
    /// </summary>
    public MapDrawable MapDrawable { get; set; } = new();
}