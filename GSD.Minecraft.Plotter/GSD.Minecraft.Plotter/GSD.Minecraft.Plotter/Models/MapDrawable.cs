// <copyright file="MapDrawable.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Models;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class MapDrawable : IDrawable, INotifyPropertyChanged
{
    private float offsetX;
    private float offsetY;
    private float zoom = 1f;

    public event PropertyChangedEventHandler? PropertyChanged;

    public float OffsetX
    {
        get => this.offsetX;
        set
        {
            if (this.offsetX != value)
            {
                this.offsetX = value;
                this.OnPropertyChanged();
            }
        }
    }

    public float OffsetY
    {
        get => this.offsetY;
        set
        {
            if (this.offsetY != value)
            {
                this.offsetY = value;
                this.OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<POI> POIs { get; } = [];

    public float Zoom
    {
        get => this.zoom;
        set
        {
            if (this.zoom != value)
            {
                this.zoom = value;
                this.OnPropertyChanged();
            }
        }
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();

        canvas.Translate((dirtyRect.Width / 2) + this.OffsetX, (dirtyRect.Height / 2) + this.OffsetY);
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
        foreach (var poi in this.POIs)
        {
            if (poi.Icon != null)
            {
                canvas.DrawImage(poi.Icon, poi.X - 16, poi.Y - 16, 32, 32);
            }
            else
            {
                canvas.FillColor = Colors.Red;
                canvas.FillCircle(poi.X, poi.Y, 10);
            }

            canvas.FontColor = Colors.White;
            canvas.FontSize = 10;
            canvas.DrawString($"{poi.X},{poi.Y}", poi.X, poi.Y, HorizontalAlignment.Left);
        }


        canvas.RestoreState();
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}