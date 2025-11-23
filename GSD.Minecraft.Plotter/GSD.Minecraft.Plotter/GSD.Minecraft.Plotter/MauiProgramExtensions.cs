// <copyright file="MauiProgramExtensions.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter;

using GSD.Minecraft.Plotter.ViewModels;
using Microsoft.Extensions.Logging;

/// <summary>
/// Provides extension methods for configuring and enhancing the <see cref="MauiAppBuilder" />.
/// </summary>
public static class MauiProgramExtensions
{
    /// <summary>
    /// Configures the <see cref="MauiAppBuilder" /> with shared application settings,  such as fonts and platform-specific logging.
    /// </summary>
    /// <param name="builder">The <see cref="MauiAppBuilder" /> instance to configure.</param>
    /// <returns>
    /// The configured <see cref="MauiAppBuilder" /> instance, enabling method chaining.
    /// </returns>
    public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddTransient<MapViewModel>();
        builder.Services.AddTransient<MarkersViewModel>();
        builder.Services.AddTransient<WorldsViewModel>();

        return builder;
    }
}