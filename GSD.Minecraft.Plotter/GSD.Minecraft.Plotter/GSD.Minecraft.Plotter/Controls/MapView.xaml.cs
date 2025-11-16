// <copyright file="MapView.xaml.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Controls;

using GSD.Minecraft.Plotter.Models;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MapView : ContentView
{
    private float startOffsetX;
    private float startOffsetY;

    public MapView()
    {
        this.InitializeComponent();
        this.BindingContext = this;
    }

    public MapDrawable MapDrawable { get; set; } = new();

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                this.startOffsetX = this.MapDrawable.OffsetX;
                this.startOffsetY = this.MapDrawable.OffsetY;
                break;

            case GestureStatus.Running:
                this.MapDrawable.OffsetX = (float)(this.startOffsetX + e.TotalX);
                this.MapDrawable.OffsetY = (float)(this.startOffsetY + e.TotalY);
                this.GraphicsView.Invalidate();
                break;
        }
    }

    private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status != GestureStatus.Running)
        {
            return;
        }

        var dx = (float)((e.ScaleOrigin.X - 0.5) * this.GraphicsView.Width);
        var dy = (float)((e.ScaleOrigin.Y - 0.5) * this.GraphicsView.Height);

        this.MapDrawable.OffsetX += dx * (1 - (float)e.Scale);
        this.MapDrawable.OffsetY += dy * (1 - (float)e.Scale);
        this.MapDrawable.Zoom *= (float)e.Scale;

        this.GraphicsView.Invalidate();
    }
}