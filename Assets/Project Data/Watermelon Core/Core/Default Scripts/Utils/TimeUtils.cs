using System;

namespace Watermelon
{
    public static class TimeUtils
    {
        private static readonly DateTime DEFAULT_DATE_UNIX = new DateTime(1970, 1, 1, 0, 0, 0);

        public const string FORMAT_FULL = "d/M/yyyy HH:mm:ss";
        public const string FORMAT_SHORT = "t";

        /// <summary>
        /// Get current Unix time
        /// </summary>
        public static double GetCurrentUnixTimestamp()
        {
            double unixTimestamp = (DateTime.Now - DEFAULT_DATE_UNIX).TotalSeconds;

            return unixTimestamp;
        }

        /// <summary>
        /// Get current Unix time
        /// </summary>
        public static double GetCurrentUnixTimestampWithExtraTime(int weeks = 0, int days = 0, int hours = 0, int minutes = 0, int seconds = 0)
        {
            DateTime nowDate = DateTime.Now;
            nowDate = nowDate.AddDays(weeks * 7);
            nowDate = nowDate.AddDays(days);
            nowDate = nowDate.AddHours(hours);
            nowDate = nowDate.AddMinutes(minutes);
            nowDate = nowDate.AddSeconds(seconds);

            double unixTimestamp = (nowDate - DEFAULT_DATE_UNIX).TotalSeconds;

            return unixTimestamp;
        }

        /// <summary>
        /// Get current Unix time
        /// </summary>
        public static double GetCurrentDayUnixTimestamp()
        {
            DateTime nowDate = DateTime.Now;
            nowDate = nowDate.AddHours(-nowDate.Hour);
            nowDate = nowDate.AddMinutes(-nowDate.Minute);
            nowDate = nowDate.AddSeconds(-nowDate.Second);

            double unixTimestamp = (nowDate - DEFAULT_DATE_UNIX).TotalSeconds;

            return unixTimestamp;
        }

        /// <summary>
        /// Convert DateTime to Unix time
        /// </summary>
        public static double GetUnixTimestampFromDateTime(DateTime target)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);

            double unixTimestamp = (target - date).TotalSeconds;

            return unixTimestamp;
        }

        /// <summary>
        /// Convert Unix time to DateTime
        /// </summary>
        public static DateTime GetDateTimeFromUnixTime(double unixTime)
        {
            return DEFAULT_DATE_UNIX.AddSeconds(unixTime);
        }

        /// <summary>
        ///  Get current date
        /// </summary>
        public static string GetCurrentDateString(string format)
        {
            return DateTime.Now.ToString(format);
        }
    }
}