// <copyright file="EditWorldPageViewModel.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using System.Windows.Input;

/// <summary>
/// Represents the view model for the page in the application.
/// </summary>
public class EditWorldPageViewModel : ViewModelBase
{
    /// <summary>
    /// The current application state, providing access to shared data and functionality.
    /// </summary>
    private readonly AppState appState;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditWorldPageViewModel" /> class.
    /// </summary>
    /// <param name="appState">The current application state, providing access to shared data and functionality.</param>
    /// <param name="viewModel">The World view model containing the data to be edited.</param>
    public EditWorldPageViewModel(AppState appState, WorldViewModel viewModel)
    {
        this.appState = appState ?? throw new ArgumentNullException(nameof(appState));
        this.World = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

        this.SaveCommand = new Command(this.Save);
        this.CancelCommand = new Command(Cancel);
        this.DeleteCommand = new Command(this.Delete);
    }

    /// <summary>
    /// Gets the command that cancels the current operation and navigates back to the previous page.
    /// </summary>
    public ICommand CancelCommand { get; }

    /// <summary>
    /// Gets the command that deletes the current World.
    /// </summary>
    public ICommand DeleteCommand { get; }

    /// <summary>
    /// Gets the command that saves the changes made to the World.
    /// </summary>
    public ICommand SaveCommand { get; }

    /// <summary>
    /// Gets the World view model associated with the page.
    /// </summary>
    public WorldViewModel World { get; }

    /// <summary>
    /// Cancels the current operation and reverts any unsaved changes made in the page.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private static async void Cancel()
    {
        await Shell.Current.CurrentPage.Navigation.PopModalAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the current World from the application's state and navigates back to the previous page.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private async void Delete()
    {
        if (this.appState.Worlds.Count == 1)
        {
            await Shell.Current.DisplayAlertAsync("Delete World", "Cannot delete the last world!", "OK").ConfigureAwait(true);
            return;
        }

        var confirmed = await Shell.Current.DisplayAlertAsync("Delete World", $"Delete {this.World.Name}?", "Delete", "Cancel").ConfigureAwait(true);

        if (!confirmed)
        {
            return;
        }

        this.appState.Worlds.Remove(this.World);
        await Shell.Current.CurrentPage.Navigation.PopModalAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Saves the current World data and applies any changes made in the page.
    /// </summary>
    /// ReSharper disable once AsyncVoidMethod
    private async void Save()
    {
        if (!this.appState.Worlds.Contains(this.World))
        {
            this.appState.Worlds.Add(this.World);
        }

        await Shell.Current.CurrentPage.Navigation.PopModalAsync().ConfigureAwait(false);
    }
}