using SkiaSharp;
using System;
using System.IO;
using AvaloniaApplicationTestTask.Services.Interfaces;

namespace AvaloniaApplicationTestTask.Services
{
    public class GraphGenerator : IGraphGenerator
    {
        private static readonly Random rand = new();
        private static readonly string pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "graph.png");

        public string GenerateGraph()
        {
            if (File.Exists(pathToFile)) File.Delete(pathToFile);

            int width = 400, height = 300;
            float padding = 50;

            using var bitmap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            using var paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                StrokeWidth = 4,
                Color = GetRandomColor()
            };

            int shapeType = rand.Next(3);

            switch (shapeType)
            {
                case 0:
                    canvas.DrawRect(new SKRect(padding, padding, width - padding, height - padding), paint);
                    break;
                case 1:
                    canvas.DrawCircle(width / 2, height / 2, Math.Min(width, height) / 3, paint);
                    break;
                case 2:
                    using (var path = new SKPath())
                    {
                        path.MoveTo(width / 2, padding);
                        path.LineTo(padding, height - padding);
                        path.LineTo(width - padding, height - padding);
                        path.Close();
                        canvas.DrawPath(path, paint);
                    }
                    break;
            }

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            File.WriteAllBytes(pathToFile, data.ToArray());

            return pathToFile;
        }

        private static SKColor GetRandomColor()
        {
            return new SKColor((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));
        }
    }
}
