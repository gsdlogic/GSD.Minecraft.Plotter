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

        canvas.Translate((dirtyRect.Width / 2) + this.CenterX, (dirtyRect.Height / 2) + this.CenterY);
        canvas.Scale(this.Zoom, this.Zoom);

        // Grid
        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 1;

        for (var i = -1000; i <= 1000; i += 50)
        {
            canvas.DrawLine(-1000, i, 1000, i);
            canvas.DrawLine(i, -1000, i, 1000);
        }

        // POIs
        foreach (var poi in this.Points)
        {
            if (poi.Icon != null)
            {
                canvas.DrawImage(poi.Icon, poi.X - 16, poi.Y - 16, 32, 32);
            }
            else
            {
                canvas.FillColor = poi.FillColor;
                canvas.FillCircle(poi.X, poi.Y, 10);
            }

            canvas.FontColor = Colors.White;
            canvas.FontSize = 10;
            canvas.DrawString($"{poi.X},{poi.Y}", poi.X, poi.Y, HorizontalAlignment.Left);
        }

        canvas.RestoreState();
    }
}