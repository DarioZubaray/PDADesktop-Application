using MahApps.Metro.Controls.Dialogs;
using PDADesktop.ViewModel;
using System.Windows.Controls;

namespace PDADesktop.View
{
    /// <summary>
    /// Lógica de interacción para VerAjustesInformadosView.xaml
    /// </summary>
    public partial class VerAjustesInformadosView : UserControl
    {
        VerAjustesInformadosViewModel verAjustesInformadosViewModel = new VerAjustesInformadosViewModel(DialogCoordinator.Instance);

        public VerAjustesInformadosView()
        {
            InitializeComponent();
            this.DataContext = verAjustesInformadosViewModel;
        }
    }
}
