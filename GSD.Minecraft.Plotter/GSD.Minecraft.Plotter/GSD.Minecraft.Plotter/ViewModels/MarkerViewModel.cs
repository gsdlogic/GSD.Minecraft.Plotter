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
    /// Gets or sets the bearing to the pinned marker.
    /// </summary>
    public float Bearing
    {
        get => this.GetValue<float>();
        set
        {
            var floor = (float)Math.Floor(value);
            this.SetValue(floor);
        }
    }

    /// <summary>
    /// Gets or sets the cardinal direction from the pinned marker.
    /// </summary>
    public string Direction
    {
        get => this.GetValue<string>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets or sets the distance from the pinned marker.
    /// </summary>
    public float Distance
    {
        get => this.GetValue<float>();
        set
        {
            var floor = (float)Math.Floor(value);
            this.SetValue(floor);
        }
    }

    /// <summary>
    /// Gets or sets a string representation of the distance, bearing, and cardinal direction
    /// from the current marker to a pinned marker.
    /// </summary>
    public string DistanceBearing
    {
        get => this.GetValue<string>();
        set => this.SetValue(value);
    }

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
    /// Gets the X-coordinate of the marker in the nether.
    /// </summary>
    public float NetherX
    {
        get => this.GetValue<float>();
        private set
        {
            var floor = (float)Math.Floor(value);
            this.SetValue(floor);
        }
    }

    /// <summary>
    /// Gets the Y-coordinate of the marker in the nether.
    /// </summary>
    public float NetherY
    {
        get => this.GetValue<float>();
        private set
        {
            var floor = (float)Math.Floor(value);
            this.SetValue(floor);
        }
    }

    /// <summary>
    /// Gets the Z-coordinate of the marker in the nether.
    /// </summary>
    public float NetherZ
    {
        get => this.GetValue<float>();
        private set
        {
            var floor = (float)Math.Floor(value);
            this.SetValue(floor);
        }
    }

    /// <summary>
    /// Gets or sets the X-coordinate of the marker.
    /// </summary>
    public float X
    {
        get => this.GetValue<float>();
        set
        {
            var floor = (float)Math.Floor(value);
            this.SetValue(floor);
            this.NetherX = floor / 8.0f;
        }
    }

    /// <summary>
    /// Gets or sets the Y-coordinate of the marker.
    /// </summary>
    public float Y
    {
        get => this.GetValue<float>();
        set
        {
            var floor = (float)Math.Floor(value);
            this.SetValue(floor);
            this.NetherY = floor / 8.0f;
        }
    }

    /// <summary>
    /// Gets or sets the Z-coordinate of the marker.
    /// </summary>
    public float Z
    {
        get => this.GetValue<float>();
        set
        {
            var floor = (float)Math.Floor(value);
            this.SetValue(floor);
            this.NetherZ = floor / 8.0f;
        }
    }
}