// <copyright file="ViewModelBase.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using System.ComponentModel;
using System.Runtime.CompilerServices;

/// <summary>
/// Serves as the base class for all view models in the application.
/// </summary>
public abstract class ViewModelBase : INotifyPropertyChanged
{
    /// <summary>
    /// The collection of property values for the view model.
    /// </summary>
    private readonly Dictionary<string, object> properties = new();

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Gets or sets the value for the specified property (does not raise <see cref="PropertyChanged" />).
    /// </summary>
    /// <param name="propertyName">The name of the property to get or set.</param>
    /// <returns>The value for the specified property or <see langword="null" /> if the property does not exist.</returns>
    private object this[string propertyName]
    {
        get => this.properties.GetValueOrDefault(propertyName);
        set => this.properties[propertyName] = value;
    }

    /// <summary>
    /// Gets the value for the specified property.
    /// </summary>
    /// <param name="propertyName">The name of the property to get.</param>
    /// <typeparam name="T">The type of value to return.</typeparam>
    /// <returns>The value for the specified property or the default value for the specified type if the property does not exist.</returns>
    protected T GetValue<T>([CallerMemberName] string propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(propertyName);
        return (T)(this[propertyName] ?? default(T));
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged" /> event when a property value changes.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Sets the value for the specified property and raises <see cref="PropertyChanged" />.
    /// </summary>
    /// <param name="value">The value for the specified property.</param>
    /// <param name="propertyName">The name of the property to set.</param>
    /// <typeparam name="T">The type of value to set.</typeparam>
    protected void SetValue<T>(T value, [CallerMemberName] string propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(propertyName);

        this[propertyName] = value;
        this.OnPropertyChanged(propertyName);
    }
}