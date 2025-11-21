// <copyright file="MauiProgram.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.WinUI;

using GSD.Minecraft.Plotter.Abstractions;
using GSD.Minecraft.Plotter.WinUI.Implementations;

/// <summary>
/// Provides methods for creating and configuring the .NET MAUI application.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Creates and configures a new instance of the .NET MAUI application.
    /// </summary>
    /// <returns>
    /// A configured instance of <see cref="MauiApp" /> representing the application.
    /// </returns>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseSharedMauiApp();
        builder.Services.AddSingleton<IPlatformZoomHandler, WindowsZoomHandler>();

        return builder.Build();
    }
}