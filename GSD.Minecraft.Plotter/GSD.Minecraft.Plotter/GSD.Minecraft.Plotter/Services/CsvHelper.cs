// <copyright file="CsvHelper.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Services;

/// <summary>
/// Provides helper methods for working with CSV formats.
/// </summary>
public static class CsvHelper
{
    /// <summary>
    /// Converts a comma-delimited string back into a string array, handling quotes and escapes.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string array that was converted from the comma-delimited string.</returns>
    public static string[] FromCsvString(string value)
    {
        if (value == null)
        {
            return [];
        }

        var values = new List<string>();
        var insideQuote = false;
        var currentValue = string.Empty;

        for (var i = 0; i < value.Length; i++)
        {
            var currentChar = value[i];

            switch (currentChar)
            {
                case '"' when insideQuote && (i + 1 < value.Length) && (value[i + 1] == '"'):
                    currentValue += "\"";
                    i++;
                    break;

                case '"':
                    insideQuote = !insideQuote;
                    break;

                case ',' when !insideQuote:
                    values.Add(currentValue);
                    currentValue = string.Empty;
                    break;

                default:
                    currentValue += currentChar;
                    break;
            }
        }

        values.Add(currentValue);

        return values.ToArray();
    }

    /// <summary>
    /// Converts a string array into a properly quoted and comma-delimited string.
    /// </summary>
    /// <param name="values">The string array to convert.</param>
    /// <returns>The properly quoted and comma-delimited string.</returns>
    public static string ToCsvString(params string[] values)
    {
        if (values == null)
        {
            return string.Empty;
        }

        var csv = new List<string>();

        foreach (var value in values)
        {
            if ((value != null) && (value.Contains(',', StringComparison.Ordinal) || value.Contains('\"', StringComparison.Ordinal)))
            {
                csv.Add($"\"{value.Replace("\"", "\"\"", StringComparison.Ordinal)}\"");
            }
            else
            {
                csv.Add(value);
            }
        }

        return string.Join(",", csv);
    }
}