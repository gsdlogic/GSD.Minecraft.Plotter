// <copyright file="IMapLayout.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Defines the layout and mapping logic for plotting markers on a map.
/// </summary>
public interface IMapLayout
{
    /// <summary>
    /// Gets the color used to represent the origin grid lines on the map.
    /// </summary>
    Color OriginGridColor { get; }

    /// <summary>
    /// Gets the color used for the primary grid lines on the map.
    /// </summary>
    Color PrimaryGridColor { get; }

    /// <summary>
    /// Gets the scale of the layout relative to the standard unit.
    /// </summary>
    float Scale { get; }

    /// <summary>
    /// Gets the color used for rendering the secondary grid lines on the map.
    /// </summary>
    Color SecondaryGridColor { get; }

    /// <summary>
    /// Calculates the map coordinates corresponding to the specified marker.
    /// </summary>
    /// <param name="marker">The marker for which the map coordinates are to be calculated.</param>
    /// <returns>
    /// A tuple containing the X and Y coordinates of the marker on the map.
    /// </returns>
    (float X, float Y) GetMapCoordinate(MarkerViewModel marker);

    /// <summary>
    /// Sets the map coordinates for the specified marker.
    /// </summary>
    /// <param name="marker">The marker for which the map coordinates are to be set.</param>
    /// <param name="x">The X coordinate to set for the marker on the map.</param>
    /// <param name="y">The Y coordinate to set for the marker on the map.</param>
    void SetMapCoordinate(MarkerViewModel marker, float x, float y);
}