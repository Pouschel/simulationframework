﻿using System;
using System.Collections.Generic;
using System.Numerics;

namespace SimulationFramework;

/// <summary>
/// Contains various useful math-related methods.
/// </summary>
public static class MathHelper
{
    /// <summary>
    /// Finds the bounding box of a polygon.
    /// </summary>
    /// <param name="polygon">The polygon to find the bounds of.</param>
    public static unsafe Rectangle PolygonBounds(Span<Vector2> polygon)
    {
        fixed (Vector2* polygonPtr = &polygon[0])
        {
            return PolygonBounds(CollectionsHelper.AsEnumerableUnsafe(polygonPtr, polygon.Length));
        }
    }

    /// <summary>
    /// Finds the bounding box of a polygon.
    /// </summary>
    /// <param name="polygon">The polygon to find the bounds of.</param>
    public static Rectangle PolygonBounds(IEnumerable<Vector2> polygon)
    {
        float minX = float.PositiveInfinity, minY = float.PositiveInfinity, maxX = float.NegativeInfinity, maxY = float.NegativeInfinity;

        foreach (var point in polygon)
        {
            maxX = MathF.Max(maxX, point.X);
            maxY = MathF.Max(maxY, point.Y);
            minX = MathF.Min(minX, point.X);
            minY = MathF.Min(minY, point.Y);
        }

        return new Rectangle(minX, minY, maxX - minX, maxY - minY);
    }

    /// <summary>
    /// Linearly interpolates between two values.
    /// </summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <param name="t">The interpolation amount, from 0.0 to 1.0.</param>
    /// <returns>The interpolated value.</returns>
    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    /// <summary>
    /// Linearly interpolates between two values, clamping the result between the two values..
    /// </summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <param name="t">The interpolation amount, from 0.0 to 1.0.</param>
    /// <returns>The interpolated value.</returns>
    public static float LerpClamped(float a, float b, float t)
    {
        return Lerp(a, b, Normalize(t));
    }

    /// <summary>
    /// Clamps a float to range [0.0, 1.0]
    /// </summary>
    /// <param name="value">The value to normalize.</param>
    /// <returns>The normalized value.</returns>
    public static float Normalize(float value)
    {
        return Math.Clamp(value, 0.0f, 1.0f);
    }
}