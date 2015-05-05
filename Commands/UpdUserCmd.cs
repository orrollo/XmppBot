using System.Linq;
using agsXMPP.protocol.client;
using jrobbot.Configs;
using jrobbot.Core;

namespace jrobbot.Commands
{
    public class UpdUserCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            if (!context.IsAuth() || !context.IsAdmin()) return false;
            var pp = GetCmdParts(msg);
            if (pp.Length == 0 || pp[0] != "UPDUSER") return false;
            var ok = pp.Length != 4;
            if (ok)
            {
                pp[2] = pp[2].ToUpper();
                ok = pp[2] == "LOGIN" || pp[2] == "PASSWORD" || pp[2] == "ISADMIN" || pp[2] == "COMP";
            }
            if (!ok)
            {
                JRobbot.Send(conn, msg.From, "command must be: UPDUSER login (LOGIN|PASSWORD|ISADMIN|COMP) value");
                return true;
            }

            var fileName = UserCfgName.ConfigName();
            var userList = fileName.LoadFromFile<UserList>();
            var ui = userList.FirstOrDefault(x => x.Login.ToUpper() == pp[1].ToUpper());

            if (ui == null)
            {
                JRobbot.Send(conn, msg.From, "error: user '{0}' not found".Fmt(pp[1]));
                return true;
            }

            if (pp[2] == "LOGIN") ui.Login = pp[3];
            if (pp[2] == "PASSWORD") ui.Password = pp[3];
            if (pp[2] == "ISADMIN")
            {
                bool isAdmin;
                if (bool.TryParse(pp[3], out isAdmin))
                {
                    JRobbot.Send(conn, msg.From, "error: value '{0}' is not boolean".Fmt(pp[3]));
                    return true;
                }
                ui.IsAdmin = isAdmin;
            }
            if (pp[2] == "COMP") ui.CompName = pp[3];

            userList.SaveToFile(UserCfgName.ConfigName());
            JRobbot.Send(conn, msg.From, "user '{0}' updated".Fmt(pp[1]));
            return true;
        }
    }
}