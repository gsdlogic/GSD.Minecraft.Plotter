// <copyright file="MapDrawable.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using System.Collections.ObjectModel;

/// <summary>
/// Defines an object that can be drawn onto a canvas.
/// </summary>
public class MapDrawable : ViewModelBase, IDrawable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MapDrawable" /> class.
    /// </summary>
    public MapDrawable()
    {
        this.Zoom = 30.0f;
    }

    /// <summary>
    /// Gets or sets the X-component for the center of the map.
    /// </summary>
    public float CenterX
    {
        get => this.GetValue<float>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets or sets the Y-component for the center of the map.
    /// </summary>
    public float CenterY
    {
        get => this.GetValue<float>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Gets the collection of POIs.
    /// </summary>
    public ObservableCollection<Poi> Points { get; } = [];

    /// <summary>
    /// Gets or sets the zoom level.
    /// </summary>
    public float Zoom
    {
        get => this.GetValue<float>();
        set => this.SetValue(value);
    }

    /// <summary>
    /// Draws the content onto the specified canvas within the given rectangle.
    /// </summary>
    /// <param name="canvas">The canvas to draw onto.</param>
    /// <param name="dirtyRect">The rectangle in which to draw.</param>
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        ArgumentNullException.ThrowIfNull(canvas);

        canvas.SaveState();

        var camera = new Camera(dirtyRect, this.CenterX, this.CenterY, this.Zoom);

        DrawGrid(canvas, camera);
        this.DrawPoints(canvas, camera);

        canvas.RestoreState();
    }

    /// <summary>
    /// Draws a grid onto the specified canvas using the provided camera settings.
    /// </summary>
    /// <param name="canvas">The canvas on which the grid will be drawn.</param>
    /// <param name="camera">
    /// The camera that provides the necessary transformations for converting  world coordinates into screen coordinates.
    /// </param>
    private static void DrawGrid(ICanvas canvas, Camera camera)
    {
        const int GridSpacing = 1;

        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 1;

        var worldMinX = camera.ScreenToWorldX(camera.ViewPort.Left);
        var worldMaxX = camera.ScreenToWorldX(camera.ViewPort.Left + camera.ViewPort.Width);
        var worldMinY = camera.ScreenToWorldY(camera.ViewPort.Top);
        var worldMaxY = camera.ScreenToWorldY(camera.ViewPort.Top + camera.ViewPort.Height);

        var startX = (int)MathF.Floor(worldMinX / GridSpacing) * GridSpacing;
        var endX = (int)MathF.Ceiling(worldMaxX / GridSpacing) * GridSpacing;

        var startY = (int)MathF.Floor(worldMinY / GridSpacing) * GridSpacing;
        var endY = (int)MathF.Ceiling(worldMaxY / GridSpacing) * GridSpacing;

        for (var x = startX; x <= endX; x += GridSpacing)
        {
            var sx = camera.WorldToScreenX(x);
            canvas.DrawLine(sx, camera.ViewPort.Top, sx, camera.ViewPort.Top + camera.ViewPort.Height);
        }

        for (var y = startY; y <= endY; y += GridSpacing)
        {
            var sy = camera.WorldToScreenY(y);
            canvas.DrawLine(camera.ViewPort.Left, sy, camera.ViewPort.Left + camera.ViewPort.Width, sy);
        }
    }

    /// <summary>
    /// Draws the points of interest (POIs) onto the specified canvas using the provided camera for coordinate transformations.
    /// </summary>
    /// <param name="canvas">The canvas on which the points will be drawn.</param>
    /// <param name="camera">The camera that provides the transformation from world coordinates to screen coordinates.</param>
    private void DrawPoints(ICanvas canvas, Camera camera)
    {
        foreach (var poi in this.Points)
        {
            var screenX = camera.WorldToScreenX(poi.X);
            var screenY = camera.WorldToScreenY(poi.Y);

            if (poi.Icon != null)
            {
                canvas.DrawImage(poi.Icon, screenX - 16, screenY - 16, 32, 32);
            }
            else
            {
                canvas.FillColor = poi.FillColor;
                canvas.FillCircle(screenX, screenY, 10);
            }

            canvas.FontColor = Colors.White;
            canvas.FontSize = 10;
            canvas.DrawString($"{poi.X},{poi.Y}", screenX + 12, screenY - 8, HorizontalAlignment.Left);
        }
    }

    /// <summary>
    /// Represents a camera used to convert world coordinates into screen
    /// coordinates based on the drawing region, center offset, and zoom level.
    /// </summary>
    /// <param name="viewPort">The drawing area for the current frame.</param>
    /// <param name="centerX">The X offset of the map center in world units.</param>
    /// <param name="centerY">The Y offset of the map center in world units.</param>
    /// <param name="zoom">The zoom level applied to world coordinates.</param>
    private sealed class Camera(RectF viewPort, float centerX, float centerY, float zoom)
    {
        /// <summary>
        /// The X offset of the world center in screen coordinates.
        /// </summary>
        private readonly float worldCenterX = (viewPort.Width / 2f) + centerX;

        /// <summary>
        /// The Y offset of the world center in screen coordinates.
        /// </summary>
        private readonly float worldCenterY = (viewPort.Height / 2f) + centerY;

        /// <summary>
        /// The zoom level applied to world coordinates.
        /// </summary>
        private readonly float zoom = zoom;

        /// <summary>
        /// Gets the drawing area for the current frame.
        /// </summary>
        public RectF ViewPort { get; } = viewPort;

        /// <summary>
        /// Converts a screen coordinate on the X-axis to a world coordinate on the X-axis
        /// based on the current camera settings, including zoom level and center offset.
        /// </summary>
        /// <param name="screenX">The X-coordinate in screen units to be converted.</param>
        /// <returns>The corresponding X-coordinate in world units.</returns>
        public float ScreenToWorldX(float screenX)
        {
            return (screenX - this.worldCenterX) / this.zoom;
        }

        /// <summary>
        /// Converts a screen coordinate on the Y-axis to a world coordinate on the Y-axis
        /// based on the current camera settings, including zoom level and center offset.
        /// </summary>
        /// <param name="screenY">The Y-coordinate in screen units to be converted.</param>
        /// <returns>The corresponding Y-coordinate in world units.</returns>
        public float ScreenToWorldY(float screenY)
        {
            return (screenY - this.worldCenterY) / this.zoom;
        }

        /// <summary>
        /// Converts a world coordinate on the X-axis to a screen coordinate on the X-axis
        /// based on the current camera settings, including zoom level and center offset.
        /// </summary>
        /// <param name="x">The X-coordinate in world units to be converted.</param>
        /// <returns>The corresponding X-coordinate in screen units.</returns>
        public float WorldToScreenX(float x)
        {
            return this.worldCenterX + (x * this.zoom);
        }

        /// <summary>
        /// Converts a world coordinate on the Y-axis to a screen coordinate on the Y-axis
        /// based on the current camera settings, including zoom level and center offset.
        /// </summary>
        /// <param name="y">The Y-coordinate in world units to be converted.</param>
        /// <returns>The corresponding Y-coordinate in screen units.</returns>
        public float WorldToScreenY(float y)
        {
            return this.worldCenterY + (y * this.zoom);
        }
    }
}