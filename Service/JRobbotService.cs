using System.ServiceProcess;
using jrobbot.Core;

namespace jrobbot.Service
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
