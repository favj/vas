/*
* @(#)Logger.cs
*
* Copyright (c) 2014, VizitSolutions.
* All rights reserved.
*
* Use is subject to license terms. This software is protected by
* copyright law and international treaties. Unauthorized reproduction or
* distribution of this program, or any portion of it, may result in severe
* civil and criminal penalties, and will be prosecuted to the maximum extent.
*/

using System.Diagnostics;

namespace Com.VizApp.Arch.Logging
{
    public class Logger
    {
        public enum Level { OFF = 1, ERROR, INFO, DEBUG };
        private static Level currentLogLevel = Level.INFO; //Default Log level is INFO

        public static void SetLevel(Level logLevel)
        {
            currentLogLevel = logLevel;
        }

        /*
         * We get the calling method from the stackFrame
         */
        private static string GetCallingMethod(StackFrame frame)
        {
            //don't calculate this if we aren't logging
            if (!Microsoft.Practices.EnterpriseLibrary.Logging.Logger.IsLoggingEnabled()) return string.Empty;
            var method = frame.GetMethod();
            return method.DeclaringType.FullName + "." + method.Name;
        }

        /// <summary>
        /// Logs an informational message
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Info(string message)
        {
            if (currentLogLevel != Level.OFF)
            {
                //var category = GetCallingMethod(new StackFrame(1));
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, "INFO", 0, 0, TraceEventType.Information);
            }
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Debug(string message)
        {
            if (currentLogLevel != Level.OFF)
            {
                //var category = GetCallingMethod(new StackFrame(1));
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, "DEBUG", 0, 0, TraceEventType.Information);
            }
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Error(string message)
        {
            if (currentLogLevel != Level.OFF)
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, "ERROR", 0, 0, TraceEventType.Error);
            }
        }

        public static void Trace(string message)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message);
        }
    }
}
