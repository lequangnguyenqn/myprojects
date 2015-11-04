using System;
using System.Globalization;

namespace WeddingInvitation.Infrastructure.Unity
{
    /// <summary>
    /// Custom Formatter for providing suffix text for day numbers 
    /// (e.g. "1st", "22nd", "14th", etc.)
    /// </summary>
    public sealed class DayNumberFormatInfo : IFormatProvider, ICustomFormatter
    {
        #region IFormatProvider Members
        object IFormatProvider.GetFormat(Type formatType)
        {
            if (typeof(ICustomFormatter).Equals(formatType))
                return this;
            else
                return null;
        }

        #endregion

        #region ICustomFormatter Members
        string ICustomFormatter.Format(string format, object arg, IFormatProvider formatProvider)
        {
            // If this is a DateTime with a supported format string...

            if (!string.IsNullOrEmpty(format) && arg is DateTime)
            {
                DateTime dateTime = (DateTime)arg;
                string formatString = format.Trim();

                if (formatString.Equals("Dx", StringComparison.Ordinal))
                    return string.Format(formatProvider, "{1} {0:MMMM} {0:yyyy}", dateTime, FormatDayNumberSuffix(dateTime));

                else if (formatString.Equals("Mx", StringComparison.OrdinalIgnoreCase))
                    return string.Format(formatProvider, "{1} {0:MMMM}", dateTime, FormatDayNumberSuffix(dateTime));

                else if (formatString.Equals("fx", StringComparison.Ordinal))
                    return string.Format(formatProvider, "{1} {0:MMMM} {0:yyyy} {0:t}", dateTime, FormatDayNumberSuffix(dateTime));

                else if (formatString.Equals("Fx", StringComparison.Ordinal))
                    return string.Format(formatProvider, "{1} {0:MMMM} {0:yyyy} {0:T}", dateTime, FormatDayNumberSuffix(dateTime));

                else if (formatString.Equals("ddx", StringComparison.Ordinal))
                    return FormatDayNumberSuffix(dateTime);

                else if (formatString.Equals("x", StringComparison.Ordinal))
                    return GetDayNumberSuffix(dateTime);

                else if (formatString.Equals("Rx", StringComparison.OrdinalIgnoreCase))
                    return string.Format(formatProvider, "{0:ddd}, {1} {0:MMM} {0:yyyy} {0:T} GMT", dateTime, FormatDayNumberSuffix(dateTime));

                else if (formatString.Equals("Ux", StringComparison.Ordinal))
                    return string.Format(formatProvider, "{1} {0:MMMM} {0:yyyy} {0:T}", dateTime, FormatDayNumberSuffix(dateTime));
            }

            // If it's not a DateTime or has an unsupported format...

            if (arg is IFormattable)
                return ((IFormattable)arg).ToString(format, formatProvider);
            else
                return arg.ToString();
        }

        #endregion

        #region Helpers
        /// <summary>
        /// Returns day number with two character suffix text for the 
        /// day number component in the given date value
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>String of day number including suffix (1st, 2nd, 3rd, 4th etc.)</returns>
        private static string FormatDayNumberSuffix(DateTime date)
        {
            if (date == null)
                throw new ArgumentNullException("date");

            return string.Format(CultureInfo.InvariantCulture, "{0:dd}{1}", date, GetDayNumberSuffix(date));
        }

        /// <summary>
        /// Returns just the two character suffix text for the 
        /// day number component in the given date value
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>String of day number suffix (st, nd, rd or th)</returns>
        private static string GetDayNumberSuffix(DateTime date)
        {
            if (date == null)
                throw new ArgumentNullException("date");

            int day = date.Day;

            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";

                case 2:
                case 22:
                    return "nd";

                case 3:
                case 23:
                    return "rd";

                default:
                    return "th";
            }
        }

        public static string FormatDate(DateTime date)
        {
            if (date == null)
                throw new ArgumentNullException("date");

            return string.Format(CultureInfo.InvariantCulture, "{0:HH:mm dddd dd}{1} {2:MMMM, yyyy}", date, GetDayNumberSuffix(date), date);
        }

        #endregion

    }
}
