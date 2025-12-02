// <copyright file="NetherMapLayout.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Represents the layout and mapping logic specific to the Nether map in the Minecraft Plotter application.
/// </summary>
public class NetherMapLayout : IMapLayout
{
    /// <summary>
    /// Gets the color used to represent the origin grid lines on the map.
    /// </summary>
    public Color OriginGridColor => Color.FromArgb("#FFFF0000");

    /// <summary>
    /// Gets the color used for the primary grid lines on the map.
    /// </summary>
    public Color PrimaryGridColor => Color.FromArgb("#FF990000");

    /// <summary>
    /// Gets the scale of the layout relative to the standard unit.
    /// </summary>
    public float Scale => 1.0f / 8.0f;

    /// <summary>
    /// Gets the color used for rendering the secondary grid lines on the map.
    /// </summary>
    public Color SecondaryGridColor => Color.FromArgb("#FF660000");

    /// <summary>
    /// Calculates the map coordinates corresponding to the specified marker.
    /// </summary>
    /// <param name="marker">The marker for which the map coordinates are to be calculated.</param>
    /// <returns>
    /// A tuple containing the X and Y coordinates of the marker on the map.
    /// </returns>
    public (float X, float Y) GetMapCoordinate(MarkerViewModel marker)
    {
        ArgumentNullException.ThrowIfNull(marker);
        return (marker.X / 8.0f, marker.Z / 8.0f);
    }
}