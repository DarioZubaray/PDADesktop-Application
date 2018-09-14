using log4net;
using PDADesktop.Model;
using System.Collections.Generic;

namespace PDADesktop.ViewModel
{
    class VerAjustesViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<Ajustes> ajustes;
        public List<Ajustes> Ajustes
        {
            get
            {
                return ajustes;
            }
            set
            {
                ajustes = value;
            }
        }

        private Ajustes selectedAdjustment;
        public Ajustes SelectedAdjustment
        {
            get
            {
                return selectedAdjustment;
            }
            set
            {
                selectedAdjustment = value;
                logger.Debug("item seleccionado: " + selectedAdjustment.ToString());
                OnPropertyChanged();
            }
        }

        private bool textbox_eanEnabled;
        public bool Textbox_eanEnabled
        {
            get
            {
                return textbox_eanEnabled;
            }
            set
            {
                textbox_eanEnabled = value;
            }
        }
        private bool textbox_motivoEnabled;
        public bool Textbox_motivoEnabled
        {
            get
            {
                return textbox_motivoEnabled;
            }
            set
            {
                textbox_motivoEnabled = value;
            }
        }
        private bool textbox_cantidadEnabled;
        public bool Textbox_cantidadEnabled
        {
            get
            {
                return textbox_cantidadEnabled;
            }
            set
            {
                textbox_cantidadEnabled = value;
            }
        }

        public VerAjustesViewModel()
        {
            Ajustes = new List<Ajustes>
            {
                new Ajustes(75024956L, "2018-09-14 103512", "VTO", -2L),
                new Ajustes(1736403L, "2018-09-14 103512", "ROT", -1L),
                new Ajustes(75032715L, "2018-09-14 103512", "ROB", -1L)
            };
            Textbox_eanEnabled = false;
            Textbox_motivoEnabled = false;
            Textbox_cantidadEnabled = false;
        }
    }
}
