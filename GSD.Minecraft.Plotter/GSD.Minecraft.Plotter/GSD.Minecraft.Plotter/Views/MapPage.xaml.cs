// <copyright file="MapPage.xaml.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Views;

using GSD.Minecraft.Plotter.Abstractions;
using GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Represents the main page for displaying and interacting with the map view in the application.
/// </summary>
public partial class MapPage
{
    /// <summary>
    /// Indicates whether the map page has been loaded and initialized.
    /// </summary>
    private bool loaded;

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
    /// Initializes a new instance of the <see cref="MapPage" /> class.
    /// </summary>
    /// <param name="viewModel">The view model that provides data and commands for the map page.</param>
    /// <param name="platformZoomHandler">The platform-specific zoom handler used to manage zoom interactions on the map.</param>
    public MapPage(MapPageViewModel viewModel, IPlatformZoomHandler platformZoomHandler)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        this.InitializeComponent();
        this.BindingContext = viewModel;

        viewModel.Camera.Invalidated += this.OnCameraInvalidated;
        platformZoomHandler?.AttachTo(this.GraphicsView, (scale, screenX, screenY) => viewModel.Camera.ZoomAt(screenX, screenY, scale));
    }

    /// <summary>
    /// Handles the <see cref="Camera.Invalidated" /> event to trigger a redraw of the map view.
    /// </summary>
    /// <param name="sender">The source of the event, typically the <see cref="MapPageViewModel" />.</param>
    /// <param name="e">The event data associated with the invalidate request.</param>
    private void OnCameraInvalidated(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() => { this.GraphicsView.Invalidate(); });
    }

    /// <summary>
    /// Handles the size changed event to update the viewport dimensions in the associated <see cref="MapPageViewModel" />.
    /// </summary>
    /// <param name="sender">The source of the event, typically the <see cref="GraphicsView" />.</param>
    /// <param name="e">The event data associated with the size change.</param>
    private void OnGraphicsViewSizeChanged(object sender, EventArgs e)
    {
        if (this.BindingContext is MapPageViewModel viewModel)
        {
            viewModel.Camera.SetViewport((float)this.GraphicsView.Width, (float)this.GraphicsView.Height);

            if (!this.loaded)
            {
                viewModel.OnLoad();
                this.loaded = true;
            }
        }
    }

    /// <summary>
    /// Occurs when a pan gesture is  recognized.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="PanUpdatedEventArgs" /> that contains the event data.</param>
    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (this.BindingContext is MapPageViewModel viewModel)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    this.panStarted = true;
                    this.startOffsetX = viewModel.Camera.OffsetX;
                    this.startOffsetY = viewModel.Camera.OffsetY;
                    break;

                case GestureStatus.Running when this.panStarted:
                    viewModel.Camera.SetOffset(this.startOffsetX + (float)e.TotalX, this.startOffsetY + (float)e.TotalY);
                    break;

                case GestureStatus.Completed:
                    this.panStarted = false;
                    break;
            }
        }
    }

    /// <summary>
    /// Occurs when a pinch gesture is recognized.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="PinchGestureUpdatedEventArgs" /> that contains the event data.</param>
    private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (this.panStarted ||
            (e.Status != GestureStatus.Running) ||
            this.BindingContext is not MapPageViewModel viewModel)
        {
            return;
        }

        viewModel.Camera.ZoomAt(
            (float)(e.ScaleOrigin.X * this.GraphicsView.Width),
            (float)(e.ScaleOrigin.Y * this.GraphicsView.Height),
            (float)e.Scale);
    }

    /// <summary>
    /// Handles the tap gesture on the map view, converting the tapped screen position
    /// into world coordinates and invoking the <see cref="MapPageViewModel.Pick" /> method
    /// to process the interaction.
    /// </summary>
    /// <param name="sender">The source of the event, typically the <see cref="GraphicsView" />.</param>
    /// <param name="e">The event data containing details about the tap gesture, including the position.</param>
    private void OnTapped(object sender, TappedEventArgs e)
    {
        var point = e.GetPosition(this.GraphicsView);

        if (!point.HasValue || this.BindingContext is not MapPageViewModel viewModel)
        {
            return;
        }

        var worldX = viewModel.Camera.ScreenToWorldX((float)point.Value.X);
        var worldY = viewModel.Camera.ScreenToWorldY((float)point.Value.Y);

        viewModel.Pick(worldX, worldY);
    }
}