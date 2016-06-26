// ***********************************************************************
// Assembly         : Portal.Core
// Author           : Administrator
// Created          : 12-23-2013
//
// Last Modified By : Administrator
// Last Modified On : 12-24-2013
// ***********************************************************************
// <copyright file="Logger.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Portal.MVC
{
    /// <summary>
    /// Class Logger
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly NLog.Logger logger = NLog.LogManager.GetLogger("Logger");


        /// <summary>
        /// Traces the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Trace(string message)
        {
            logger.Trace(message);
        }


        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Debug(string message)
        {
            logger.Debug(message);
        }

        /// <summary>
        /// Infoes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Info(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Warn(string message)
        {
            logger.Warn(message);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Error(string message)
        {
            logger.Error(message);
        }

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Fatal(string message)
        {
            logger.Fatal(message);
        }
    }
}