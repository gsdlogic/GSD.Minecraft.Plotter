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

        viewModel.InvalidateRequested += this.OnInvalidateRequested;
        platformZoomHandler?.AttachTo(this.GraphicsView, viewModel.ZoomAndScale);
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
            viewModel.ViewWidth = (float)this.GraphicsView.Width;
            viewModel.ViewHeight = (float)this.GraphicsView.Height;

            if (!this.loaded)
            {
                viewModel.OnLoad();
                this.loaded = true;
            }
        }
    }

    /// <summary>
    /// Handles the <see cref="MapPageViewModel.InvalidateRequested" /> event to trigger a redraw of the map view.
    /// </summary>
    /// <param name="sender">The source of the event, typically the <see cref="MapPageViewModel" />.</param>
    /// <param name="e">The event data associated with the invalidate request.</param>
    private void OnInvalidateRequested(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() => { this.GraphicsView.Invalidate(); });
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
                    this.startOffsetX = viewModel.CenterX;
                    this.startOffsetY = viewModel.CenterY;
                    break;

                case GestureStatus.Running when this.panStarted:
                    viewModel.CenterMap(this.startOffsetX + (float)e.TotalX, this.startOffsetY + (float)e.TotalY);
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

        viewModel.ZoomAndScale(
            (float)e.Scale,
            (float)(e.ScaleOrigin.X * this.GraphicsView.Width),
            (float)(e.ScaleOrigin.Y * this.GraphicsView.Height));
    }
}