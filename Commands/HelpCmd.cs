using agsXMPP.protocol.client;
using jrobbot.Core;

namespace jrobbot.Commands
{
    class HelpCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            var pp = GetCmdParts(msg);
            if (pp.Length == 0 || pp[0] != "HELP") return false;
            JRobbot.Send(conn, msg.From, "Availabale commands:");
            JRobbot.Send(conn, msg.From, "HELP - this text");
            if (!context.IsAuth())
            {
                JRobbot.Send(conn, msg.From, "USER login password - login to system");
            }
            else
            {
                JRobbot.Send(conn, msg.From, "LIST - show list of online computers");
                JRobbot.Send(conn, msg.From, "UP - wake up computer, connected to user");
                if (context.IsAdmin())
                {
                    JRobbot.Send(conn, msg.From, "LISTCOMP - show list of computers");
                    JRobbot.Send(conn, msg.From, "ADDCOMP name ip mac - add computer to list");
                    JRobbot.Send(conn, msg.From, "DELCOMP name - remove computer from list");
                    JRobbot.Send(conn, msg.From, "LISTUSER - show list of users");
                    JRobbot.Send(conn, msg.From, "ADDUSER login password admin comp - add user to list");
                    JRobbot.Send(conn, msg.From, "DELUSER login - remove user from list");
                }
                JRobbot.Send(conn, msg.From, "QUIT - logout from system");
            }
            return true;
        }
    }
}