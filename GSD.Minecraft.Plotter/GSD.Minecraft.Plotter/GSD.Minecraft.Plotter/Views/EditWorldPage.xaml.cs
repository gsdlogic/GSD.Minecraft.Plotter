// <copyright file="EditWorldPage.xaml.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Views;

using GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Represents the page used for editing markers in the application.
/// </summary>
public partial class EditWorldPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EditWorldPage" /> class with the specified view model.
    /// </summary>
    /// <param name="viewModel">The <see cref="EditWorldPageViewModel" /> instance that provides data binding for the page.</param>
    public EditWorldPage(EditWorldPageViewModel viewModel)
    {
        this.InitializeComponent();
        this.BindingContext = viewModel;
    }
}