// <copyright file="Camera.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Represents a camera view model that manages viewport transformations, zooming, and panning
/// for rendering and interacting with a 2D world space.
/// </summary>
public class Camera : ViewModelBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Camera" /> class.
    /// </summary>
    public Camera()
    {
        this.Zoom = 1.0f;
    }

    /// <summary>
    /// Occurs when the camera's state is invalidated, typically as a result of changes
    /// to its position, zoom level, or viewport dimensions.
    /// </summary>
    public event EventHandler Invalidated;

    /// <summary>
    /// Gets or sets the X-component for the center of the world relative to the view.
    /// </summary>
    public float OffsetX
    {
        get => this.GetValue<float>();
        protected set => this.SetValue(value);
    }

    /// <summary>
    /// Gets or sets the Y-component for the center of the world relative to the view.
    /// </summary>
    public float OffsetY
    {
        get => this.GetValue<float>();
        protected set => this.SetValue(value);
    }

    /// <summary>
    /// Gets or sets the height of the viewport.
    /// </summary>
    public float ViewportHeight
    {
        get => this.GetValue<float>();
        protected set => this.SetValue(value);
    }

    /// <summary>
    /// Gets or sets the width of the viewport.
    /// </summary>
    public float ViewportWidth
    {
        get => this.GetValue<float>();
        protected set => this.SetValue(value);
    }

    /// <summary>
    /// Gets or sets the zoom level.
    /// </summary>
    public float Zoom
    {
        get => this.GetValue<float>();
        protected set => this.SetValue(value);
    }

    /// <summary>
    /// Centers the camera on a specified world position and adjusts the zoom level
    /// to fit a circular area defined by the given radius within the viewport.
    /// </summary>
    /// <param name="worldX">The X-coordinate of the world position to center on.</param>
    /// <param name="worldY">The Y-coordinate of the world position to center on.</param>
    /// <param name="worldRadius">The radius of the circular area in world units to fit within the viewport.</param>
    public void CenterAndFit(float worldX, float worldY, float worldRadius)
    {
        if ((this.ViewportWidth <= 0) || (this.ViewportHeight <= 0) || (worldRadius <= 0))
        {
            return;
        }

        var zoomX = this.ViewportWidth / (2f * worldRadius);
        var zoomY = this.ViewportHeight / (2f * worldRadius);

        this.Zoom = Math.Min(Math.Max(Math.Min(zoomX, zoomY), 0.001f), 50f);

        this.CenterOnWorld(worldX, worldY);
    }

    /// <summary>
    /// Centers the camera on a specified world position.
    /// </summary>
    /// <param name="worldX">The X-coordinate of the world position to center on.</param>
    /// <param name="worldY">The Y-coordinate of the world position to center on.</param>
    public void CenterOnWorld(float worldX, float worldY)
    {
        this.OffsetX = -worldX * this.Zoom;
        this.OffsetY = -worldY * this.Zoom;

        this.Invalidate();
    }

    /// <summary>
    /// Notifies all subscribers that the camera's state has been invalidated.
    /// </summary>
    public void Invalidate()
    {
        this.Invalidated?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Scales the camera's view by a specified factor, adjusting the zoom level
    /// and re-centering the camera on the world coordinates corresponding to the
    /// center of the viewport.
    /// </summary>
    /// <param name="scale">The scaling factor to apply to the camera's view.</param>
    public void ScaleCenter(float scale)
    {
        var worldX = this.ScreenToWorldX(this.ViewportWidth / 2.0f);
        var worldY = this.ScreenToWorldY(this.ViewportHeight / 2.0f);

        var newWorldX = worldX * scale;
        var newWorldY = worldY * scale;

        this.Zoom *= 1.0f / scale;

        this.CenterOnWorld(newWorldX, newWorldY);
    }

    /// <summary>
    /// Converts a screen coordinate on the X-axis to a world coordinate on the X-axis
    /// based on the current camera settings, including zoom level and center offset.
    /// </summary>
    /// <param name="screenX">The X-coordinate in screen units to be converted.</param>
    /// <returns>The corresponding X-coordinate in world units.</returns>
    public float ScreenToWorldX(float screenX)
    {
        return (screenX - (this.ViewportWidth / 2f) - this.OffsetX) / this.Zoom;
    }

    /// <summary>
    /// Converts a screen coordinate on the Y-axis to a world coordinate on the Y-axis
    /// based on the current camera settings, including zoom level and center offset.
    /// </summary>
    /// <param name="screenY">The Y-coordinate in screen units to be converted.</param>
    /// <returns>The corresponding Y-coordinate in world units.</returns>
    public float ScreenToWorldY(float screenY)
    {
        return (screenY - (this.ViewportHeight / 2f) - this.OffsetY) / this.Zoom;
    }

    /// <summary>
    /// Sets the camera's offset in view-space coordinates, enabling manual panning
    /// by specifying the new horizontal and vertical offset values.
    /// </summary>
    /// <param name="offsetX">The horizontal offset in screen-space coordinates.</param>
    /// <param name="offsetY">The vertical offset in screen-space coordinates.</param>
    public void SetOffset(float offsetX, float offsetY)
    {
        this.OffsetX = offsetX;
        this.OffsetY = offsetY;

        this.Invalidate();
    }

    /// <summary>
    /// Sets the dimensions of the viewport in view-space coordinates.
    /// </summary>
    /// <param name="width">The width of the viewport in screen-space coordinates.</param>
    /// <param name="height">The height of the viewport in screen-space coordinates.</param>
    public void SetViewport(float width, float height)
    {
        this.ViewportWidth = width;
        this.ViewportHeight = height;
    }

    /// <summary>
    /// Converts a world coordinate on the X-axis to a screen coordinate on the X-axis
    /// based on the current camera settings, including zoom level and center offset.
    /// </summary>
    /// <param name="worldX">The X-coordinate in world units to be converted.</param>
    /// <returns>The corresponding X-coordinate in screen units.</returns>
    public float WorldToScreenX(float worldX)
    {
        return (this.ViewportWidth / 2f) + this.OffsetX + (worldX * this.Zoom);
    }

    /// <summary>
    /// Converts a world coordinate on the Y-axis to a screen coordinate on the Y-axis
    /// based on the current camera settings, including zoom level and center offset.
    /// </summary>
    /// <param name="worldY">The Y-coordinate in world units to be converted.</param>
    /// <returns>The corresponding Y-coordinate in screen units.</returns>
    public float WorldToScreenY(float worldY)
    {
        return (this.ViewportHeight / 2f) + this.OffsetY + (worldY * this.Zoom);
    }

    /// <summary>
    /// Adjusts the zoom level of the camera at a specific screen coordinate, ensuring
    /// the specified point remains centered in the viewport after zooming.
    /// </summary>
    /// <param name="screenX">The X-coordinate on the screen where the zoom operation is centered.</param>
    /// <param name="screenY">The Y-coordinate on the screen where the zoom operation is centered.</param>
    /// <param name="scale">The scaling factor to adjust the zoom level by. Values greater than 1 zoom in, and values less than 1 zoom out.</param>
    public void ZoomAt(float screenX, float screenY, float scale)
    {
        var worldX = this.ScreenToWorldX(screenX);
        var worldY = this.ScreenToWorldY(screenY);

        this.Zoom = Math.Min(Math.Max(this.Zoom * scale, 0.001f), 50f);

        this.OffsetX = screenX - (worldX * this.Zoom) - (this.ViewportWidth / 2f);
        this.OffsetY = screenY - (worldY * this.Zoom) - (this.ViewportHeight / 2f);

        this.Invalidate();
    }
}