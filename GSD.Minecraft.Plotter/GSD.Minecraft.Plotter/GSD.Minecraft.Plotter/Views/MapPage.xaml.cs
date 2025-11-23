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
    /// Initializes a new instance of the <see cref="MapPage" /> class.
    /// </summary>
    /// <param name="viewModel">The view model that provides data and commands for the map page.</param>
    /// <param name="platformZoomHandler">The platform-specific zoom handler used to manage zoom interactions on the map.</param>
    public MapPage(MapPageViewModel viewModel, IPlatformZoomHandler platformZoomHandler)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        this.InitializeComponent();
        this.BindingContext = viewModel;
        this.MapView.Attach(platformZoomHandler);
    }
}