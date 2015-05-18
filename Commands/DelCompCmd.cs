using System.Linq;
using agsXMPP.protocol.client;
using jrobbot.Configs;
using jrobbot.Core;

namespace jrobbot.Commands
{
    public class DelCompCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            if (!context.IsAuth() || !context.IsAdmin()) return false;
            var pp = GetCmdParts(msg);
            if (pp.Length <= 0 || pp[0] != "DELCOMP") return false;
            if (pp.Length != 2)
            {
                JRobbot.Send(msg.From, "command must be: DELCOMP name");
            }
            else
            {
                var fileName = CompCfgName.ConfigName();
                var computerList = fileName.LoadFromFile<ComputerList>();
                var ci = computerList.FirstOrDefault(x => x.Name.ToUpper() == pp[1].ToUpper());
                if (ci == null)
                {
                    JRobbot.Send(msg.From, "error: computer '{0}' not found".Fmt(pp[1]));
                }
                else
                {
                    computerList.Remove(ci);
                    computerList.SaveToFile(CompCfgName.ConfigName());
                    JRobbot.Send(msg.From, "computer '{0}' removed".Fmt(pp[1]));
                }
            }
            return true;
        }
    }
}