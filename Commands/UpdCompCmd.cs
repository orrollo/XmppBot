using System.Linq;
using agsXMPP.protocol.client;
using jrobbot.Configs;
using jrobbot.Core;

namespace jrobbot.Commands
{
    public class UpdCompCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            if (!context.IsAuth() || !context.IsAdmin()) return false;
            var pp = GetCmdParts(msg);
            if (pp.Length == 0 || pp[0] != "UPDCOMP") return false;
            var ok = pp.Length == 4;
            if (ok)
            {
                pp[2] = pp[2].ToUpper();
                ok = pp[2] == "IP" || pp[2] == "NAME" || pp[2] == "MAC";
            }
            if (!ok)
            {
                JRobbot.Send(msg.From, "command must be: UPDCOMP name (NAME|IP|MAC) value");
                return true;
            }

            var fileName = CompCfgName.ConfigName();
            var computerList = fileName.LoadFromFile<ComputerList>();
            var ci = computerList.FirstOrDefault(x => x.Name.ToUpper() == pp[1].ToUpper());

            if (ci == null)
            {
                JRobbot.Send(msg.From, "error: computer '{0}' not found".Fmt(pp[1]));
                return true;
            }

            if (pp[2] == "IP") ci.Ip = pp[3];
            if (pp[2] == "MAC") ci.Mac = pp[3];
            if (pp[2] == "NAME") ci.Name = pp[3];

            computerList.SaveToFile(CompCfgName.ConfigName());
            JRobbot.Send(msg.From, "computer '{0}' updated".Fmt(pp[1]));
            return true;
        }
    }
}