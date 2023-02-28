using Kztek.Data;
using Kztek.Model.Models;
using System;
using System.Linq;

namespace Kztek.Web.Core.Functions
{
    public class FunctionAppHelper
    {
        public static void GetDateTimeOutStatus(string timeOutInEvent, string timeOutInCommand, int timeAlert, ref string result, ref string status)
        {
            var timeInEvent = Convert.ToDateTime(timeOutInEvent);
            var timeInCommand = Convert.ToDateTime(timeOutInCommand);

            var timeSpan = timeInEvent - timeInCommand;

            var t = timeSpan.TotalMinutes;
            if (t > 0)
            {
                if (t > timeAlert)
                {
                    result = t.ToString();

                    status = "2";
                }
                else
                {
                    result = t.ToString();

                    status = "3";
                }
            }
            else
            {
                var newT = -t;
                if (newT > timeAlert)
                {
                    result = newT.ToString();

                    status = "1";
                }
                else
                {
                    result = newT.ToString();

                    status = "3";
                }
            }
        }
    }
}
