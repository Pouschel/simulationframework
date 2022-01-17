﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.IMGUI;

/// <summary>
/// Provides customization options for ImGui windows.
/// </summary>
public enum WindowFlags
{
    /// <summary>
    /// Default window
    /// </summary>
    None = 0,
    /// <summary>
    /// Disable title-bar
    /// </summary>
    NoTitleBar = 1 << 0,
    /// <summary>
    /// Disable user resizing with the lower-right grip
    /// </summary>
    NoResize = 1 << 1,
    /// <summary>
    /// Disable user moving the window
    /// </summary>
    NoMove = 1 << 2,
    /// <summary>
    /// Disable scrollbars (window can still scroll with mouse or programmatically)
    /// </summary>
    NoScrollbar = 1 << 3,
    /// <summary>
    /// Disable user vertically scrolling with mouse wheel. On child window, mouse wheel will be forwarded to the parent unless NoScrollbar is also set.
    /// </summary>
    NoScrollWithMouse = 1 << 4,
    /// <summary>
    /// Disable user collapsing window by double-clicking on it. Also referred to as Window Menu Button (e.g. within a docking node).
    /// </summary>
    NoCollapse = 1 << 5,
    /// <summary>
    /// Resize every window to its content every frame
    /// </summary>
    AlwaysAutoResize = 1 << 6,
    /// <summary>
    /// Disable drawing background color (WindowBg, etc.) and outside border. Similar as using SetNextWindowBgAlpha(0.0f).
    /// </summary>
    NoBackground = 1 << 7,
    /// <summary>
    /// Never load/save settings in .ini file
    /// </summary>
    NoSavedSettings = 1 << 8,
    /// <summary>
    /// Disable catching mouse, hovering test with pass through.
    /// </summary>
    NoMouseInputs = 1 << 9,
    /// <summary>
    /// Has a menu-bar
    /// </summary>
    MenuBar = 1 << 10,
    /// <summary>
    /// Allow horizontal scrollbar to appear (off by default). You may use SetNextWindowContentSize(ImVec2(width,0.0f)); prior to calling Begin() to specify width. Read code in imgui_demo in the "Horizontal Scrolling" section.
    /// </summary>
    HorizontalScrollbar = 1 << 11,
    /// <summary>
    /// Disable taking focus when transitioning from hidden to visible state
    /// </summary>
    NoFocusOnAppearing = 1 << 12,
    /// <summary>
    /// Disable bringing window to front when taking focus (e.g. clicking on it or programmatically giving it focus)
    /// </summary>
    NoBringToFrontOnFocus = 1 << 13,
    /// <summary>
    /// Always show vertical scrollbar (even if ContentSize.y &gt Size.y)
    /// </summary>
    AlwaysVerticalScrollbar = 1 << 14,
    /// <summary>
    /// Always show horizontal scrollbar (even if ContentSize.x &lt Size.x)
    /// </summary>
    AlwaysHorizontalScrollbar = 1 << 15,
    /// <summary>
    /// Ensure child windows without border uses style.WindowPadding (ignored by default for non-bordered child windows, because more convenient)
    /// </summary>
    AlwaysUseWindowPadding = 1 << 16,
    /// <summary>
    /// No gamepad/keyboard navigation within the window
    /// </summary>
    NoNavInputs = 1 << 18,
    /// <summary>
    /// No focusing toward this window with gamepad/keyboard navigation (e.g. skipped by CTRL+TAB)
    /// </summary>
    NoNavFocus = 1 << 19,
    /// <summary>
    /// Display a dot next to the title. When used in a tab/docking context, tab is selected when clicking the X + closure is not assumed (will wait for user to stop submitting the tab). Otherwise closure is assumed when pressing the X, so if you keep submitting the tab may reappear at end of tab bar.
    /// </summary>
    UnsavedDocument = 1 << 20,
    NoNav = NoNavInputs | NoNavFocus,
    NoDecoration = NoTitleBar | NoResize | NoScrollbar | NoCollapse,
    NoInputs = NoMouseInputs | NoNavInputs | NoNavFocus,
}