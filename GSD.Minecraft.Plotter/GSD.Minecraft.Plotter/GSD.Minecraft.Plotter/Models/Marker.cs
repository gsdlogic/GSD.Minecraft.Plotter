// <copyright file="Marker.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Models;

/// <summary>
/// Represents a marker model used for plotting markers on a map.
/// </summary>
public class Marker
{
    /// <summary>
    /// Gets or sets the unique identifier for the marker.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the marker.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the world associated with this marker.
    /// </summary>
    public World World { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated <see cref="World" />.
    /// </summary>
    public int WorldId { get; set; }

    /// <summary>
    /// Gets or sets the X-coordinate of the marker.
    /// </summary>
    public float X { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate of the marker.
    /// </summary>
    public float Y { get; set; }

    /// <summary>
    /// Gets or sets the Z-coordinate of the marker.
    /// </summary>
    public float Z { get; set; }
}