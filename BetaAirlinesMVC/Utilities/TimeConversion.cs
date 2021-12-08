using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetaAirlinesMVC.Utilities
{
    public class TimeConversion
    {
        public String GetDuration(int totalminutes)
        {
            string durationTime;
            int hours = Convert.ToInt32(Math.Floor((double)totalminutes / 60));
            if (hours > 0)
            {
                int minutes = totalminutes % 60;
                durationTime = hours + " hrs " + minutes + " min";
            } else
            {
                durationTime = totalminutes + " min";
            }
            return durationTime;
        }
    }
}