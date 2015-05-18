using agsXMPP.protocol.client;
using jrobbot.Configs;
using jrobbot.Core;

namespace jrobbot.Commands
{
    public class ListUserCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            if (!context.IsAuth() || !context.IsAdmin()) return false;
            var pp = GetCmdParts(msg);
            if (pp.Length <= 0 || pp[0] != "LISTUSER") return false;
            var fileName = UserCfgName.ConfigName();
            var userList = fileName.LoadFromFile<UserList>();
            JRobbot.Send(msg.From, "Users are:");
            foreach (var ui in userList)
                JRobbot.Send(msg.From, "{0} *** {1} {2}".Fmt(ui.Login, ui.IsAdmin, ui.CompName));
            return true;
        }
    }
}