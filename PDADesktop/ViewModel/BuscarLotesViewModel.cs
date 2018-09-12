using log4net;
using PDADesktop.Classes;
using PDADesktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PDADesktop.ViewModel
{
    class BuscarLotesViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string start;
        public string Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }
        private string end;
        public string End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }
        private string totalItems;
        public string TotalItems
        {
            get
            {
                return totalItems;
            }
            set
            {
                totalItems = value;
            }
        }
        private List<Lote> lotes;
        public List<Lote> Lotes
        {
            get
            {
                return lotes;
            }
            set
            {
                lotes = value;
            }
        }

        private ICommand firstCommand;
        public ICommand FirstCommand
        {
            get
            {
                return firstCommand;
            }
            set
            {
                firstCommand = value;
            }
        }

        private ICommand previousCommand;
        public ICommand PreviousCommand
        {
            get
            {
                return previousCommand;
            }
            set
            {
                previousCommand = value;
            }
        }

        private ICommand nextCommand;
        public ICommand NextCommand
        {
            get
            {
                return nextCommand;
            }
            set
            {
                previousCommand = value;
            }
        }

        private ICommand lastCommand;
        public ICommand LastCommand
        {
            get
            {
                return lastCommand;
            }
            set
            {
                lastCommand = value;
            }
        }

        private bool canExecute = true;

        public BuscarLotesViewModel()
        {
            Start = "1";
            End = "5";
            TotalItems = "5";
            Lotes = new List<Lote>
            {
                new Lote(1, DateTime.Now, 706),
                new Lote(2, DateTime.Now, 706),
                new Lote(3, DateTime.Now, 706),
                new Lote(4, DateTime.Now, 706),
                new Lote(5, DateTime.Now, 706)
            };

            FirstCommand = new RelayCommand(FirstButton, param => this.canExecute);
            PreviousCommand = new RelayCommand(PreviousButton, param => this.canExecute);
            NextCommand = new RelayCommand(NextButton, param => this.canExecute);
            LastCommand = new RelayCommand(LastButton, param => this.canExecute);
        }

        public void FirstButton(object obj)
        {
            logger.Debug("FristButton");
        }

        public void PreviousButton(object obj)
        {
            logger.Debug("PreviousButton");
        }

        public void NextButton(object obj)
        {
            logger.Debug("NextButton");
        }

        public void LastButton(object obj)
        {
            logger.Debug("LastButton");
        }
    }
}
