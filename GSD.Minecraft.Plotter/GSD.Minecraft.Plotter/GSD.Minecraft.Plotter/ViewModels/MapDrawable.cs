// <copyright file="MapDrawable.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Represents a drawable map with zoom and center point properties,  supporting the addition of markers and rendering capabilities.
/// </summary>
public class MapDrawable : ViewModelBase, IDrawable
{
    /// <summary>
    /// The <see cref="MapPageViewModel" /> instance that provides data and commands for the map.
    /// </summary>
    private readonly MapPageViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="MapDrawable" /> class with the specified view model.
    /// </summary>
    /// <param name="viewModel">The <see cref="MapPageViewModel" /> instance that provides data and commands for the map.</param>
    public MapDrawable(MapPageViewModel viewModel)
    {
        this.viewModel = viewModel;
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

        var camera = new Camera(dirtyRect, this.viewModel.CenterX, this.viewModel.CenterY, this.viewModel.Zoom);

        this.DrawGrid(canvas, camera);
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
    private void DrawGrid(ICanvas canvas, Camera camera)
    {
        var originGridColor = this.viewModel.Layout.OriginGridColor;
        var primaryGidColor = this.viewModel.Layout.PrimaryGridColor;
        var secondaryGridColor = this.viewModel.Layout.SecondaryGridColor;

        var gridSpacing = 1;

        while (this.viewModel.Zoom * gridSpacing < 16)
        {
            gridSpacing *= 2;
        }

        var labelSpacing = gridSpacing * 2;

        var worldMinX = camera.ScreenToWorldX(camera.ViewPort.Left);
        var worldMaxX = camera.ScreenToWorldX(camera.ViewPort.Left + camera.ViewPort.Width);
        var startX = MathF.Floor(worldMinX);
        var endX = MathF.Ceiling(worldMaxX);

        for (var x = startX; x <= endX; x += 1.0f)
        {
            var sx = camera.WorldToScreenX(x - 0.5f);
            var floor = MathF.Floor(x);
            var isOrigin = floor == 0;
            var isChunkLine = floor % 16 == 0;

            if (floor % gridSpacing == 0)
            {
                canvas.StrokeSize = 1;
                canvas.StrokeColor = isOrigin ? originGridColor : isChunkLine ? primaryGidColor : secondaryGridColor;
                canvas.DrawLine(sx, camera.ViewPort.Top, sx, camera.ViewPort.Top + camera.ViewPort.Height);
            }

            if (floor % labelSpacing == 0)
            {
                canvas.FontSize = 14;
                canvas.FontColor = Colors.White;
                canvas.DrawString($"{floor}", sx, camera.ViewPort.Top + 20, HorizontalAlignment.Center);
            }
        }

        var worldMinY = camera.ScreenToWorldY(camera.ViewPort.Top);
        var worldMaxY = camera.ScreenToWorldY(camera.ViewPort.Top + camera.ViewPort.Height);
        var startY = MathF.Floor(worldMinY);
        var endY = MathF.Ceiling(worldMaxY);

        for (var y = startY; y <= endY; y += 1.0f)
        {
            var sy = camera.WorldToScreenY(y - 0.5f);
            var floor = MathF.Floor(y);
            var isOrigin = floor == 0;
            var isChunkLine = floor % 16 == 0;

            if (floor % gridSpacing == 0)
            {
                canvas.StrokeSize = 1;
                canvas.StrokeColor = isOrigin ? originGridColor : isChunkLine ? primaryGidColor : secondaryGridColor;
                canvas.DrawLine(camera.ViewPort.Left, sy, camera.ViewPort.Left + camera.ViewPort.Width, sy);
            }

            if (floor % labelSpacing == 0)
            {
                canvas.FontSize = 14;
                canvas.FontColor = Colors.White;
                canvas.DrawString($"{floor}", camera.ViewPort.Left + 20, sy, HorizontalAlignment.Center);
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
        foreach (var marker in this.viewModel.Markers)
        {
            var point = this.viewModel.Layout?.GetMapCoordinate(marker) ?? (marker.X, marker.Y);
            var floorX = (float)Math.Floor(point.X);
            var floorY = (float)Math.Floor(point.Y);

            var screenX = camera.WorldToScreenX(floorX);
            var screenY = camera.WorldToScreenY(floorY);

            canvas.FillColor = Colors.White;
            canvas.FillCircle(screenX, screenY, 10);

            canvas.FontColor = Colors.White;
            canvas.FontSize = 14;
            canvas.DrawString($"{marker.Name}", screenX + 15, screenY - 8, HorizontalAlignment.Left);
            canvas.DrawString($"{floorX},{floorY}", screenX + 15, screenY + 8, HorizontalAlignment.Left);
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