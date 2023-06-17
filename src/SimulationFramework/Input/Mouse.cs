﻿using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SimulationFramework.Input;

/// <summary>
/// Provides mouse input to the application.
/// </summary>
public static class Mouse
{
    private static IMouseProvider Provider => Application.GetComponent<IMouseProvider>();

    /// <summary>
    /// The position of the mouse.
    /// </summary>
    public static Vector2 Position 
    { 
        get => Provider.Position; 
        set => Provider.Position = value;
    }

    /// <summary>
    /// The distance the mouse has moved since the last frame.
    /// </summary>
    public static Vector2 DeltaPosition => Provider.DeltaPosition;

    /// <summary>
    /// The distance the scroll whell has moved since the last frame.
    /// </summary>
    public static int ScrollWheelDelta => Provider.ScrollWheelDelta;

    public static bool Visible
    {
        get => Provider.Visible;
        set => Provider.Visible = value;
    }

    /// <summary>
    /// Fired when the user presses a mouse button.
    /// </summary>
    public static event MouseButtonEvent ButtonDown
    {
        add => Provider.ButtonPressed += value;
        remove => Provider.ButtonPressed -= value;
    }

    /// <summary>
    /// Fired when the user released a mouse button.
    /// </summary>
    public static event MouseButtonEvent ButtonUp
    {
        add => Provider.ButtonReleased += value;
        remove => Provider.ButtonReleased -= value;
    }

    /// <summary>
    /// Gets a collection of all buttons which are held this frame.
    /// </summary>
    public static IEnumerable<MouseButton> HeldButtons => Provider.HeldButtons;

    /// <summary>
    /// Gets a collection of all buttons pressed this frame. A key is only considered pressed on the first frame that it is held.
    /// </summary>
    public static IEnumerable<MouseButton> HeldPressedButtons => Provider.PressedButtons;

    /// <summary>
    /// Gets a collection of all buttons released this frame. A key is only considered released on the first frame that it is not held.
    /// </summary>
    public static IEnumerable<MouseButton> ReleasedButtons => Provider.ReleasedButtons;

    /// <summary>
    /// Returns <see langword="true"/> if the provided button is down; otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonDown(MouseButton button) => Provider.HeldButtons.Contains(button);

    /// <summary>
    /// Returns <see langword="true"/> if the provided button was just pressed this frame; otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonPressed(MouseButton button) => Provider.PressedButtons.Contains(button);

    /// <summary>
    /// Returns <see langword="true"/> if the provided button was just released this frame; otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonReleased(MouseButton button) => Provider.ReleasedButtons.Contains(button);

    public static unsafe void SetCursor(Color[,] colors, int centerX = 0, int centerY = 0)
    {
        int width = colors.GetLength(0);
        int height = colors.GetLength(1);
        
        fixed (Color* colorsPtr = &colors[0, 0])
        {
            SetCursor(new ReadOnlySpan<Color>(colorsPtr, width * height), width, height, centerX, centerY);
        }
    }

    public static void SetCursor(ITexture texture, int centerX = 0, int centerY = 0)
    {
        SetCursor(texture.Pixels, texture.Width, texture.Height, centerX, centerY);
    }

    public static void SetCursor(ReadOnlySpan<Color> colors, int width, int height, int centerX = 0, int centerY = 0)
    {
        Provider.SetCursor(width, height, colors, centerX, centerY);
    }

    public static void SetCursor(SystemCursor cursor)
    {
        Provider.SetCursor(cursor);
    }
}