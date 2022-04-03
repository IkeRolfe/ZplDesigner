using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;
using ZplDesigner.Desktop.Annotations;

namespace ZplDesigner.Desktop.Models
{
    public class Printer
    {
        private DiscoveredPrinter _discoveredPrinter;
        private ZebraPrinter _zebraPrinter;
        private PrinterStatus _status = PrinterStatus.DISCONNECTED;
        public Action<PrinterStatus> StatusChangedAction;
        public bool Connected => _zebraPrinter?.Connection.Connected ?? false;

        public PrinterStatus Status
        {
            get => _status;
            private set
            {
                if (value == _status) return;

                _status = value;
                StatusChangedAction?.Invoke(value);
            }
        }


        public Printer(DiscoveredPrinter discoveredPrinter)
        {
            _discoveredPrinter = discoveredPrinter;
            Connect();
        }

        public void SendCommand(string cmd)
        {
            CheckConnection();
            _zebraPrinter.SendCommand(cmd);
        }

        private void CheckConnection()
        {
            if (!Connected)
            {
                Connect();
            }
        }

        public void Connect()
        {
            if (_zebraPrinter == null)
            {
                var connection = _discoveredPrinter.GetConnection();
                connection.Open();
                _zebraPrinter = ZebraPrinterFactory.GetInstance(connection);
                StartMonitoring();
            }
        }

        public void Calibrate()
        {
            var t = _zebraPrinter.RetrieveObjectsProperties();
            _zebraPrinter.Calibrate();
        }

        public override string ToString()
        {
            return _discoveredPrinter.DiscoveryDataMap["MODEL"] + " | " + _discoveredPrinter.DiscoveryDataMap["SERIAL_NUMBER"];
        }

        public PrinterStatus GetStatus()
        {
            CheckConnection();
            var status = _zebraPrinter.GetCurrentStatus();
            
            if (status.isHeadOpen)
            {
                return PrinterStatus.OPEN;
            }
            if (status.isHeadTooHot)
            {
                return PrinterStatus.TOO_HOT;
            }
            if (status.isPaperOut)
            {
                return PrinterStatus.PAPER_OUT;
            }
            if (status.isPaused)
            {
                return PrinterStatus.PAUSED;
            }
            return PrinterStatus.READY;
        }

        private CancellationToken _monitorCancellationToken;
        public void StartMonitoring()
        {
            _monitorCancellationToken = new CancellationToken(false);
            Task.Run(async () =>
            {
                while (!_monitorCancellationToken.IsCancellationRequested)
                {
                    Status = GetStatus();
                    await Task.Delay(TimeSpan.FromSeconds(5), _monitorCancellationToken);
                }
            }, _monitorCancellationToken);
        }
    }

    public enum PrinterStatus
    {
        READY,
        OPEN,
        TOO_HOT,
        PAPER_OUT,
        PAUSED,
        DISCONNECTED
    }
}
