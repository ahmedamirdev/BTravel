using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.BL.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime ConvertUtcToCairoTime(this DateTime UtcTime)
        {
            return UtcTime.AddHours(3);
        }

        public static TimeOnly ConvertUtcToCairoTime(this TimeOnly UtcTime)
        {
            return UtcTime.AddHours(3);
        }
    }
}
