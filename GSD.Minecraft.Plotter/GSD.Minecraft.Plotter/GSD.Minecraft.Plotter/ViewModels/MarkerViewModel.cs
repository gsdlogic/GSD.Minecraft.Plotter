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
    /// Gets or sets the fill color used to visually represent the marker.
    /// </summary>
    public Color FillColor
    {
        get => this.GetValue<Color>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets or sets the icon representing the marker.
    /// </summary>
    public IImage Icon
    {
        get => this.GetValue<IImage>();
        set => this.SetValue(value);
    }

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