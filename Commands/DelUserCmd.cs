using System.Linq;
using agsXMPP.protocol.client;
using jrobbot.Configs;
using jrobbot.Core;

namespace jrobbot.Commands
{
    public class DelUserCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            if (!context.IsAuth() || !context.IsAdmin()) return false;
            var pp = GetCmdParts(msg);
            if (pp.Length <= 0 || pp[0] != "DELUSER") return false;
            if (pp.Length != 2)
            {
                JRobbot.Send(msg.From, "command must be: DELUSER login");
            }
            else
            {
                var fileName = UserCfgName.ConfigName();
                var userList = fileName.LoadFromFile<UserList>();
                var ui = userList.FirstOrDefault(x => x.Login.ToUpper() == pp[1].ToUpper());
                if (ui == null)
                {
                    JRobbot.Send(msg.From, "error: user '{0}' not found".Fmt(pp[1]));
                }
                else
                {
                    userList.Remove(ui);
                    userList.SaveToFile(UserCfgName.ConfigName());
                    JRobbot.Send(msg.From, "user '{0}' removed".Fmt(pp[1]));
                }
            }
            return true;
        }
    }
}