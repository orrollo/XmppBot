using System.Linq;
using agsXMPP.protocol.client;
using jrobbot.Configs;
using jrobbot.Core;

namespace jrobbot.Commands
{
    public class AddUserCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            if (!context.IsAuth() || !context.IsAdmin()) return false;
            var pp = GetCmdParts(msg);
            if (pp.Length <= 0 || pp[0] != "ADDUSER") return false;
            if (pp.Length != 5)
            {
                JRobbot.Send(msg.From, "command must be: ADDUSER login password isadmin comp");
            }
            else
            {
                var fileName = UserCfgName.ConfigName();
                var userList = fileName.LoadFromFile<UserList>();
                if (userList.Any(x => x.Login.ToUpper() == pp[1].ToUpper()))
                {
                    JRobbot.Send(msg.From, "error: user '{0}' already exists".Fmt(pp[1]));
                }
                else
                {
                    bool isAdmin;
                    var computerList = CompCfgName.ConfigName().LoadFromFile<ComputerList>();
                    if (!bool.TryParse(pp[3], out isAdmin))
                    {
                        JRobbot.Send(msg.From, "error: isadmin must be true or false");
                    }
                    else if (computerList.All(x=>x.Name.ToUpper()!=pp[4].ToUpper()))
                    {
                        JRobbot.Send(msg.From, "error: comp '{0}' not found".Fmt(pp[4]));
                    } 
                    else 
                    {
                        userList.Add(new User(pp[1], pp[2], isAdmin, pp[4]));
                        userList.SaveToFile(UserCfgName.ConfigName());
                        JRobbot.Send(msg.From, "user '{0}' added to list".Fmt(pp[1]));
                    }
                }
            }
            return true;
        }
    }
}