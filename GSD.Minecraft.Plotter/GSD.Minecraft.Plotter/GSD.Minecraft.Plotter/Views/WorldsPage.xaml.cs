// <copyright file="WorldsPage.xaml.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Views;

using GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Contains interaction logic for <c>WorldsPage.xaml</c>.
/// </summary>
public partial class WorldsPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorldsPage" /> class with the specified view model.
    /// </summary>
    /// <param name="viewModel">The <see cref="WorldsViewModel" /> instance that provides data and commands for the page.</param>
    public WorldsPage(WorldsViewModel viewModel)
    {
        this.InitializeComponent();
        this.BindingContext = viewModel;
    }
}