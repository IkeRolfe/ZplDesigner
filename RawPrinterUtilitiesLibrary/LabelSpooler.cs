using System;
using System.Collections.Generic;
using System.Windows;
using System.Drawing.Printing;
using System.Collections.ObjectModel;

namespace RawPrinterUtilitiesLibrary
{
    public class PrinterDetail
    {
        public string PrinterName { get; set; }
    }
    public class LabelSpooler
    {
        private string _printerName;
        private static List<Label> _labelQueue = new List<Label>();

        public LabelSpooler(string printerName)
        {
            _printerName = printerName;
        }
        public LabelSpooler()
        {
            _printerName = null;
        }

        public string PrinterName
        {
            get => _printerName;
            set
            {
                _printerName = value;
            }
        }

        public List<string> GetAvailablePrinters()
        {
            var printers = new List<string>();

            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                //printers.Add(printer);
                //PrinterDetail pd = new PrinterDetail();
                //pd.PrinterName = printer;
                //printers.Add(pd);
                printers.Add(printer);
            }

            return printers;
        }

        public void UpdatePrinterName(string printerName)
        {
            _printerName = printerName;
        }

        public void AddLabel(Label labelToAdd)
        {
            _labelQueue.Add(labelToAdd);
        }

        public void RemoveLabel(Label labelToRemove)
        {
            _labelQueue.Remove(labelToRemove);
        }

        public void ClearQueue()
        {
            _labelQueue.Clear();
        }

        public int QueueSize()
        {
            return _labelQueue.Count;
        }

        public bool PrintQueue()
        {
            if (_labelQueue.Count == 0)
            {
                throw new Exception("Print Queue is Empty");
            }

            foreach (var label in _labelQueue)
            {
                //var zplData = ZPLBuilders.GenerateSmallProductLabel(label.Title, label.Subtitle, label.Barcode);
                var zplData = label.ZplData;
                for (int num = 0; num < label.Quantity; ++num)
                {
                    try
                    {
                        RawPrinterHelper.SendStringToPrinter(_printerName, zplData);
                        System.Diagnostics.Debug.WriteLine("A label should have been printed.");
                    }
                    catch (Exception e)
                    {
                        // Handle if print goes wrong, subtract quantity printed from quantity total.
                        System.Diagnostics.Debug.WriteLine("Exeption Occured: " + e);
                        label.Quantity -= num + 1;
                        throw new Exception(e.Message);
                        //return false;
                    }
                }
            }
            _labelQueue.Clear();

            return true;
        }
    }
}
