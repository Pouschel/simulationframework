﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides graphical functionality to the application.
/// </summary>
public static class Graphics
{
    private static IGraphicsProvider Provider => Simulation.Current.GetComponent<IGraphicsProvider>();

    /// <summary>
    /// Gets canvas which draws to the current frame.
    /// </summary>
    /// <returns></returns>
    public static ICanvas GetFrameCanvas()
    {
        return Provider.GetFrameCanvas();
    }

    /// <summary>
    /// Creates a surface and loads its data from a file.
    /// </summary>
    /// <param name="file">The .PNG image file to load the image from.</param>
    /// <returns>The new surface.</returns>
    public static ISurface CreateSurface(string file)
    {
        var fileData = File.ReadAllBytes(file);

        return Provider.CreateSurface(fileData.AsSpan());
    }

    /// <summary>
    /// Creates a blank surface of the provided size.
    /// </summary>
    /// <param name="width">The width of the surface, in pixels.</param>
    /// <param name="height">The height of the surface, in pixels.</param>
    /// <returns>THe new surface.</returns>
    public static ISurface CreateSurface(int width, int height)
    {
        return Provider.CreateSurface(width, height, null);
    }

    /// <summary>
    /// Creates a surface of the provided size and fills it with the provided colors.
    /// </summary>
    /// <param name="width">The width of the surface, in pixels.</param>
    /// <param name="height">The height of the surface, in pixels.</param>
    /// <param name="colors">The data of to fill the surface with. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <returns>The new surface.</returns>
    public static ISurface CreateSurface(int width, int height, Span<Color> colors)
    {
        return Provider.CreateSurface(width, height, colors);
    }

    /// <summary>
    /// Creates a surface of the provided size and fills it with the provided colors.
    /// </summary>
    /// <param name="width">The width of the surface, in pixels.</param>
    /// <param name="height">The height of the surface, in pixels.</param>
    /// <param name="colors">The data of to fill the surface with. Must be of length <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <returns>The new surface.</returns>
    public static ISurface CreateSurface(int width, int height, Color[] colors)
    {
        return Provider.CreateSurface(width, height, colors.AsSpan());
    }
}