// <copyright file="WorldViewModel.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using System.Collections.ObjectModel;

/// <summary>
/// Represents the view model for a single world in the application.
/// </summary>
public class WorldViewModel : ViewModelBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the world.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the world represented by this view model.
    /// </summary>
    public string Name
    {
        get => this.GetValue<string>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets or sets the seed value for the world.
    /// </summary>
    public string Seed
    {
        get => this.GetValue<string>();
        set => this.SetValue(value);
    }
}