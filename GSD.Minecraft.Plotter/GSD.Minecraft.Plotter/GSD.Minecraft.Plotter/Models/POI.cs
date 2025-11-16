// <copyright file="POI.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GSD.Minecraft.Plotter.Models;

using IImage = Microsoft.Maui.Graphics.IImage;

public class POI
{
    public IImage? Icon { get; set; }

    public string Name { get; set; } = "";

    public float X { get; set; }

    public float Y { get; set; }
}