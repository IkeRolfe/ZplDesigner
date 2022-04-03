using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZplDesigner.Desktop.Annotations;
using ZplDesigner.Desktop.Models;

namespace ZplDesigner.Desktop.Controls
{
    /// <summary>
    /// Interaction logic for ZebraPrinterConfigControl.xaml
    /// </summary>
    public partial class ZebraPrinterConfigControl : UserControl, INotifyPropertyChanged
    {
        public ZebraPrinterConfigControl()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty PrinterProperty = DependencyProperty.Register(
            "Printer", typeof(Printer), typeof(ZebraPrinterConfigControl), new PropertyMetadata(default(Printer)));

        
        public Printer Printer
        {
            get => (Printer) GetValue(PrinterProperty);
            set
            {
                SetValue(PrinterProperty, value); 
                Printer.StatusChangedAction = OnStatusChanged;
                OnPropertyChanged("");
            }
        }

        public PrinterStatus Status => Printer?.Status ?? PrinterStatus.DISCONNECTED;

        public bool Connected => Printer != null;


        private void OnStatusChanged(PrinterStatus status)
        {
            OnPropertyChanged(nameof(Status));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Calibrate_OnClick(object sender, RoutedEventArgs e)
        {
            Printer.Calibrate();
        }
    }
}
