using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLog.Targets;
using NLog.Config;

namespace jrobbot.Core
{
    public class Log
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Info(string msg, params object[] args)
        {
            if (args != null && args.Length > 0) msg = string.Format(msg, args);
            Logger.Info(msg);
        }

        public static void Debug(string msg, params object[] args)
        {
            if (args != null && args.Length > 0) msg = string.Format(msg, args);
            Logger.Debug(msg);
        }

        public static void Error(string msg, params object[] args)
        {
            if (args != null && args.Length > 0) msg = string.Format(msg, args);
            Logger.Error(msg);
        }

        public static void Trace(string msg, params object[] args)
        {
            if (args != null && args.Length > 0) msg = string.Format(msg, args);
            Logger.Trace(msg);
        }

        public static void Fatal(string msg, params object[] args)
        {
            if (args != null && args.Length > 0) msg = string.Format(msg, args);
            Logger.Fatal(msg);
        }
    }
}
