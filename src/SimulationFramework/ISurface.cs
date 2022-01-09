﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// A 2D bitmap that can be rendered, or rendered to.
/// </summary>
public interface ISurface : IDisposable
{
    /// <summary>
    /// The width of the surface, in pixels.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// The height of the surface, in pixels.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// A span of colors making up surface's data
    /// </summary>
    Span<Color> Pixels { get; }

    /// <summary>
    /// Gets a reference to the element of <see cref="Pixels"/> at the provided <paramref name="x"/> and <paramref name="y"/> coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the pixel.</param>
    /// <param name="y">The y-coordinate of the pixel.</param>
    ref Color GetPixel(int x, int y);

    /// <summary>
    /// Opens a new canvas which draws to this surface.
    /// </summary>
    /// <returns>An <see cref="ICanvas"/> which draws onto this surface.</returns>
    ICanvas OpenCanvas();
}