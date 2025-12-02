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

        this.DrawGrid(canvas);
        this.DrawPoints(canvas);

        canvas.RestoreState();
    }

    /// <summary>
    /// Draws a grid onto the specified canvas using the provided camera settings.
    /// </summary>
    /// <param name="canvas">The canvas on which the grid will be drawn.</param>
    private void DrawGrid(ICanvas canvas)
    {
        const float ViewTop = 0.0f;
        const float ViewLeft = 0.0f;

        var originGridColor = this.viewModel.Layout.OriginGridColor;
        var primaryGidColor = this.viewModel.Layout.PrimaryGridColor;
        var secondaryGridColor = this.viewModel.Layout.SecondaryGridColor;

        var gridSpacing = 1;

        while (this.viewModel.Camera.Zoom * gridSpacing < 16)
        {
            gridSpacing *= 2;
        }

        var labelSpacing = gridSpacing * 2;

        var worldMinX = this.viewModel.Camera.ScreenToWorldX(0);
        var worldMaxX = this.viewModel.Camera.ScreenToWorldX(this.viewModel.Camera.ViewportWidth);
        var startX = MathF.Floor(worldMinX / gridSpacing) * gridSpacing;
        var endX = MathF.Ceiling(worldMaxX);

        for (var x = startX; x <= endX; x += gridSpacing)
        {
            var sx = this.viewModel.Camera.WorldToScreenX(x - 0.5f);
            var floor = MathF.Floor(x);
            var isOrigin = floor == 0;
            var isChunkLine = floor % 16 == 0;

            canvas.StrokeSize = 1;
            canvas.StrokeColor = isOrigin ? originGridColor : isChunkLine ? primaryGidColor : secondaryGridColor;
            canvas.DrawLine(sx, ViewTop, sx, ViewTop + this.viewModel.Camera.ViewportHeight);

            if (floor % labelSpacing == 0)
            {
                canvas.FontSize = 14;
                canvas.FontColor = Colors.White;
                canvas.DrawString($"{floor}", sx, ViewTop + 20, HorizontalAlignment.Center);
            }
        }

        var worldMinY = this.viewModel.Camera.ScreenToWorldY(ViewTop);
        var worldMaxY = this.viewModel.Camera.ScreenToWorldY(ViewTop + this.viewModel.Camera.ViewportHeight);
        var startY = MathF.Floor(worldMinY / gridSpacing) * gridSpacing;
        var endY = MathF.Ceiling(worldMaxY);

        for (var y = startY; y <= endY; y += gridSpacing)
        {
            var sy = this.viewModel.Camera.WorldToScreenY(y - 0.5f);
            var floor = MathF.Floor(y);
            var isOrigin = floor == 0;
            var isChunkLine = floor % 16 == 0;

            canvas.StrokeSize = 1;
            canvas.StrokeColor = isOrigin ? originGridColor : isChunkLine ? primaryGidColor : secondaryGridColor;
            canvas.DrawLine(ViewLeft, sy, ViewLeft + this.viewModel.Camera.ViewportWidth, sy);

            if (floor % labelSpacing == 0)
            {
                canvas.FontSize = 14;
                canvas.FontColor = Colors.White;
                canvas.DrawString($"{floor}", ViewLeft + 20, sy, HorizontalAlignment.Center);
            }
        }
    }

    /// <summary>
    /// Draws the points of interest (POIs) onto the specified canvas using the provided camera for coordinate transformations.
    /// </summary>
    /// <param name="canvas">The canvas on which the points will be drawn.</param>
    private void DrawPoints(ICanvas canvas)
    {
        var pinned = this.viewModel.PinnedMarker;
        var selected = this.viewModel.SelectedMarker;

        foreach (var marker in this.viewModel.Markers)
        {
            var point = this.viewModel.Layout?.GetMapCoordinate(marker) ?? (marker.X, marker.Y);
            var truncatedX = (float)Math.Truncate(point.X);
            var truncatedY = (float)Math.Truncate(point.Y);

            var screenX = this.viewModel.Camera.WorldToScreenX(truncatedX);
            var screenY = this.viewModel.Camera.WorldToScreenY(truncatedY);

            if ((marker != selected) && (marker != pinned))
            {
                canvas.FillColor = Colors.White;
                canvas.FillCircle(screenX, screenY, 10);
            }

            canvas.FontColor = Colors.White;
            canvas.FontSize = 14;
            canvas.DrawString($"{marker.Name}", screenX + 15, screenY - 8, HorizontalAlignment.Left);
            canvas.DrawString($"{truncatedX},{truncatedY}", screenX + 15, screenY + 8, HorizontalAlignment.Left);
        }

        if (selected != null)
        {
            var point = this.viewModel.Layout?.GetMapCoordinate(selected) ?? (selected.X, selected.Y);

            var truncatedX = (float)Math.Truncate(point.X);
            var truncatedY = (float)Math.Truncate(point.Y);

            var screenX = this.viewModel.Camera.WorldToScreenX(truncatedX);
            var screenY = this.viewModel.Camera.WorldToScreenY(truncatedY);

            canvas.FillColor = Colors.Yellow;
            canvas.StrokeColor = Colors.Yellow;
            canvas.FillCircle(screenX, screenY, 10);
            canvas.DrawCircle(screenX, screenY, 12);
        }

        if (pinned != null)
        {
            var point = this.viewModel.Layout?.GetMapCoordinate(pinned) ?? (pinned.X, pinned.Y);

            var truncatedX = (float)Math.Truncate(point.X);
            var truncatedY = (float)Math.Truncate(point.Y);

            var screenX = this.viewModel.Camera.WorldToScreenX(truncatedX);
            var screenY = this.viewModel.Camera.WorldToScreenY(truncatedY);

            canvas.FillColor = Colors.Green;
            canvas.StrokeColor = Colors.Green;
            canvas.FillCircle(screenX, screenY, 10);
            canvas.DrawCircle(screenX, screenY, 12);
        }
    }
}