using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDADesktop.Classes.Utils
{
    public class DateTimeUtils
    {
        public static bool IsGreatherThanToday(string synchronizationDefault)
        {
            bool isGreather = false;
            string synchroFormat = "yyyyMMddHHmmss";
            DateTime synchronizationDT = DateTime.ParseExact(synchronizationDefault, synchroFormat, CultureInfo.InvariantCulture);
            int numberDaySynchronizationDT = synchronizationDT.Day;
            DateTime nowDT = DateTime.Now;
            int numberDayNowDT = nowDT.Day;

            if(numberDayNowDT - numberDaySynchronizationDT == 0)
            {
                // Es el mismo dia
                isGreather = false;
            }
            else
            {
                //Es de un dia anterior
                isGreather = true;
            }
            return isGreather;
        }
    }
}
