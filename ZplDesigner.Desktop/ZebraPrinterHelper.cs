using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;
using ZplDesigner.Desktop.Models;

namespace ZplDesigner.Desktop
{
    public static class ZebraPrinterHelper
    {
        public static IEnumerable<Printer> GetConnectedPrinters()
        {
            var printers = UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter());
            /*var printers = discoveredUsbPrinters.Select(d => {
                var conn = d.GetConnection();
                conn.Open();
                return ZebraPrinterFactory.GetInstance(conn);
            });*/
            var p = printers?.FirstOrDefault();
            if (p == null) return null;
            var model = p.DiscoveryDataMap["MODEL"] + " | " + p.DiscoveryDataMap["SERIAL_NUMBER"];
            return printers.Select(p=> new Printer(p));
        }
    }
}