using System;
using System.Globalization;

namespace PDADesktop.Classes.Utils
{
    public class DateTimeUtils
    {
        public static bool IsGreatherThanToday(string synchronizationDefault)
        {
            bool isGreather = false;
            if(synchronizationDefault != null && synchronizationDefault.Equals("00000000000000"))
            {
                return true;
            }
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

        public static Int32 GetUnixTimeFromUTCNow()
        {
            DateTime unixTime = new DateTime(1970, 1, 1);
            DateTime datetimeNow = DateTime.UtcNow;
            Int32 unixTimestamp = (Int32)(datetimeNow.Subtract(unixTime)).TotalSeconds;
            return unixTimestamp;
        }
    }
}
