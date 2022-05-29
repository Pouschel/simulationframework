﻿using SimulationFramework.Drawing.Canvas;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SkiaSharp;

internal sealed class SkiaCanvasState : CanvasState, IDisposable
{
    private SKCanvas canvas;
    private SKPaint paint;

    public bool IsActive => canvas is not null;
    public SKPaint Paint => paint ??= new SKPaint();

    private SKShader fillTextureShader;
    private SKShader fillGradientShader;
    private SKShader strokeGradientShader;

    public SkiaCanvasState(SKCanvas canvas, SkiaCanvasState other = null)
    {
        this.canvas = canvas;
        this.Initialize(other);
    }

    protected override void Initialize(CanvasState other)
    {
        if (other is null)
        {
            this.paint = new SKPaint();
        }
        else
        {
            this.paint = ((SkiaCanvasState)other).paint.Clone();
            this.fillTextureShader = GradientShaderCache.GetShader(other.FillGradient);
            this.fillGradientShader = GradientShaderCache.GetShader(other.StrokeGradient);
            this.strokeGradientShader = GradientShaderCache.GetShader(other.StrokeGradient);
        }

        base.Initialize(other);
    }

    public void Dispose()
    {
        paint.Dispose();
    }

    protected override void UpdateTransform(Matrix3x2 transform)
    {
        canvas.SetMatrix(transform.AsSKMatrix());
        base.UpdateTransform(transform);
    }

    protected override void UpdateFillColor(Color fillColor)
    {
        paint.Color = fillColor.AsSKColor();
        base.UpdateFillColor(fillColor);
    }
}

//internal sealed class SkiaCanvasState : CanvasState, IDisposable
//{
//    private SKCanvas canvas;
//    private SKPaint paint;

//    public bool IsActive => canvas is not null;
//    public SKPaint Paint => paint;

//    private SKShader fillTextureShader;
//    private SKShader gradientShader;

//    private SkiaCanvasState()
//    {
//    }


//    public void Dispose()
//    {
//        paint.Dispose();
//    }

//    public void UpdateDrawMode(DrawMode mode)
//    {
//        switch (mode)
//        {
//            case DrawMode.Fill:
//                paint.Style = SKPaintStyle.Fill;
//                break;
//            case DrawMode.Stroke:
//                paint.Style = SKPaintStyle.Stroke;
//                break;
//            case DrawMode.FillGradient:
//                paint.Style = SKPaintStyle.Fill;
//                paint.Shader = gradientShader;
//                break;
//            case DrawMode.StrokeGradient:
//                paint.Style = SKPaintStyle.Fill;
//                paint.Shader = gradientShader;
//                break;
//            case DrawMode.Textured:
//                paint.Style = SKPaintStyle.Fill;
//                paint.Shader = fillTextureShader;
//                break;
//            default:
//                throw new ArgumentException("Unknown DrawMode value!", nameof(mode));
//        }

//        this.DrawMode = mode;
//    }

//    public void UpdateColor(Color color)
//    {
//        this.Color = color;
//        paint.Color = color.AsSKColor();
//    }

//    public void UpdateStrokeWidth(float strokeWidth)
//    {
//        this.StrokeWidth = strokeWidth;
//        this.paint.StrokeWidth = strokeWidth;
//    }

//    internal void UpdateFillTexture(FillTexture texture)
//    {
//        this.FillTexture = texture;

//        this.fillTextureShader?.Dispose();

//        if (texture.Texture is not null)
//        {
//            this.fillTextureShader = SKShader.CreateBitmap(
//                SkiaInterop.GetBitmap(texture.Texture),
//                texture.TileModeX.AsSKShaderTileMode(),
//                texture.TileModeY.AsSKShaderTileMode(),
//                texture.Transform.AsSKMatrix()
//                );
//        }

//        if (this.DrawMode == DrawMode.Textured)
//        {
//            paint.Shader = fillTextureShader;
//        }
//    }

//    internal void UpdateGradient(Gradient gradient)
//    {
//        this.Gradient = gradient;

//        this.gradientShader?.Dispose();

//        if (gradient is not null)
//        {
//            this.gradientShader = GradientShaderCache.GetShader(gradient);
//        }
        
//        if (this.DrawMode == DrawMode.Gradient)
//        {
//            paint.Shader = gradientShader;
//        }
//    }

//    public void UpdateTransform(Matrix3x2 transform)
//    {
//        this.Transform = transform;

//        if (this.IsActive)
//        {
//            this.canvas.SetMatrix(transform.AsSKMatrix());
//        }
//    }

//    public void UpdateClipRect(Rectangle? clipRect)
//    {
//        this.ClipRect = clipRect;

//        if (this.IsActive)
//        {
//            //throw new InvalidOperationException("Clipping is not supported on skia!");
//        }
//    }
//}