using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using jrobbot.Core;

namespace jrobbot.Commands
{
    public static class CmdHelper
    {
		public static string ConfigName(this string extName)
		{
			var fullname = Assembly.GetExecutingAssembly().Location;
			return Path.ChangeExtension(fullname, extName);
		}

        public static string Fmt(this string msg, params object[] args)
        {
            if (args != null && args.Length > 0) msg = string.Format(msg, args);
            return msg;
        }

        //public static void Info(this string msg, params object[] args)
        //{
        //    if (args != null && args.Length > 0) msg = string.Format(msg, args);
        //    Debug.WriteLine(msg);
        //    //if (Environment.UserInteractive) Console.WriteLine(msg);
        //    Log.Info(msg);
        //}

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