using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using CoreHtmlToImage;
using ImageFormat = CoreHtmlToImage.ImageFormat;

namespace ZplDesigner.Library
{
    public class ZplImage
    {
        public Bitmap OriginalBmp;

        public ZplImage(Bitmap bitmap)
        {
            OriginalBmp = bitmap;
        }

        public ZplImage(Stream stream)
        {
            OriginalBmp = new Bitmap(stream);
            stream.Dispose();
        }

        public ZplImage(string htmlContent)
        {
            var htmlConverter = new HtmlConverter();
            var imageBytes = htmlConverter.FromHtmlString(htmlContent, 800, ImageFormat.Png);
            using var ms = new MemoryStream(imageBytes);
            OriginalBmp = new Bitmap(ms);
        }


        public string ToZpl(out Bitmap previewBmp, int scale, int ditheringLevel = 0)
        {
            return ZplImageTools.BuildLabel(OriginalBmp, out previewBmp, scale, ditheringLevel);
        }



    }
}
