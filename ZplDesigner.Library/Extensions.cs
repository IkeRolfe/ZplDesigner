using System.Drawing;

namespace ZplDesigner.Library
{
    public static class Extensions
    {
        public static Bitmap ToGrayscale(this Bitmap bitmap)
        {
            var result = new Bitmap(bitmap.Width, bitmap.Height);
            for (int x = 0; x < bitmap.Width; x++)
            for (int y = 0; y < bitmap.Height; y++)
            {
                var grayColor = ToGrayscaleColor(bitmap.GetPixel(x, y));
                result.SetPixel(x, y, grayColor);
            }
            return result;
        }

        public static Color ToGrayscaleColor(Color color)
        {
            var level = (byte)((color.R + color.G + color.B) / 3);
            var result = Color.FromArgb(level, level, level);
            return result;
        }

    }
}