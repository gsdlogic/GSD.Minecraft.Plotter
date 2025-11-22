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

        var startX = (int)MathF.Floor(camera.WorldMinX / GridSpacing) * GridSpacing;
        var endX = (int)MathF.Ceiling(camera.WorldMaxX / GridSpacing) * GridSpacing;

        var startY = (int)MathF.Floor(camera.WorldMinY / GridSpacing) * GridSpacing;
        var endY = (int)MathF.Ceiling(camera.WorldMaxY / GridSpacing) * GridSpacing;

        for (var x = startX; x <= endX; x += GridSpacing)
        {
            var sx = camera.WorldCenterX + (x * camera.Zoom);
            canvas.DrawLine(sx, 0, sx, camera.DirtyRect.Height);
        }

        for (var y = startY; y <= endY; y += GridSpacing)
        {
            var sy = camera.WorldCenterY + (y * camera.Zoom);
            canvas.DrawLine(0, sy, camera.DirtyRect.Width, sy);
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
            var screenX = camera.WorldCenterX + (poi.X * camera.Zoom);
            var screenY = camera.WorldCenterY + (poi.Y * camera.Zoom);

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
    /// <param name="dirtyRect">The drawing area for the current frame.</param>
    /// <param name="centerX">The X offset of the map center in world units.</param>
    /// <param name="centerY">The Y offset of the map center in world units.</param>
    /// <param name="zoom">The zoom level applied to world coordinates.</param>
    private sealed class Camera(RectF dirtyRect, float centerX, float centerY, float zoom)
    {
        /// <summary>
        /// Gets the drawing area for the current frame.
        /// </summary>
        public RectF DirtyRect { get; } = dirtyRect;

        /// <summary>
        /// Gets the X offset of the world center in screen coordinates.
        /// </summary>
        public float WorldCenterX { get; } = (dirtyRect.Width / 2f) + centerX;

        /// <summary>
        /// Gets the Y offset of the world center in screen coordinates.
        /// </summary>
        public float WorldCenterY { get; } = (dirtyRect.Height / 2f) + centerY;

        /// <summary>
        /// Gets the maximum visible world X value.
        /// </summary>
        public float WorldMaxX { get; } = ((dirtyRect.Width / 2f) - centerX) / zoom;

        /// <summary>
        /// Gets the maximum visible world Y value.
        /// </summary>
        public float WorldMaxY { get; } = ((dirtyRect.Height / 2f) - centerY) / zoom;

        /// <summary>
        /// Gets the minimum visible world X value.
        /// </summary>
        public float WorldMinX { get; } = -((dirtyRect.Width / 2f) + centerX) / zoom;

        /// <summary>
        /// Gets the minimum visible world Y value.
        /// </summary>
        public float WorldMinY { get; } = -((dirtyRect.Height / 2f) + centerY) / zoom;

        /// <summary>
        /// Gets the zoom level applied to world coordinates.
        /// </summary>
        public float Zoom { get; } = zoom;
    }
}