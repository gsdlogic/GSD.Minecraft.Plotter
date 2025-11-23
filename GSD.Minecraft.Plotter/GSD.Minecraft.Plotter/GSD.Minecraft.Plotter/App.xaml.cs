// <copyright file="App.xaml.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter;

using GSD.Minecraft.Plotter.Views;

/// <summary>
/// Contains interaction logic for <c>App.xaml</c>.
/// </summary>
public partial class App
{
    /// <summary>
    /// Initializes a new instance of the <see cref="App" /> class.
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// Creates and configures the main application window.
    /// </summary>
    /// <param name="activationState">The activation state that provides context for the window creation.</param>
    /// <returns>A new instance of the <see cref="Window" /> class configured with the <see cref="AppShell" />.</returns>
    protected override Window CreateWindow(IActivationState activationState)
    {
        return new Window(new AppShell());
    }
}