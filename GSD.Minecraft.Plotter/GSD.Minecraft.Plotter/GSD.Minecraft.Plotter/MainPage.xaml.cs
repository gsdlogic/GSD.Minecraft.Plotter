// <copyright file="MainPage.xaml.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter;

using GSD.Minecraft.Plotter.Models;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        this.InitializeComponent();
        this.MapView.MapDrawable.POIs.Add(new POI { X = 10, Y = 10 });
    }
}