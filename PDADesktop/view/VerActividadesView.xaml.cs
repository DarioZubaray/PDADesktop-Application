using MahApps.Metro.Controls.Dialogs;
using PDADesktop.ViewModel;
using System.Windows.Controls;

namespace PDADesktop.View
{
    /// <summary>
    /// Lógica de interacción para VerActividadesView.xaml
    /// </summary>
    public partial class VerActividadesView : UserControl
    {
        VerActividadesViewModel verActividadesViewModel = new VerActividadesViewModel(DialogCoordinator.Instance);
        public VerActividadesView()
        {
            InitializeComponent();
            this.DataContext = verActividadesViewModel;
        }
    }
}
