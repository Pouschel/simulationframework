﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public interface IBitmap : ISurface
{
    Span<Color> GetPixels();
    IntPtr GetPixelData(out int rowSize, out int rowCount);
}