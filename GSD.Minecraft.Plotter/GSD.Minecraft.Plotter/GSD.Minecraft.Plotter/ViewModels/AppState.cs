// <copyright file="AppState.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Represents the application state, providing properties and functionality to manage and interact with the current state of the application.
/// </summary>
public class AppState : ViewModelBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppState" /> class.
    /// </summary>
    public AppState()
    {
        this.CurrentWorld = new WorldViewModel
        {
            Name = "Default World",
        };

        this.CurrentWorld.Markers.Add(new MarkerViewModel { Name = "Center", X = 0, Y = 0, FillColor = Colors.White });
        this.CurrentWorld.Markers.Add(new MarkerViewModel { Name = "Top left", X = -8, Y = -8, FillColor = Colors.Red });
        this.CurrentWorld.Markers.Add(new MarkerViewModel { Name = "Top right", X = 8, Y = -8, FillColor = Colors.Yellow });
        this.CurrentWorld.Markers.Add(new MarkerViewModel { Name = "Bottom left", X = -8, Y = 8, FillColor = Colors.Cyan });
        this.CurrentWorld.Markers.Add(new MarkerViewModel { Name = "Bottom right", X = 8, Y = 8, FillColor = Colors.LightGreen });
    }

    /// <summary>
    /// Gets or sets the current world being managed or interacted with in the application.
    /// </summary>
    public WorldViewModel CurrentWorld
    {
        get => this.GetValue<WorldViewModel>();
        set => this.SetValue(value);
    }
}