// <copyright file="MapDrawable.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using System.Collections.ObjectModel;

/// <summary>
/// Represents a drawable map with zoom and center point properties,  supporting the addition of markers and rendering capabilities.
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
    /// Gets or sets the map layout.
    /// </summary>
    public IMapLayout Layout { get; set; }

    /// <summary>
    /// Gets the collection of markers.
    /// </summary>
    public Collection<MarkerViewModel> Markers { get; } = [];

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
        const float GridSpacing = 1;
        const float CellOffset = GridSpacing / 2f;

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
            var sx = camera.WorldToScreenX(x - CellOffset);
            var isChunkLine = MathF.Floor(x) % 16 == 0;

            if (isChunkLine || (camera.Zoom > 10.0))
            {
                canvas.StrokeSize = 1;
                canvas.StrokeColor = isChunkLine ? Colors.LightGray : Colors.Gray;
                canvas.DrawLine(sx, camera.ViewPort.Top, sx, camera.ViewPort.Top + camera.ViewPort.Height);
            }

            if (isChunkLine)
            {
                canvas.FontSize = 14;
                canvas.FontColor = Colors.White;
                canvas.DrawString($"{MathF.Floor(x)}", sx, camera.ViewPort.Top + 20, HorizontalAlignment.Center);
            }
        }

        for (var y = startY; y <= endY; y += GridSpacing)
        {
            var sy = camera.WorldToScreenY(y - CellOffset);
            var isChunkLine = MathF.Floor(y) % 16 == 0;

            if (isChunkLine || (camera.Zoom > 10.0))
            {
                canvas.StrokeSize = 1;
                canvas.StrokeColor = isChunkLine ? Colors.LightGray : Colors.Gray;
                canvas.DrawLine(camera.ViewPort.Left, sy, camera.ViewPort.Left + camera.ViewPort.Width, sy);
            }

            if (isChunkLine)
            {
                canvas.FontSize = 14;
                canvas.FontColor = Colors.White;
                canvas.DrawString($"{MathF.Floor(y)}", camera.ViewPort.Left + 20, sy, HorizontalAlignment.Center);
            }
        }
    }

    /// <summary>
    /// Draws the points of interest (POIs) onto the specified canvas using the provided camera for coordinate transformations.
    /// </summary>
    /// <param name="canvas">The canvas on which the points will be drawn.</param>
    /// <param name="camera">The camera that provides the transformation from world coordinates to screen coordinates.</param>
    private void DrawPoints(ICanvas canvas, Camera camera)
    {
        foreach (var marker in this.Markers)
        {
            var point = this.Layout?.GetMapCoordinate(marker) ?? (marker.X, marker.Y);
            var floorX = (float)Math.Floor(point.X);
            var floorY = (float)Math.Floor(point.Y);

            var screenX = camera.WorldToScreenX(floorX);
            var screenY = camera.WorldToScreenY(floorY);

            canvas.FillColor = Colors.White;
            canvas.FillCircle(screenX, screenY, 10);

            canvas.FontColor = Colors.White;
            canvas.FontSize = 14;
            canvas.DrawString($"{floorX},{floorY}", screenX + 12, screenY - 8, HorizontalAlignment.Left);
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
        private readonly float centerX = (viewPort.Width / 2f) + centerX;

        /// <summary>
        /// The Y offset of the world center in screen coordinates.
        /// </summary>
        private readonly float centerY = (viewPort.Height / 2f) + centerY;

        /// <summary>
        /// Gets the drawing area for the current frame.
        /// </summary>
        public RectF ViewPort { get; } = viewPort;

        /// <summary>
        /// Gets the zoom level applied to world coordinates.
        /// </summary>
        public float Zoom { get; } = zoom;

        /// <summary>
        /// Converts a screen coordinate on the X-axis to a world coordinate on the X-axis
        /// based on the current camera settings, including zoom level and center offset.
        /// </summary>
        /// <param name="screenX">The X-coordinate in screen units to be converted.</param>
        /// <returns>The corresponding X-coordinate in world units.</returns>
        public float ScreenToWorldX(float screenX)
        {
            return (screenX - this.centerX) / this.Zoom;
        }

        /// <summary>
        /// Converts a screen coordinate on the Y-axis to a world coordinate on the Y-axis
        /// based on the current camera settings, including zoom level and center offset.
        /// </summary>
        /// <param name="screenY">The Y-coordinate in screen units to be converted.</param>
        /// <returns>The corresponding Y-coordinate in world units.</returns>
        public float ScreenToWorldY(float screenY)
        {
            return (screenY - this.centerY) / this.Zoom;
        }

        /// <summary>
        /// Converts a world coordinate on the X-axis to a screen coordinate on the X-axis
        /// based on the current camera settings, including zoom level and center offset.
        /// </summary>
        /// <param name="worldX">The X-coordinate in world units to be converted.</param>
        /// <returns>The corresponding X-coordinate in screen units.</returns>
        public float WorldToScreenX(float worldX)
        {
            return this.centerX + (worldX * this.Zoom);
        }

        /// <summary>
        /// Converts a world coordinate on the Y-axis to a screen coordinate on the Y-axis
        /// based on the current camera settings, including zoom level and center offset.
        /// </summary>
        /// <param name="worldY">The Y-coordinate in world units to be converted.</param>
        /// <returns>The corresponding Y-coordinate in screen units.</returns>
        public float WorldToScreenY(float worldY)
        {
            return this.centerY + (worldY * this.Zoom);
        }
    }
}