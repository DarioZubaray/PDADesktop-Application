using log4net;
using PDADesktop.Classes;
using PDADesktop.Model;
using System.Collections.Generic;
using System.Windows.Input;

namespace PDADesktop.ViewModel
{
    class VerAjustesViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Attributes
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
                if(selectedAdjustment != null)
                {
                    Textbox_cantidadEnabled = true;
                }
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
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

            EliminarAjusteCommand = new RelayCommand(EliminarAjusteButton);
            ActualizarAjusteCommand = new RelayCommand(ActualizarAjusteButton);
            DescartarCambiosCommand = new RelayCommand(DescartarCambiosButton);
            GuardarCambiosCommand = new RelayCommand(GuardarCambiosButton);
        }
        #endregion

        #region Commands
        private ICommand eliminarAjusteCommand;
        public ICommand EliminarAjusteCommand
        {
            get
            {
                return eliminarAjusteCommand;
            }
            set
            {
                eliminarAjusteCommand = value;
            }
        }

        private ICommand actualizarAjusteCommand;
        public ICommand ActualizarAjusteCommand
        {
            get
            {
                return actualizarAjusteCommand;
            }
            set
            {
                actualizarAjusteCommand = value;
            }
        }

        private ICommand descartarCambiosCommand;
        public ICommand DescartarCambiosCommand
        {
            get
            {
                return descartarCambiosCommand;
            }
            set
            {
                descartarCambiosCommand = value;
            }
        }

        private ICommand guardarAjustesCommand;
        public ICommand GuardarCambiosCommand
        {
            get
            {
                return guardarAjustesCommand;
            }
            set
            {
                guardarAjustesCommand = value;
            }
        }
        #endregion

        #region Methods
        public void EliminarAjusteButton(object obj)
        {
            logger.Debug("EliminarAjusteButton");
        }
        public void ActualizarAjusteButton(object obj)
        {
            logger.Debug("ActualizarAjusteButton");
        }
        public void DescartarCambiosButton(object obj)
        {
            logger.Debug("DescartarCambiosButton");
        }
        public void GuardarCambiosButton(object obj)
        {
            logger.Debug("GuardarCambiosButton");
        }
        #endregion
    }
}
