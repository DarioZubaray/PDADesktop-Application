using MahApps.Metro.Controls.Dialogs;
using PDADesktop.ViewModel;
using System.Windows.Controls;

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
