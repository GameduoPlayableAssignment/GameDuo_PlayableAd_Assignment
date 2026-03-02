using System;

namespace Helper.Time
{
    public partial class TimeHelper
    {
#if UNITY_EDITOR
        private static readonly bool DEBUG_ONE_DAY_IS_ONE_MINUTE = !true;
#else
        private const bool DEBUG_ONE_DAY_IS_ONE_MINUTE = false;
#endif
        const string EXPIRATION_TIME_D = "expiration_time_d";
        const string EXPIRATION_TIME_H = "expiration_time_h";
        const string EXPIRATION_TIME_M = "expiration_time_m";

        const string TIME_D = "time_d";
        const string TIME_H = "time_h";
        const string TIME_M = "time_m";
        const string TIME_S = "time_s";

        private const string TIME_DOUBLE_DIGIT = "time_double_digit";
        
        private static Tuple<int, int, int, int> _GetSplitTime(int daysAndHoursAndMinutesAndSeconds)
        {
            int days = daysAndHoursAndMinutesAndSeconds / (60 * 60 * 24);
            int hoursAndMinutesAndSeconds = daysAndHoursAndMinutesAndSeconds - (days * 60 * 60 * 24);
            int hours = hoursAndMinutesAndSeconds / (60 * 60);
            int minutesAndSeconds = hoursAndMinutesAndSeconds - (hours * 60 * 60);
            int minutes = minutesAndSeconds / 60;
            int seconds = minutesAndSeconds - (minutes * 60);
            return new Tuple<int, int, int, int>(days, hours, minutes, seconds);
        }
        
        public static long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }
    }
}