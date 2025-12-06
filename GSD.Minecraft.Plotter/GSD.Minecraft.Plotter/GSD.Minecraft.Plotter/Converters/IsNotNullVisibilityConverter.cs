// <copyright file="IsNotNullVisibilityConverter.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Converters;

using System.Globalization;

/// <summary>
/// Converts a value to a <see cref="Visibility" /> enumeration based on whether the value is null or not.
/// </summary>
public class IsNotNullVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Converts a value to a <see cref="Visibility" /> enumeration based on whether the value is null or not.
    /// </summary>
    /// <param name="value">The value to evaluate for nullity.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">An optional parameter to influence the conversion logic.</param>
    /// <param name="culture">The culture to be used during the conversion.</param>
    /// <returns>
    /// Returns <see cref="Visibility.Visible" /> if the value is not null; otherwise, returns <see cref="Visibility.Collapsed" />.
    /// </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is null ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// Converts a <see cref="Visibility" /> value back to its original value.
    /// </summary>
    /// <param name="value">The <see cref="Visibility" /> value to be converted back.</param>
    /// <param name="targetType">The type to which the value is being converted.</param>
    /// <param name="parameter">An optional parameter to be used during the conversion.</param>
    /// <param name="culture">The culture to be used during the conversion.</param>
    /// <returns>Throws a <see cref="NotSupportedException" /> as this method is not supported.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}