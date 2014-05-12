/*
* @(#)DateUtil.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using System;

namespace Com.VizApp.Arch.Util
{
    public class DateUtil
    {
        private const string dateFormat = "yyyy-MM-ddTHH:mm:ss";
        public static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        public static DateTime GetFormattedCurrentTime()
        {
            return DateTime.ParseExact(DateTime.Now.ToString(dateFormat), dateFormat, null);
        }

        public static DateTime GetFormattedDate(DateTime dateTime)
        {
            return DateTime.ParseExact(dateTime.ToString(dateFormat), dateFormat, null);
        }

        public static string GetTotalHoursWithMinutes(DateTime datetime)
        {
            var dtSpan = DateTimeSpan.CompareDates(datetime, DateTime.Now);            
            string totalHours = string.Empty;
            string totalDays = string.Empty;
            string totalMonths = string.Empty;

            int months = dtSpan.Years < 1 ? dtSpan.Months : (dtSpan.Years * 12) + dtSpan.Months;
            
            if (months > 0)
                totalMonths = months == 1 ? months + " month " : months + " months ";

            if (dtSpan.Days >= 0)
                totalDays = dtSpan.Days <= 1 && !string.IsNullOrEmpty(totalMonths) ? dtSpan.Days + " day " : dtSpan.Days != 0 ? dtSpan.Days + " days " : string.Empty;

            if (dtSpan.Hours >= 0)
                totalHours = dtSpan.Hours <= 1 && !string.IsNullOrEmpty(totalDays) ? dtSpan.Hours + " hr " : dtSpan.Hours != 0 ? dtSpan.Hours + " hrs " : string.Empty;

            string totalMinutes = dtSpan.Minutes <= 1 ? dtSpan.Minutes + " min" : dtSpan.Minutes + " mins";
            
            return totalMonths + totalDays + totalHours + totalMinutes;
        }

        public static string GetHoursWithMinutes(DateTime datetime)
        {
            return datetime.ToString("HH:mm");
        }
    }
}
