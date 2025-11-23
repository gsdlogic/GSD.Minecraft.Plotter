// <copyright file="MarkersPage.xaml.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Views;

using GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Represents the MarkersPage view in the application, providing the user interface for managing markers.
/// </summary>
public partial class MarkersPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MarkersPage" /> class with the specified view model.
    /// </summary>
    /// <param name="viewModel">The <see cref="MarkersPageViewModel" /> instance that provides data and commands for the view.</param>
    public MarkersPage(MarkersPageViewModel viewModel)
    {
        this.InitializeComponent();
        this.BindingContext = viewModel;
    }
}