using MahApps.Metro.Controls.Dialogs;
using PDADesktop.ViewModel;
using System.Windows.Controls;

namespace PDADesktop.View
{
    /// <summary>
    /// Lógica de interacción para VerAjustesRealizadosView.xaml
    /// </summary>
    public partial class VerAjustesRealizadosView : UserControl
    {
        VerAjustesRealizadosViewModel verAjustesRealizadosViewModel = new VerAjustesRealizadosViewModel(DialogCoordinator.Instance);
        public VerAjustesRealizadosView()
        {
            InitializeComponent();
            this.DataContext = verAjustesRealizadosViewModel;
        }
    }
}
