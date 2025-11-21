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
        this.Zoom = 1.0f;
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

        // Apply only translation to center
        var centerX = (dirtyRect.Width / 2) + this.CenterX;
        var centerY = (dirtyRect.Height / 2) + this.CenterY;

        ////// Draw grid first (grid lines scale with zoom)
        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 1;

        // World bounds visible on screen
        var worldMinX = -((dirtyRect.Width / 2f) + this.CenterX) / this.Zoom;
        var worldMaxX = ((dirtyRect.Width / 2f) - this.CenterX) / this.Zoom;

        var worldMinY = -((dirtyRect.Height / 2f) + this.CenterY) / this.Zoom;
        var worldMaxY = ((dirtyRect.Height / 2f) - this.CenterY) / this.Zoom;

        // Grid
        const int GridSpacing = 16;

        // Snap to nearest grid lines
        var startX = (int)MathF.Floor(worldMinX / GridSpacing) * GridSpacing;
        var endX = (int)MathF.Ceiling(worldMaxX / GridSpacing) * GridSpacing;

        var startY = (int)MathF.Floor(worldMinY / GridSpacing) * GridSpacing;
        var endY = (int)MathF.Ceiling(worldMaxY / GridSpacing) * GridSpacing;

        // Vertical lines
        for (var x = startX; x <= endX; x += GridSpacing)
        {
            var sx = centerX + (x * this.Zoom);
            canvas.DrawLine(sx, 0, sx, dirtyRect.Height);
        }

        // Horizontal lines
        for (var y = startY; y <= endY; y += GridSpacing)
        {
            var sy = centerY + (y * this.Zoom);
            canvas.DrawLine(0, sy, dirtyRect.Width, sy);
        }

        // Draw POIs (fixed size)
        foreach (var poi in this.Points)
        {
            var screenX = centerX + (poi.X * this.Zoom);
            var screenY = centerY + (poi.Y * this.Zoom);

            if (poi.Icon != null)
            {
                canvas.DrawImage(poi.Icon, screenX - 16, screenY - 16, 32, 32);
            }
            else
            {
                canvas.FillColor = poi.FillColor;
                canvas.FillCircle(screenX, screenY, 10); // fixed radius
            }

            canvas.FontColor = Colors.White;
            canvas.FontSize = 10; // fixed font size
            canvas.DrawString($"{poi.X},{poi.Y}", screenX, screenY, HorizontalAlignment.Left);
        }

        canvas.RestoreState();
    }
}