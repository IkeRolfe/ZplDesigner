using System;
using System.Collections.Generic;
using System.Text;

namespace RawPrinterUtilitiesLibrary
{
    public class Label
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Barcode { get; set; }
        public string ZplData { get; set; }
        public int Quantity { get; set; } = 1;

        //A base label, with things 
        public Label()
        {
        }
        //Constructor for adding zpl on new label creation
        public Label(string zplData)
        {
            ZplData = zplData;
        }
        //Constructor for adding a title and ZPLData
        public Label(string title, string zplData)
        {
            Title = title;
            ZplData = zplData;
        }
        public Label(string title, string subtitle, string barcode, int quantity)
        {
            Title = title;
            Subtitle = subtitle;
            Barcode = barcode;
            Quantity = quantity;
        }
    }
}
