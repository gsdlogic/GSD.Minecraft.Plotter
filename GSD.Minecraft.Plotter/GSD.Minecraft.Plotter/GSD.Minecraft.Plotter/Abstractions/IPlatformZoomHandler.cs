// <copyright file="IPlatformZoomHandler.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Abstractions;

/// <summary>
/// Defines a contract for handling platform-specific zoom functionality.
/// </summary>
public interface IPlatformZoomHandler
{
    /// <summary>
    /// Attaches the platform-specific zoom handler to the specified graphics view.
    /// </summary>
    /// <param name="view">The <see cref="GraphicsView" /> to which the zoom handler will be attached.</param>
    /// <param name="zoomCallback">A callback function that is invoked with the zoom factor when a zoom action occurs.</param>
    void AttachTo(GraphicsView view, Action<float, float, float> zoomCallback);
}