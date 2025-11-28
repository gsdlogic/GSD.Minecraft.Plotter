// <copyright file="OverworldMapLayout.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Represents the layout logic for plotting markers on an overworld map in Minecraft.
/// </summary>
public class OverworldMapLayout : IMapLayout
{
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
        return (marker.X, marker.Z);
    }
}