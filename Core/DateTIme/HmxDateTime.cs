using System;
using System.Globalization;

namespace HmxLabs.Core.DateTIme
{
    /// <summary>
    /// Utility extension methods on the DateTime class
    /// </summary>
    public static class HmxDateTime
    {
        /// <summary>
        /// The string required to genreate an ISO datetime output when calling ToString() on a DateTime object
        /// </summary>
        public const string IsoDateTimeFormatString = "yyyy-MM-ddTHH:mm:ss";

        /// <summary>
        /// The string required to genreate an ISO date only when calling ToString() on a DateTime object
        /// </summary>
        public const string IsoDateFormatString = "yyyy-MM-dd";

        /// <summary>
        /// The string require to generate European style medium date output (e.g 25 Jan 2017) when calling ToString() on a DateTime object
        /// </summary>
        public const string ExplicitDateDisplayString = "dd MMM yyyy";

        /// <summary>
        /// Utility method to genrate an ISO datetime string
        /// </summary>
        /// <param name="datetime_">The DateTime object to operate on</param>
        /// <returns></returns>
        public static string ConvertToIsoDateTimeString(DateTime datetime_)
        {
            return datetime_.ToString(IsoDateTimeFormatString);
        }

        /// <summary>
        /// Utility method to generate and ISO date string
        /// </summary>
        /// <param name="datetime_">The DateTime object to operate on</param>
        /// <returns></returns>
        public static string ConvertToIsoDateString(DateTime datetime_)
        {
            return datetime_.ToString(IsoDateFormatString);
        }

        /// <summary>
        /// Utililty method to parse a string containg an ISO datetime representation into a DateTime object
        /// </summary>
        /// <param name="isoDatetimeStr_">The string containing the ISO DateTime represetation</param>
        /// <returns></returns>
        public static DateTime ParseIsoDateTimeString(string isoDatetimeStr_)
        {
            return DateTime.Parse(isoDatetimeStr_, null, DateTimeStyles.AllowWhiteSpaces);
        }

        /// <summary>
        /// Utility method to try and parse a string containing an ISO datetime string into a DateTime object
        /// </summary>
        /// <param name="isoDateTimeStr_">The string containing the ISO DateTime representation</param>
        /// <param name="result_">The object where the value should be set if the parse is successful</param>
        /// <returns></returns>
        public static bool TryParseIsoDateTimeString(string isoDateTimeStr_, out DateTime result_)
        {
            return DateTime.TryParse(isoDateTimeStr_, null, DateTimeStyles.AllowWhiteSpaces, out result_);
        }

        /// <summary>
        /// Extension method to generate an ISO date only output
        /// </summary>
        /// <param name="datetime_">The DateTime object to operate on</param>
        /// <returns></returns>
        public static string ToIsoDateString(this DateTime datetime_)
        {
            return ConvertToIsoDateString(datetime_);
        }

        /// <summary>
        /// Extension method to generate an ISO DateTime string
        /// </summary>
        /// <param name="datetime_">The DateTime object to operate on</param>
        /// <returns></returns>
        public static string ToIsoDateTimeString(this DateTime datetime_)
        {
            return ConvertToIsoDateTimeString(datetime_);
        }

        /// <summary>
        /// Extension method to genreate an medium format date string
        /// </summary>
        /// <param name="date_">The DateTime obejct to operate on</param>
        /// <returns></returns>
        public static string ToExplicitDateDisplayString(this DateTime date_)
        {
            return date_.ToString(ExplicitDateDisplayString);
        }
    }
}
