// <copyright file="World.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Models;

using System.Collections.ObjectModel;
using GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Represents the model for a single world in the application.
/// </summary>
public class World : ViewModelBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the world.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets the collection of markers associated with this world.
    /// </summary>
    public Collection<Marker> Markers { get; } = [];

    /// <summary>
    /// Gets or sets the name of the world.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the seed value for the world.
    /// </summary>
    public string Seed { get; set; }
}