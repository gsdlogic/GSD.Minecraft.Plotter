// <copyright file="MarkerViewModel.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Represents a marker view model used for plotting markers on a map.
/// </summary>
public class MarkerViewModel : ViewModelBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the marker.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the marker.
    /// </summary>
    public string Name
    {
        get => this.GetValue<string>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets or sets the X-coordinate of the marker.
    /// </summary>
    public float X
    {
        get => this.GetValue<float>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets or sets the Y-coordinate of the marker.
    /// </summary>
    public float Y
    {
        get => this.GetValue<float>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets or sets the Z-coordinate of the marker.
    /// </summary>
    public float Z
    {
        get => this.GetValue<float>();
        set => this.SetValue(value);
    }
}