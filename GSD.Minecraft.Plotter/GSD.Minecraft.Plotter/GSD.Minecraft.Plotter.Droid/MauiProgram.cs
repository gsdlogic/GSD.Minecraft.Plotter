// <copyright file="MauiProgram.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Droid;

using GSD.Minecraft.Plotter.Abstractions;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseSharedMauiApp();
        builder.Services.AddSingleton<IPlatformZoomHandler, DefaultZoomHandler>();

        return builder.Build();
    }
}