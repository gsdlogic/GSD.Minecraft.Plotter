// <copyright file="ModelExtensions.cs" company="GSD Logic">
// Copyright © 2025 GSD Logic. All Rights Reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.ViewModels;

using GSD.Minecraft.Plotter.Models;

/// <summary>
/// Provides extension methods for converting models to their corresponding view models.
/// </summary>
public static class ModelExtensions
{
    /// <summary>
    /// Converts a <see cref="WorldViewModel" /> model to a <see cref="World" />.
    /// </summary>
    /// <param name="viewModel">The <see cref="WorldViewModel" /> instance to convert.</param>
    /// <returns>A <see cref="World" /> representing the provided <see cref="WorldViewModel" /> model, or <see langword="null" /> if the input is <see langword="null" />.</returns>
    public static World ToModel(this WorldViewModel viewModel)
    {
        if (viewModel == null)
        {
            return null;
        }

        return new World
        {
            Id = viewModel.Id,
            Name = viewModel.Name,
            Seed = viewModel.Seed,
        };
    }

    /// <summary>
    /// Converts a <see cref="MarkerViewModel" /> model to a <see cref="Marker" />.
    /// </summary>
    /// <param name="model">The <see cref="MarkerViewModel" /> instance to convert.</param>
    /// <returns>A <see cref="MarkerViewModel" /> representing the provided <see cref="Marker" /> model, or <see langword="null" /> if the input is <see langword="null" />.</returns>
    public static Marker ToModel(this MarkerViewModel model)
    {
        if (model == null)
        {
            return null;
        }

        return new Marker
        {
            Id = model.Id,
            Name = model.Name,
            X = model.X,
            Y = model.Y,
            Z = model.Z,
        };
    }

    /// <summary>
    /// Converts a <see cref="World" /> model to a <see cref="WorldViewModel" />.
    /// </summary>
    /// <param name="model">The <see cref="World" /> instance to convert.</param>
    /// <returns>A <see cref="WorldViewModel" /> representing the provided <see cref="World" /> model, or <see langword="null" /> if the input is <see langword="null" />.</returns>
    public static WorldViewModel ToViewModel(this World model)
    {
        if (model == null)
        {
            return null;
        }

        return new WorldViewModel
        {
            Id = model.Id,
            Name = model.Name,
            Seed = model.Seed,
        };
    }

    /// <summary>
    /// Converts a <see cref="Marker" /> model to a <see cref="MarkerViewModel" />.
    /// </summary>
    /// <param name="model">The <see cref="Marker" /> instance to convert.</param>
    /// <returns>A <see cref="MarkerViewModel" /> representing the provided <see cref="Marker" /> model, or <see langword="null" /> if the input is <see langword="null" />.</returns>
    public static MarkerViewModel ToViewModel(this Marker model)
    {
        if (model == null)
        {
            return null;
        }

        return new MarkerViewModel
        {
            Id = model.Id,
            Name = model.Name,
            X = model.X,
            Y = model.Y,
            Z = model.Z,
        };
    }
}