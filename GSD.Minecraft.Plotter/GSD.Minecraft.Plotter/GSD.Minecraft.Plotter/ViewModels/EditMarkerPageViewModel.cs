// <copyright file="EditMarkerPageViewModel.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;

/// <summary>
/// Represents the view model for the page in the application.
/// </summary>
public class EditMarkerPageViewModel : ViewModelBase
{
    /// <summary>
    /// The current application state, providing access to shared data and functionality.
    /// </summary>
    private readonly AppState appState;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditMarkerPageViewModel" /> class.
    /// </summary>
    /// <param name="appState">The current application state, providing access to shared data and functionality.</param>
    /// <param name="viewModel">The marker view model containing the data to be edited.</param>
    public EditMarkerPageViewModel(AppState appState, MarkerViewModel viewModel)
    {
        this.appState = appState ?? throw new ArgumentNullException(nameof(appState));
        this.Marker = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

        this.SaveCommand = new Command(this.Save);
        this.CancelCommand = new Command(Cancel);
        this.DeleteCommand = new Command(this.Delete);

        this.Colors.Add(Microsoft.Maui.Graphics.Colors.White);
        this.Colors.Add(Microsoft.Maui.Graphics.Colors.Red);
        this.Colors.Add(Microsoft.Maui.Graphics.Colors.Orange);
        this.Colors.Add(Microsoft.Maui.Graphics.Colors.Yellow);
        this.Colors.Add(Microsoft.Maui.Graphics.Colors.Green);
        this.Colors.Add(Microsoft.Maui.Graphics.Colors.Cyan);
        this.Colors.Add(Microsoft.Maui.Graphics.Colors.Red);
    }

    /// <summary>
    /// Gets the command that cancels the current operation and navigates back to the previous page.
    /// </summary>
    public ICommand CancelCommand { get; }

    /// <summary>
    /// Gets the collection of available colors that can be selected for the marker.
    /// </summary>
    public ObservableCollection<Color> Colors { get; } = [];

    /// <summary>
    /// Gets the command that deletes the current marker.
    /// </summary>
    public ICommand DeleteCommand { get; }

    /// <summary>
    /// Gets the marker view model associated with the page.
    /// </summary>
    public MarkerViewModel Marker { get; }

    /// <summary>
    /// Gets the command that saves the changes made to the marker.
    /// </summary>
    public ICommand SaveCommand { get; }

    /// <summary>
    /// Cancels the current operation and reverts any unsaved changes made in the page.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private static async void Cancel()
    {
        await Shell.Current.CurrentPage.Navigation.PopModalAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the current marker from the application's state and navigates back to the previous page.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private async void Delete()
    {
        var confirmed = await Shell.Current.DisplayAlertAsync("Delete Marker", $"Delete {this.Marker.Name}?", "Delete", "Cancel").ConfigureAwait(true);

        if (!confirmed)
        {
            return;
        }

        this.appState.Markers.Remove(this.Marker);
        await Shell.Current.CurrentPage.Navigation.PopModalAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Saves the current marker data and applies any changes made in the page.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private async void Save()
    {
        if (!this.appState.Markers.Contains(this.Marker))
        {
            this.appState.Markers.Add(this.Marker);
        }

        await Shell.Current.CurrentPage.Navigation.PopModalAsync().ConfigureAwait(false);
    }
}