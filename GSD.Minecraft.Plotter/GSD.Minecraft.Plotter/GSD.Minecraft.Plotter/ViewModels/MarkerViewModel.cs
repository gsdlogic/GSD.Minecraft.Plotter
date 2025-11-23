// <copyright file="MarkerViewModel.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using IImage = Microsoft.Maui.Graphics.IImage;

/// <summary>
/// Represents a marker view model used for plotting markers on a map.
/// </summary>
public class MarkerViewModel : ViewModelBase
{
    /// <summary>
    /// Gets or sets the fill color used to visually represent the point of interest (POI) on the map.
    /// </summary>
    public Color FillColor { get; set; }

    /// <summary>
    /// Gets or sets the icon representing the point of interest (POI).
    /// </summary>
    public IImage Icon { get; set; }

    /// <summary>
    /// Gets or sets the name of the point of interest (POI).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the X-coordinate of the point of interest (POI) on the map.
    /// </summary>
    public float X { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate of the point of interest (POI) on the map.
    /// </summary>
    public float Y { get; set; }
}