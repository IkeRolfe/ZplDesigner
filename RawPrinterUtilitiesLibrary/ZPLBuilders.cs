using System;
using System.Collections.Generic;
using System.Text;

namespace RawPrinterUtilitiesLibrary
{
    public class ZPLBuilders
    {
        public static string GenerateSmallProductLabel(string title, string subtitle, string barcode)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("^XA");
            //CF bitmap font type,size
            sb.Append("^CF0,20");
            //FO - font location x,y
            //FD - field data printable data
            //FS - field separator end of FD
            sb.Append("^FO30,0");
            sb.Append("^BY2^BCN,60,N,N,N");
            sb.Append($"^FD{barcode}^FS");
            sb.Append($"^FO30,65^FD{barcode}^FS");
            //Barcode definition honestly a lot going on here don't understand all
            //refer to ZPL documentation in general only use CODE 128 or CODE 39
            sb.Append("^CF0,22");
            sb.Append($"^FO0,90^FB415,10,0,L,0^FD{subtitle}^FS");
            sb.Append("^CF0,22");
            sb.Append($"^FO0,160^FB415,10,0,L,0^FD{title}^FS");
            sb.Append("^XZ");
            return sb.ToString();
        }

        public static string TestZplData()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("^XA");
            sb.Append("^CF0,60");
            sb.Append("^FO220,50^FDTEST PRINT^FS");
            sb.Append("^XZ");

            return sb.ToString();
        }
    }
}
