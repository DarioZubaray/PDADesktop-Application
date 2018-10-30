using MahApps.Metro.Controls.Dialogs;
using PDADesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PDADesktop.View
{
    /// <summary>
    /// Lógica de interacción para ImprimirRecepcionView.xaml
    /// </summary>
    public partial class ImprimirRecepcionView : UserControl
    {
        ImprimirRecepcionViewModel imprimirRecepcionViewModel = new ImprimirRecepcionViewModel(DialogCoordinator.Instance);

        public ImprimirRecepcionView()
        {
            InitializeComponent();
            this.DataContext = imprimirRecepcionViewModel;
        }
    }
}
