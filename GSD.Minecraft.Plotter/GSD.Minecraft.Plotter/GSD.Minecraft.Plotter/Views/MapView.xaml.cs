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
        this.BindingContext = this;
    }

    /// <summary>
    /// Gets or sets the map drawable.
    /// </summary>
    public MapDrawable MapDrawable { get; set; } = new();

    /// <summary>
    /// Attaches a platform zoom handler.
    /// </summary>
    /// <param name="platformZoomHandler">The platform zoom handler to attach.</param>
    public void Attach(IPlatformZoomHandler platformZoomHandler)
    {
        platformZoomHandler?.AttachTo(
            this.GraphicsView,
            (factor, _, _) =>
            {
                this.MapDrawable.Zoom *= factor;
                this.GraphicsView.Invalidate();
            });
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
                this.startOffsetX = this.MapDrawable.CenterX;
                this.startOffsetY = this.MapDrawable.CenterY;
                break;

            case GestureStatus.Running:
                this.MapDrawable.CenterX = (float)(this.startOffsetX + e.TotalX);
                this.MapDrawable.CenterY = (float)(this.startOffsetY + e.TotalY);
                this.GraphicsView.Invalidate();
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
        if (e.Status != GestureStatus.Running)
        {
            return;
        }

        this.MapDrawable.Zoom *= (float)e.Scale;
        this.GraphicsView.Invalidate();
    }
}