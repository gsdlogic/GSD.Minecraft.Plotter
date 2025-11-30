// <copyright file="WindowsZoomHandler.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.WinUI.Implementations;

using GSD.Minecraft.Plotter.Abstractions;
using Microsoft.UI.Xaml;

/// <summary>
/// Defines a contract for handling platform-specific zoom functionality.
/// </summary>
public class WindowsZoomHandler : IPlatformZoomHandler
{
    /// <summary>
    /// Attaches the platform-specific zoom handler to the specified graphics view.
    /// </summary>
    /// <param name="view">The <see cref="GraphicsView" /> to which the zoom handler will be attached.</param>
    /// <param name="zoomCallback">A callback function that is invoked with the zoom factor when a zoom action occurs.</param>
    public void AttachTo(GraphicsView view, Action<float, float, float> zoomCallback)
    {
        ArgumentNullException.ThrowIfNull(view);

        view.HandlerChanged += (_, _) =>
        {
            if (view.Handler?.PlatformView is UIElement element)
            {
                element.PointerWheelChanged += (s, e) =>
                {
                    var point = e.GetCurrentPoint((UIElement)s);
                    var factor = point.Properties.MouseWheelDelta > 0 ? 1.1f : 0.9f;

                    zoomCallback(
                        factor,
                        (float)point.Position.X,
                        (float)point.Position.Y);
                };
            }
        };
    }
}