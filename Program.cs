﻿using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Xml.Serialization;
using NLog;
using NLog.Targets;
using NLog.Config;
using jrobbot.Commands;
using jrobbot.Configs;
using jrobbot.Core;
using jrobbot.Service;

namespace jrobbot
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

	    static void Main(string[] args)
	    {
            ConfigureLogs(Environment.UserInteractive);
		    CreateConfigs();

		    AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            if (Environment.UserInteractive)
            {
                var arg = args != null && args.Length > 0 ? args[0].ToLower() : "";
                if (arg == "-i")
                {
                    Console.WriteLine("installing service...");
                    var param = new[] { Assembly.GetExecutingAssembly().Location };
                    ManagedInstallerClass.InstallHelper(param);
                }
                else if (arg == "-u")
                {
                    Console.WriteLine("uninstalling service...");
                    var param = new[] { "/u", Assembly.GetExecutingAssembly().Location };
                    ManagedInstallerClass.InstallHelper(param);
                }
                else
                {
                    AllocConsole();
                    JRobbot.Start();
                    Console.Write("Press enter to stop server...");
                    Console.ReadLine();
                    JRobbot.Stop();
                }
            }
            else
            {
                using (var service = new JRobbotService())
                {
                    ServiceBase.Run(service);
                }
            }

        }

        private static void ConfigureLogs(bool isConsole)
        {
            var config = new LoggingConfiguration();
            
            // file logs
            var fileTarget = new FileTarget();
            fileTarget.FileName = "${basedir}/logs/${shortdate}.log";
            fileTarget.Layout = "${date} ${logger} ${message}";
            config.AddTarget("file", fileTarget);
            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            // console logs
            if (isConsole)
            {
                var consoleTarget = new ColoredConsoleTarget();
                consoleTarget.Layout = @"${date:format=HH\\:MM\\:ss} ${logger} ${message}";
                config.AddTarget("console", consoleTarget);
                var rule1 = new LoggingRule("*", LogLevel.Info, consoleTarget);
                config.LoggingRules.Add(rule1);
            }

            LogManager.Configuration = config;
        }

        private static void CreateConfigs()
	    {
		    CreateCompConfig();
		    CreateUserConfig();
	    }

	    private static void CreateCompConfig()
	    {
			var comps = BaseCmd.CompCfgName.ConfigName().LoadFromFile<ComputerList>();
		    if (comps.Count == 0)
		    {
			    comps.Add(new Computer("test", "192.168.0.1", "1a2b3c4d5e6f"));
			    comps.SaveToFile(BaseCmd.CompCfgName.ConfigName());
		    }
	    }

		private static void CreateUserConfig()
		{
			var users = BaseCmd.UserCfgName.ConfigName().LoadFromFile<UserList>();
			if (users.Count == 0)
			{
				users.Add(new User("test", "test", false, "test"));
				users.SaveToFile(BaseCmd.UserCfgName.ConfigName());
			}
		}

		private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Error("unhandled exception, global trapper:" + e.ExceptionObject.ToString());
            //var error = e.ExceptionObject.ToString();
            //error.Info();
            //string errorLog = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".log");
            //File.AppendText(errorLog);
        }
    }
}
