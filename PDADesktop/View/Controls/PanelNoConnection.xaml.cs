using System.Windows;
using System.Windows.Input;

namespace PDADesktop.View.Controls
{
    /// <summary>
    /// Lógica de interacción para PanelNoConnection.xaml
    /// </summary>
    public partial class PanelNoConnection
    {
        public static readonly DependencyProperty IsLoadingProperty_NC =
    DependencyProperty.Register("IsLoading_NC", typeof(bool), typeof(PanelNoConnection), new UIPropertyMetadata(false));

        public static readonly DependencyProperty MessageProperty_NC =
            DependencyProperty.Register("Message_NC", typeof(string), typeof(PanelNoConnection), new UIPropertyMetadata("Loading..."));

        public static readonly DependencyProperty SubMessageProperty_NC =
            DependencyProperty.Register("SubMessage_NC", typeof(string), typeof(PanelNoConnection), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty ClosePanelCommandProperty_NC =
            DependencyProperty.Register("ClosePanelCommand_NC", typeof(ICommand), typeof(PanelNoConnection));
        public PanelNoConnection()
        {
            InitializeComponent();
        }

        public bool IsLoading_NC
        {
            get { return (bool)GetValue(IsLoadingProperty_NC); }
            set { SetValue(IsLoadingProperty_NC, value); }
        }
        public string Message_NC
        {
            get { return (string)GetValue(MessageProperty_NC); }
            set { SetValue(MessageProperty_NC, value); }
        }
        public string SubMessage_NC
        {
            get { return (string)GetValue(SubMessageProperty_NC); }
            set { SetValue(SubMessageProperty_NC, value); }
        }
        public ICommand ClosePanelCommand_NC
        {
            get { return (ICommand)GetValue(ClosePanelCommandProperty_NC); }
            set { SetValue(ClosePanelCommandProperty_NC, value); }
        }
        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            if (ClosePanelCommand_NC != null)
            {
                ClosePanelCommand_NC.Execute(null);
            }
        }
    }
}
