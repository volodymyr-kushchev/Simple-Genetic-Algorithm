using SkiaSharp;

namespace UI;

public static class SkiaSharpExtensions
{
    public static SKColor ToSkiaColor(this Color color)
    {
        return new SKColor(color.R, color.G, color.B, color.A);
    }

    public static SKRect ToSkiaRect(this Rectangle rect)
    {
        return new SKRect(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
    }
}