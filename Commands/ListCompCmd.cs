using agsXMPP.protocol.client;
using jrobbot.Configs;
using jrobbot.Core;

namespace jrobbot.Commands
{
    public class ListCompCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            if (!context.IsAuth() || !context.IsAdmin()) return false;
            var pp = GetCmdParts(msg);
            if (pp.Length <= 0 || pp[0] != "LISTCOMP") return false;
            var fileName = CompCfgName.ConfigName();
            var computerList = fileName.LoadFromFile<ComputerList>();
            JRobbot.Send(msg.From, "Computers are:");
            foreach (var ci in computerList) 
                JRobbot.Send(msg.From, "{0} {1} {2}".Fmt(ci.Name, ci.Ip, ci.Mac));
            return true;
        }
    }
}