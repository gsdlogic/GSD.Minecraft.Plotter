// <copyright file="EditMarkerPage.xaml.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Views;

using GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Represents the page used for editing markers in the application.
/// </summary>
public partial class EditMarkerPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EditMarkerPage" /> class with the specified view model.
    /// </summary>
    /// <param name="viewModel">The <see cref="EditMarkerPageViewModel" /> instance that provides data binding for the page.</param>
    public EditMarkerPage(EditMarkerPageViewModel viewModel)
    {
        this.InitializeComponent();
        this.BindingContext = viewModel;
    }
}