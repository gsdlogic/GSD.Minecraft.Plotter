// <copyright file="Setting.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Models;

/// <summary>
/// Represents a configuration setting with a key-value pair.
/// </summary>
public class Setting
{
    /// <summary>
    /// Gets or sets the unique identifier for the setting.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the value associated with the configuration setting.
    /// </summary>
    public string Value { get; set; }
}