using MahApps.Metro.Controls.Dialogs;
using PDADesktop.ViewModel;
using System.Windows.Controls;

namespace PDADesktop.View
{
    /// <summary>
    /// Lógica de interacción para VerAjustesModificarView.xaml
    /// </summary>
    public partial class VerAjustesModificarView : UserControl
    {
        VerAjustesModificarViewModel verAjustesModificadosViewModel = new VerAjustesModificarViewModel(DialogCoordinator.Instance);
        public VerAjustesModificarView()
        {
            InitializeComponent();
            this.DataContext = verAjustesModificadosViewModel;
        }
    }
}
