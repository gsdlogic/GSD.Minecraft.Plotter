// <copyright file="MapView.xaml.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Views;

using GSD.Minecraft.Plotter.Abstractions;
using GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Contains interaction logic for <c>MapView.xml</c>>.
/// </summary>
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MapView
{
    /// <summary>
    /// Identifies the <see cref="MapDrawable" /> bindable property, which represents the drawable content of the map.
    /// </summary>
    public static readonly BindableProperty MapDrawableProperty =
        BindableProperty.Create(
            nameof(MapDrawable),
            typeof(MapDrawable),
            typeof(MapView),
            propertyChanged: OnMapDrawableChanged);

    /// <summary>
    /// Indicates whether a pan gesture has started.
    /// </summary>
    private bool panStarted;

    /// <summary>
    /// The X-component of the start offset.
    /// </summary>
    private float startOffsetX;

    /// <summary>
    /// The Y-component of the start offset.
    /// </summary>
    private float startOffsetY;

    /// <summary>
    /// Initializes a new instance of the <see cref="MapView" /> class.
    /// </summary>
    public MapView()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the <see cref="MapDrawable" /> instance that represents the drawable content of the map.
    /// </summary>
    public MapDrawable MapDrawable
    {
        get => (MapDrawable)this.GetValue(MapDrawableProperty);
        set => this.SetValue(MapDrawableProperty, value);
    }

    /// <summary>
    /// Attaches a platform zoom handler.
    /// </summary>
    /// <param name="platformZoomHandler">The platform zoom handler to attach.</param>
    public void Attach(IPlatformZoomHandler platformZoomHandler)
    {
        platformZoomHandler?.AttachTo(
            this.GraphicsView,
            this.ZoomAndScale);
    }

    /// <summary>
    /// Handles changes to the <see cref="MapDrawable" /> property.
    /// </summary>
    /// <param name="bindable">The bindable object on which the property changed.</param>
    /// <param name="oldValue">The previous value of the property.</param>
    /// <param name="newValue">The new value of the property.</param>
    private static void OnMapDrawableChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (MapView)bindable;

        view.GraphicsView.Drawable = (IDrawable)newValue;
        view.GraphicsView.Invalidate();
    }

    /// <summary>
    /// Occurs when a pan gesture is  recognized.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="PanUpdatedEventArgs" /> that contains the event data.</param>
    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                this.panStarted = true;
                this.startOffsetX = this.MapDrawable.CenterX;
                this.startOffsetY = this.MapDrawable.CenterY;
                break;

            case GestureStatus.Running when this.panStarted:
                this.MapDrawable.CenterX = (float)(this.startOffsetX + e.TotalX);
                this.MapDrawable.CenterY = (float)(this.startOffsetY + e.TotalY);
                this.GraphicsView.Invalidate();
                break;

            case GestureStatus.Completed:
                this.panStarted = false;
                break;
        }
    }

    /// <summary>
    /// Occurs when a pinch gesture is recognized.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="PinchGestureUpdatedEventArgs" /> that contains the event data.</param>
    private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if ((e.Status != GestureStatus.Running) || this.panStarted)
        {
            return;
        }

        this.ZoomAndScale(
            (float)e.Scale,
            (float)(e.ScaleOrigin.X * this.GraphicsView.Width),
            (float)(e.ScaleOrigin.Y * this.GraphicsView.Height));
    }

    /// <summary>
    /// Adjusts the zoom level and repositions the map's center based on the specified scale and origin.
    /// </summary>
    /// <param name="scale">The zoom scale factor to apply.</param>
    /// <param name="originX">The X-coordinate of the zoom origin in the view.</param>
    /// <param name="originY">The Y-coordinate of the zoom origin in the view.</param>
    private void ZoomAndScale(float scale, float originX, float originY)
    {
        var worldX = (originX - (this.GraphicsView.Width / 2f) - this.MapDrawable.CenterX) / this.MapDrawable.Zoom;
        var worldY = (originY - (this.GraphicsView.Height / 2f) - this.MapDrawable.CenterY) / this.MapDrawable.Zoom;

        this.MapDrawable.Zoom *= scale;
        this.MapDrawable.CenterX = (float)(originX - (worldX * this.MapDrawable.Zoom) - (this.GraphicsView.Width / 2f));
        this.MapDrawable.CenterY = (float)(originY - (worldY * this.MapDrawable.Zoom) - (this.GraphicsView.Height / 2f));

        this.GraphicsView.Invalidate();
    }
}