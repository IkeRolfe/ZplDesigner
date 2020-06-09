using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;

namespace ZplDesigner.Library
{
    public static class ZplImageTools
    {
        private const int BLACK_LIMIT = 155;
        private static readonly uint[] Lookup32Unsafe = CreateHexLookup();

        private static readonly unsafe uint* Lookup32UnsafeP
            = (uint*)GCHandle.Alloc(Lookup32Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();

        private static readonly string[] MapHex =
        {
            "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "g", "gG",
            "gH", "gI", "gJ", "gK", "gL", "gM", "gN", "gO", "gP", "gQ", "gR", "gS", "gT", "gU", "gV", "gW", "gX", "gY",
            "h", "hG", "hH", "hI", "hJ", "hK", "hL", "hM", "hN", "hO", "hP", "hQ", "hR", "hS", "hT", "hU", "hV", "hW",
            "hX", "hY", "i", "iG", "iH", "iI", "iJ", "iK", "iL", "iM", "iN", "iO", "iP", "iQ", "iR", "iS", "iT", "iU",
            "iV", "iW", "iX", "iY", "j", "jG", "jH", "jI", "jJ", "jK", "jL", "jM", "jN", "jO", "jP", "jQ", "jR", "jS",
            "jT", "jU", "jV", "jW", "jX", "jY", "k", "kG", "kH", "kI", "kJ", "kK", "kL", "kM", "kN", "kO", "kP", "kQ",
            "kR", "kS", "kT", "kU", "kV", "kW", "kX", "kY", "l", "lG", "lH", "lI", "lJ", "lK", "lL", "lM", "lN", "lO",
            "lP", "lQ", "lR", "lS", "lT", "lU", "lV", "lW", "lX", "lY", "m", "mG", "mH", "mI", "mJ", "mK", "mL", "mM",
            "mN", "mO", "mP", "mQ", "mR", "mS", "mT", "mU", "mV", "mW", "mX", "mY", "n", "nG", "nH", "nI", "nJ", "nK",
            "nL", "nM", "nN", "nO", "nP", "nQ", "nR", "nS", "nT", "nU", "nV", "nW", "nX", "nY", "o", "oG", "oH", "oI",
            "oJ", "oK", "oL", "oM", "oN", "oO", "oP", "oQ", "oR", "oS", "oT", "oU", "oV", "oW", "oX", "oY", "p", "pG",
            "pH", "pI", "pJ", "pK", "pL", "pM", "pN", "pO", "pP", "pQ", "pR", "pS", "pT", "pU", "pV", "pW", "pX", "pY",
            "q", "qG", "qH", "qI", "qJ", "qK", "qL", "qM", "qN", "qO", "qP", "qQ", "qR", "qS", "qT", "qU", "qV", "qW",
            "qX", "qY", "r", "rG", "rH", "rI", "rJ", "rK", "rL", "rM", "rN", "rO", "rP", "rQ", "rR", "rS", "rT", "rU",
            "rV", "rW", "rX", "rY", "s", "sG", "sH", "sI", "sJ", "sK", "sL", "sM", "sN", "sO", "sP", "sQ", "sR", "sS",
            "sT", "sU", "sV", "sW", "sX", "sY", "t", "tG", "tH", "tI", "tJ", "tK", "tL", "tM", "tN", "tO", "tP", "tQ",
            "tR", "tS", "tT", "tU", "tV", "tW", "tX", "tY", "u", "uG", "uH", "uI", "uJ", "uK", "uL", "uM", "uN", "uO",
            "uP", "uQ", "uR", "uS", "uT", "uU", "uV", "uW", "uX", "uY", "v", "vG", "vH", "vI", "vJ", "vK", "vL", "vM",
            "vN", "vO", "vP", "vQ", "vR", "vS", "vT", "vU", "vV", "vW", "vX", "vY", "w", "wG", "wH", "wI", "wJ", "wK",
            "wL", "wM", "wN", "wO", "wP", "wQ", "wR", "wS", "wT", "wU", "wV", "wW", "wX", "wY", "x", "xG", "xH", "xI",
            "xJ", "xK", "xL", "xM", "xN", "xO", "xP", "xQ", "xR", "xS", "xT", "xU", "xV", "xW", "xX", "xY", "y", "yG",
            "yH", "yI", "yJ", "yK", "yL", "yM", "yN", "yO", "yP", "yQ", "yR", "yS", "yT", "yU", "yV", "yW", "yX", "yY",
            "z", "zG", "zH", "zI", "zJ", "zK", "zL", "zM", "zN", "zO", "zP", "zQ", "zR", "zS", "zT", "zU", "zV", "zW",
            "zX", "zY"
        };

        // // This generates the above array.
        // private static string[] CreateHexCompressionMapping()
        // {
        //     var returnResult = new string[419];
        //     const string capitalLetters = "GHIJKLMNOPQRSTUVWXY";
        //     const string lowercaseLetters = "ghijklmnopqrstuvwxyz";
        //     var returnIndex = 0;
        //     for (var i = 0; i < capitalLetters.Length; i++)
        //         returnResult[returnIndex++] = new string(capitalLetters[i], 1);
        //     for (var i = 0; i < lowercaseLetters.Length; i++)
        //     for (var j = 0; j < capitalLetters.Length; j++)
        //     {
        //         if (j % 20 == 0) returnResult[returnIndex++] = new string(lowercaseLetters[i], 1);
        //         returnResult[returnIndex++] = new string(new[] {lowercaseLetters[i], capitalLetters[j]});
        //     }
        //
        //     return returnResult;
        // }

        private static uint[] CreateHexLookup()
        {
            var result = new uint[256];
            for (var i = 0; i < 256; i++)
            {
                var s = i.ToString("X2");
                if (BitConverter.IsLittleEndian)
                    result[i] = s[0] + ((uint)s[1] << 16);
                else
                    result[i] = s[1] + ((uint)s[0] << 16);
            }

            return result;
        }

        public static string BuildLabel(Bitmap bmp, out Bitmap previewBmp, int scale, int ditheringLevel = 0)
        {
            if (scale != 100)
            {
                bmp = ScaleBitmap(bmp, scale);
            }
            var widthBytes = GetImageWidthInBytes(bmp);
            var total = widthBytes * bmp.Height;
            var body = ConvertBitmapToHex(bmp, out previewBmp, ditheringLevel);
            return "^XA\r\n^FO0,0" // Start of header
                   +
                   string.Join(',', "^GFA", total, total, widthBytes) +
                   "," // Graphic line declaration
                   +
                   CompressHex(body, widthBytes) // Hex body compressed
                   +
                   "^FS\r\n^XZ"; // closing
        }

        private static unsafe string ByteArrayToHex(byte[] bytes)
        {
            var lookupP = Lookup32UnsafeP;
            var result = new char[bytes.Length * 2];
            fixed (byte* bytesP = bytes)
            fixed (char* resultP = result)
            {
                var resultP2 = (uint*)resultP;
                for (var i = 0; i < bytes.Length; i++) resultP2[i] = lookupP[bytesP[i]];
            }

            return new string(result);
        }


        private static Bitmap ScaleBitmap(Image image, int scalePercent)
        {
            var width = scalePercent * image.Width / 100;
            var height = scalePercent * image.Height / 100;
            var bitmap = new Bitmap(image, width, height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = InterpolationMode.Bicubic;
            }

            return bitmap;
        }

        private static int GetImageWidthInBytes(Bitmap originalImage)
        {
            var width = originalImage.Width;
            int widthBytes;
            if (width % 8 > 0)
                widthBytes = width / 8 + 1;
            else
                widthBytes = width / 8;
            return widthBytes;
        }

        private static string ConvertBitmapToHex(Bitmap originalImage, out Bitmap previewBitmap, int ditheringLevel)
        {
            //Start here so first full line is completed
            var index = 7;
            var current = 0b0000_0000;
            var height = originalImage.Height;
            var width = originalImage.Width;
            //for live preview
            previewBitmap = DrawFilledRectangle(width,height);
            //Round up to the nearest complete bits then covert back
            //Need this to calculate array length since last bit of row may be calculated without 8 pixels
            var bitRoundedWidth = GetImageWidthInBytes(originalImage) * 8;
            //Not sure where this extra bit comes from
            var bytes = new byte[height*bitRoundedWidth/8+1];
            //var byteList = new List<byte>();
            var i = 0;
            var rand = new Random();

            for (var h = 0; h < height; h++)
            {
                for (var w = 0; w < width; w++)
                {
                    var rgb = originalImage.GetPixel(w, h);
                    var totalColor = (rgb.R + rgb.G + rgb.B) / 3;
                    if (ditheringLevel > 0)
                    {
                        var whiteBoundary = 255 - ditheringLevel;
                        var blackBoundary = ditheringLevel;
                        if (totalColor < whiteBoundary)
                        {
                            totalColor = 0;
                        }
                        else if (totalColor > blackBoundary)
                        {
                            totalColor = 255;
                        }
                        else
                        {
                            totalColor = totalColor > rand.Next() % 256 ? 255 : 0;
                        }
                    }
                    //Average color or alpha channel
                    if (totalColor < BLACK_LIMIT && rgb.A >= BLACK_LIMIT)
                    {
                        current |= 1 << index;
                        //update preview
                        previewBitmap.SetPixel(w, h, Color.Black);
                    }

                    index--;

                    if (index == -1 || w == width - 1)
                    {
                        bytes[i] = (byte)current;
                        //byteList.Add((byte)current);
                        index = 7;
                        current = 0b0000_0000;
                        i++;
                    }

                }
            }
                
            //var bytes = byteList.ToArray();
            return ByteArrayToHex(bytes);
        }

        private static string CompressHex(string code, int widthBytes)
        {
            var maxLineLength = widthBytes * 2;

            var sbCode = new StringBuilder();
            var sbLinea = new StringBuilder();
            string previousLine = null;
            var counter = 1;
            var firstChar = code[0];
            for (var i = 1; i < code.Length; i++)
            {
                if (i % maxLineLength == 0 || i == code.Length - 1)
                {
                    if (counter >= maxLineLength - 1 && firstChar == '0')
                        sbLinea.Append(",");
                    else if (counter >= maxLineLength - 1 && firstChar == 'F')
                        sbLinea.Append("!");
                    else
                        sbLinea.Append(MapHex[counter - 1] + firstChar);
                    if (sbLinea.ToString().Equals(previousLine))
                        sbCode.Append(":");
                    else
                        sbCode.Append(sbLinea.ToString());
                    previousLine = sbLinea.ToString();
                    sbLinea.Clear();
                    firstChar = code[i];
                    counter = 0;
                }

                if (firstChar == code[i])
                {
                    counter++;
                }
                else
                {
                    sbLinea.Append(MapHex[counter - 1] + firstChar);
                    counter = 1;
                    firstChar = code[i];
                }
            }

            return sbCode.ToString();
        }

        //Get white bmp for preview
        private static Bitmap DrawFilledRectangle(int x, int y)
        {
            Bitmap bmp = new Bitmap(x, y);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle ImageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(Brushes.Transparent, ImageSize);
            }

            bmp.MakeTransparent(Color.White);
            return bmp;
        }
    }

}