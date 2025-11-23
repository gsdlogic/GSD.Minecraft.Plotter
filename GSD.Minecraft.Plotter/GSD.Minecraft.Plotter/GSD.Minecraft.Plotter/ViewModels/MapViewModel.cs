// <copyright file="MapViewModel.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Represents the view model for the map, providing data and commands to support map-related functionality.
/// </summary>
public class MapViewModel : ViewModelBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MapViewModel" /> class.
    /// </summary>
    public MapViewModel()
    {
        this.MapDrawable.Markers.Add(new MarkerViewModel { X = 0, Y = 0, FillColor = Colors.White });
        this.MapDrawable.Markers.Add(new MarkerViewModel { X = -8, Y = -8, FillColor = Colors.Red });
        this.MapDrawable.Markers.Add(new MarkerViewModel { X = 8, Y = -8, FillColor = Colors.Yellow });
        this.MapDrawable.Markers.Add(new MarkerViewModel { X = -8, Y = 8, FillColor = Colors.Cyan });
        this.MapDrawable.Markers.Add(new MarkerViewModel { X = 8, Y = 8, FillColor = Colors.LightGreen });
    }

    /// <summary>
    /// Gets or sets the map drawable.
    /// </summary>
    public MapDrawable MapDrawable { get; set; } = new();
}