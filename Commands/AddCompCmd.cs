using System.Linq;
using System.Text.RegularExpressions;
using agsXMPP.protocol.client;
using jrobbot.Configs;
using jrobbot.Core;

namespace jrobbot.Commands
{
    public class AddCompCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            if (!context.IsAuth() || !context.IsAdmin()) return false;
            var pp = GetCmdParts(msg);
            if (pp.Length <= 0 || pp[0] != "ADDCOMP") return false;
            if (pp.Length != 4)
            {
                JRobbot.Send(msg.From, "command must be: ADDCOMP name ip mac");
            } 
            else 
            {
                var fileName = CompCfgName.ConfigName();
                var computerList = fileName.LoadFromFile<ComputerList>();
                if (computerList.Any(x => x.Name.ToUpper() == pp[1].ToUpper()))
                {
                    JRobbot.Send(msg.From, "error: computer '{0}' already exists".Fmt(pp[1]));
                }
                else
                {
                    if (!Regex.IsMatch(pp[2],@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
                    {
                        JRobbot.Send(msg.From, "error: ip '{0}' must be in form 'N.N.N.N'".Fmt(pp[2]));
                    }
                    else if (!Regex.IsMatch(pp[3],@"^[a-zA-Z0-9]{12}$"))
                    {
                        JRobbot.Send(msg.From, "error: mac '{0}' must be in form '1a2b3c4d5e6f'".Fmt(pp[3]));
                    }
                    else
                    {
                        computerList.Add(new Computer(pp[1], pp[2], pp[3]));
                        computerList.SaveToFile(CompCfgName.ConfigName());
                        JRobbot.Send(msg.From, "computer '{0}' added to list".Fmt(pp[1]));
                    }
                }
            }
            return true;
        }
    }
}