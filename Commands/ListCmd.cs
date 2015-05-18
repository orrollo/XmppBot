using System.Configuration;
using System.Net.NetworkInformation;
using agsXMPP.protocol.client;
using jrobbot.Configs;
using jrobbot.Core;

namespace jrobbot.Commands
{
    class ListCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            if (!context.IsAuth()) return false;
            if (msg.Body.ToLower() != "list") return false;

			var ping = new Ping();
			var fileName = CompCfgName.ConfigName();
	        var computerList = fileName.LoadFromFile<ComputerList>();
	        foreach (var comp in computerList)
	        {
				if (string.IsNullOrEmpty(comp.Ip)) continue;
				var pi = ping.Send(comp.Ip, 500);
		        var txt = string.Format("{0} ({1}): {2}\r\n", comp.Name, 
					comp.Ip, pi.Status == IPStatus.Success ? "online" : "offline");
		        JRobbot.Send(msg.From, txt);
	        }
            return true;
        }
    }
}