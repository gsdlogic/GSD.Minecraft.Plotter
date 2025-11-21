// <copyright file="MainPage.xaml.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Views;

using GSD.Minecraft.Plotter.Abstractions;
using GSD.Minecraft.Plotter.ViewModels;

/// <summary>
/// Contains interaction logic for <c>MainPage.xaml</c>.
/// </summary>
public partial class MainPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage" /> class.
    /// </summary>
    /// <param name="platformZoomHandler">The platform zoom handler.</param>
    public MainPage(IPlatformZoomHandler platformZoomHandler)
    {
        this.InitializeComponent();

        this.MapView.Attach(platformZoomHandler);

        this.MapView.MapDrawable.Points.Add(new Poi { X = 0, Y = 0, FillColor = Colors.White });
        this.MapView.MapDrawable.Points.Add(new Poi { X = -8, Y = -8, FillColor = Colors.Red });
        this.MapView.MapDrawable.Points.Add(new Poi { X = 8, Y = -8, FillColor = Colors.Yellow});
        this.MapView.MapDrawable.Points.Add(new Poi { X = -8, Y = 8, FillColor = Colors.Cyan });
        this.MapView.MapDrawable.Points.Add(new Poi { X = 8, Y = 8, FillColor = Colors.LightGreen });
    }
}