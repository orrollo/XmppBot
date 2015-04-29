using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace jrobbot
{
    partial class JRobbotService : ServiceBase
    {
        public JRobbotService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            JRobbot.Start();
        }

        protected override void OnStop()
        {
            JRobbot.Stop();
        }
    }
}
