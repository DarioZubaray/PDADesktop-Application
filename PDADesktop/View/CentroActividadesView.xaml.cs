using MahApps.Metro.Controls.Dialogs;
using PDADesktop.ViewModel;
using System.Windows.Controls;

namespace PDADesktop.View
{
    /// <summary>
    /// Lógica de interacción para CentroActividades.xaml
    /// </summary>
    public partial class CentroActividadesView : UserControl
    {
        CentroActividadesViewModel centroActividadesViewModel = new CentroActividadesViewModel(DialogCoordinator.Instance);
        public CentroActividadesView()
        {
            InitializeComponent();
            DataContext = centroActividadesViewModel;
        }
    }
}
