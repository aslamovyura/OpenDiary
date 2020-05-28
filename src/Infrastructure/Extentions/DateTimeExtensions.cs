using System;
using Application.Enums;

namespace Infrastructure.Extentions
{
    /// <summary>
    /// Extension methods for dateTime.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Calculate age of object.
        /// </summary>
        /// <param name="startDate">Start date.</param>
        /// <returns>Age and units.</returns>
        public static (int age, AgeUnits units) Age(this DateTime startDate)
        {
            var zeroTime = new DateTime(1, 1, 1);
            var span = zeroTime + (DateTime.Now - startDate);

            if (span.Year - 1 > 0)
                return (span.Year, AgeUnits.Year);

            else if (span.Month - 1 > 0)
                return (span.Month, AgeUnits.Month);

            else if (span.Day - 1 > 7)
                return (span.Day%7, AgeUnits.Week);

            else if (span.Day - 1 > 0)
                return (span.Day, AgeUnits.Day);

            else if (span.Hour - 1 > 0)
                return (span.Hour, AgeUnits.Hour);

            else if (span.Minute - 1 > 0)
                return (span.Minute, AgeUnits.Minute);

            else
                return (span.Second, AgeUnits.Second);
        }

        /// <summary>
        /// Calculate age of object in certain units.
        /// </summary>
        /// <param name="startDate">Start date.</param>
        /// <param name="units">Units of age measurement.</param>
        /// <returns>Age in specific units.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int Age(this DateTime startDate, AgeUnits units)
        {
            var zeroTime = new DateTime(1, 1, 1);
            if(DateTime.Now < startDate)
            {
                throw new ArgumentOutOfRangeException(nameof(startDate));
            }
            var span = zeroTime + (DateTime.Now - startDate);

            switch(units)
            {
                case AgeUnits.Year:
                    return span.Year - 1;

                case AgeUnits.Month:
                    return span.Month - 1;

                case AgeUnits.Week:
                    return span.Day%7 - 1;

                case AgeUnits.Day:
                    return span.Day - 1;

                case AgeUnits.Hour:
                    return span.Hour - 1;

                case AgeUnits.Minute:
                    return span.Minute - 1;

                case AgeUnits.Second:
                    return span.Second - 1;

                default:
                    return span.Year - 1;
            }
        }
    }
}