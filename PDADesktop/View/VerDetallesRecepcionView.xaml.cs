using MahApps.Metro.Controls.Dialogs;
using PDADesktop.ViewModel;
using System.Windows.Controls;

namespace PDADesktop.View
{
    /// <summary>
    /// Lógica de interacción para VerDetallesRecepcionView.xaml
    /// </summary>
    public partial class VerDetallesRecepcionView : UserControl
    {
        VerDetallesRecepcionViewModel verDetalleRecepcionViewModel = new VerDetallesRecepcionViewModel(DialogCoordinator.Instance);
        public VerDetallesRecepcionView()
        {
            InitializeComponent();
            this.DataContext = verDetalleRecepcionViewModel;
        }
    }
}
