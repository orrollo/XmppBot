using System.Configuration;
using System.Net.NetworkInformation;
using agsXMPP.protocol.client;

namespace jrobbot
{
    class ListCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            if (!ctx.IsAuth()) return false;
            if (msg.Body.ToLower() != "list") return false;

            // обходим все компьютеры конфигурации и пингуем
            var config = ConfigurationManager.GetSection("CompInfo") as CompInfoSection;
            var ping = new System.Net.NetworkInformation.Ping();
            foreach (CompInfo ci in config.CompInfo)
            {
                if (string.IsNullOrEmpty(ci.Ip)) continue;
                var pi = ping.Send(ci.Ip, 500);
                JRobbot.Send(conn, msg.From, string.Format("{0} ({1})... {2}\r\n", ci.Name, ci.Ip, pi.Status==IPStatus.Success ? "on" : "off"));
            }
            return true;
        }
    }
}