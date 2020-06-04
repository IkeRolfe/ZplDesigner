using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace ZplDesigner.Library
{
    public class ZplImageConverter
    {
        public Bitmap OriginalBmp;

        public ZplImageConverter()
        {

        }
        public ZplImageConverter(string path)
        {
            OriginalBmp = new Bitmap(path);
        }

        
        public string ToZpl(out Bitmap previewBmp, int scale,int ditheringLevel = 0)
        {
            //TODO static class
            var zplTools = new ZplTools();
            return zplTools.BuildLabel(OriginalBmp, out previewBmp, scale, ditheringLevel);
        }

        public static string SpliceText(string text, int lineLength)
        {
            return Regex.Replace(text, "(.{" + lineLength + "})", "$1" + Environment.NewLine);
        }


    }
}
