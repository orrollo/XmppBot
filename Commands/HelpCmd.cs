using agsXMPP.protocol.client;
using jrobbot.Core;
using System.Text;

namespace jrobbot.Commands
{
    class HelpCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            var pp = GetCmdParts(msg);
            if (pp.Length == 0 || pp[0] != "HELP") return false;
            var sb = new StringBuilder();
            sb.AppendLine("Availabale commands:");
            sb.AppendLine("HELP - this text");
            if (!context.IsAuth())
            {
                sb.AppendLine("USER login password - login to system");
            }
            else
            {
                sb.AppendLine("LIST - show list of online computers");
                sb.AppendLine("UP - wake up computer, connected to user");
                sb.AppendLine("QUIT - logout from system");
                if (context.IsAdmin())
                {
                    sb.AppendLine("");
                    sb.AppendLine("admin commands:");
                    sb.AppendLine("");
                    sb.AppendLine("LISTCOMP - show list of computers");
                    sb.AppendLine("ADDCOMP name ip mac - add computer to list");
                    sb.AppendLine("DELCOMP name - remove computer from list");
                    sb.AppendLine("UPDCOMP name (NAME|IP|MAC) value - update computer data");
                    sb.AppendLine("");
                    sb.AppendLine("LISTUSER - show list of users");
                    sb.AppendLine("ADDUSER login password admin comp - add user to list");
                    sb.AppendLine("DELUSER login - remove user from list");
                    sb.AppendLine("UPDUSER login (LOGIN|PASSWORD|ISADMIN|COMP) value - update user data");
                }
            }
            JRobbot.Send(conn, msg.From, sb.ToString());
            return true;
        }
    }
}