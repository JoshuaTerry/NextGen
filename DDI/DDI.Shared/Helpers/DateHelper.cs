using System;

namespace DDI.Shared.Helpers
{
    public static class DateHelper
    {
        /// <summary>
        /// Calculate the nearest valid date for a given month, day, year.  Day can be less than 1, and month can be any value.
        /// </summary>
        public static DateTime GetNearestValidDate(int month, int day, int year, bool backwards)
        {
            DateTime dt = DateTime.MinValue;

            // Adjust for day < 1
            if (day < 1)
            {
                day = 31;
                month--;
            }

            // Adjust for month outside of 1-12 range.
            while (month < 1)
            {
                month += 12;
                year--;
            }

            while (month > 12)
            {
                month -= 12;
                year++;
            }

            // Try to create DateTime value, moving backwards or forwards until successful.
            while (true)
            {
                try
                {
                    dt = new DateTime(year, month, day);
                    break;
                }
                catch
                {
                    if (backwards)
                    {
                        day--;
                        if (day < 1)
                        {
                            month--;
                            day = 31;
                        }
                        if (month < 1)
                        {
                            month = 12;
                            year--;
                        }

                    }
                    else
                    {
                        day++;
                        if (day > 31)
                        {
                            day = 1;
                            month++;
                        }
                        if (month > 12)
                        {
                            month = 1;
                            year++;
                        }
                    }
                }

            }
            return dt;
        }


    }
}
