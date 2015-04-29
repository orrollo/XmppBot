using System;
using System.Diagnostics;

namespace jrobbot
{
    public static class CmdHelper
    {
        public static string Fmt(this string msg, params object[] args)
        {
            if (args != null && args.Length > 0) msg = string.Format(msg, args);
            return msg;
        }

        public static void Info(this string msg, params object[] args)
        {
            if (args != null && args.Length > 0) msg = string.Format(msg, args);
            Debug.WriteLine(msg);
            if (Environment.UserInteractive) Console.WriteLine(msg);
        }

        public static bool IsAuth(this Context ctx)
        {
            return ctx.GetAs<bool>("isAuth", false);
        }

        public static bool IsAdmin(this Context ctx)
        {
            return ctx.GetAs<int>("level", 0) == 2;
        }
    }
}