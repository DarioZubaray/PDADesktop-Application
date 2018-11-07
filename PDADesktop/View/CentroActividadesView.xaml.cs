using PDADesktop.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PDADesktop.View
{
    /// <summary>
    /// Lógica de interacción para CentroActividades.xaml
    /// </summary>
    public partial class CentroActividadesView : UserControl
    {
        CentroActividadesViewModel centroActividadesViewModel = new CentroActividadesViewModel();

        public CentroActividadesView()
        {
            InitializeComponent();
            this.DataContext = centroActividadesViewModel;
        }
    }
}
