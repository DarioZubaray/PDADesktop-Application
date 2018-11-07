using PDADesktop.ViewModel;
using System.Windows.Controls;

namespace PDADesktop.View
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        LoginViewModel loginViewModel = new LoginViewModel();

        public LoginView()
        {
            InitializeComponent();
            this.DataContext = loginViewModel;
        }
    }
}
