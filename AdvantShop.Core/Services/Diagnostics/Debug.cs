//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;

namespace AdvantShop.Diagnostics
{
    /// <summary>
    /// Logger - shell for log4net logger
    /// </summary>
    public enum ErrType
    {
        None,
        Err500,
        ErrHttp,
        Info
    }

    public class Debug
    {
        public static readonly string ErrFilesPath = SettingsGeneral.AbsolutePath + "App_Data\\errlog\\";


        public static string GetErrFileName(ErrType type)
        {
            return ErrFilesPath + type + ".csv";

        }

        private static readonly ILog log = LogManager.GetLogger(typeof(Debug));

        public const string CharSeparate = ";";

        public static void InitLogger()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(SettingsGeneral.AbsolutePath + "log4net.config"));
        }

        public static ILog Log
        {
            get
            {
                return log;
            }
        }

        [Obsolete("LogError is deprecated, use Log class instead.")]
        public static void LogError(Exception exception, string message)
        {
            log.Error(message, exception);
        }

        [Obsolete("LogError is deprecated, use Log class instead.")]
        public static void LogError(string message)
        {
            log.Error(message, new Exception(message));
        }

        [Obsolete("LogError is deprecated, use Log class instead.")]
        public static void LogError(string message, bool flag = true)
        {
            log.Error(message, new Exception(message));
        }

        [Obsolete("LogError is deprecated, use Log class instead.")]
        public static void LogError(Exception exception, bool flag = true)
        {
            log.Error(string.Empty, exception);
        }
    }
}
