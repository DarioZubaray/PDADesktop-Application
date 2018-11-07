using MahApps.Metro.Controls.Dialogs;
using PDADesktop.ViewModel;
using System.Windows.Controls;

namespace PDADesktop.View
{
    /// <summary>
    /// Lógica de interacción para BuscarLotesView.xaml
    /// </summary>
    public partial class BuscarLotesView : UserControl
    {
        BuscarLotesViewModel buscarLotesViewModel = new BuscarLotesViewModel(DialogCoordinator.Instance);
        public BuscarLotesView()
        {
            InitializeComponent();
            this.DataContext = buscarLotesViewModel;
        }
    }
}
