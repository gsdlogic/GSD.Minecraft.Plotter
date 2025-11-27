// <copyright file="MauiProgram.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.WinUI;

using GSD.Minecraft.Plotter.Abstractions;
using GSD.Minecraft.Plotter.WinUI.Implementations;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using WinRT.Interop;

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

        builder.UseSharedMauiApp()
            .ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(windows =>
                {
                    windows.OnWindowCreated(window =>
                    {
                        const int Width = 800;
                        const int Height = 1200;

                        var hwnd = WindowNative.GetWindowHandle(window);
                        var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
                        var appWindow = AppWindow.GetFromWindowId(windowId);

                        appWindow.Resize(new SizeInt32(Width, Height));
                    });
                });
            });

        builder.Services.AddSingleton<IPlatformZoomHandler, WindowsZoomHandler>();

        return builder.Build();
    }
}