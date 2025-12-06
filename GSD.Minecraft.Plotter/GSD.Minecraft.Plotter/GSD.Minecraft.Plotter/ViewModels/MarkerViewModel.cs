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
    /// The cardinal directions.
    /// </summary>
    private readonly string[] directions =
    [
        "N", "NNE", "NE", "ENE",
        "E", "ESE", "SE", "SSE",
        "S", "SSW", "SW", "WSW",
        "W", "WNW", "NW", "NNW",
    ];

    /// <summary>
    /// Gets the bearing to the pinned marker.
    /// </summary>
    public float Bearing
    {
        get => this.GetValue<float>();
        private set
        {
            var floor = (float)Math.Floor(value);
            this.SetValue(floor);
        }
    }

    /// <summary>
    /// Gets the cardinal direction from the pinned marker.
    /// </summary>
    public string Direction
    {
        get => this.GetValue<string>();
        private set => this.SetValue(value);
    }

    /// <summary>
    /// Gets the distance from the pinned marker.
    /// </summary>
    public float Distance
    {
        get => this.GetValue<float>();
        private set
        {
            var floor = (float)Math.Floor(value);
            this.SetValue(floor);
        }
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
    /// Gets or sets a string representation of the distance, bearing, and cardinal direction
    /// from the current marker to a pinned marker.
    /// </summary>
    public string DistanceBearing
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

    /// <summary>
    /// Updates the pinned marker.
    /// </summary>
    /// <param name="pinned">The pinned marker.</param>
    public void PinTo(MarkerViewModel pinned)
    {
        const int Slices = 360 / 16;

        if (pinned == null)
        {
            this.Distance = 0.0f;
            return;
        }

        this.Distance = (float)Math.Sqrt(Math.Pow(this.X - pinned.X, 2) + Math.Pow(this.Z - pinned.Z, 2));
        this.Bearing = (float)(Math.Atan2(this.X - pinned.X, pinned.Z - this.Z) * (180.0 / Math.PI));
        this.Direction = this.directions[(((int)this.Bearing + 360) / Slices) % 16];
        this.DistanceBearing = $"{this.Distance}, {this.Bearing}° {this.Direction}";
    }
}