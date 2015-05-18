using System.Configuration;
using agsXMPP.protocol.client;
using jrobbot.Configs;
using jrobbot.Core;

namespace jrobbot.Commands
{
    class AuthCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            if (context.IsAuth()) return false;
            //var auth = context.GetAs<bool>("isAuth", false);
            //if (auth) return false;

            var pp = GetCmdParts(msg);
	        var fromJid = msg.From;
	        if (pp.Length == 0 || pp.Length != 3 || pp[0] != "USER")
            {
                JRobbot.Send(fromJid, "command must be: USER login password");
            }
            else 
            {
                var login = pp[1].ToLower();
                var password = pp[2];

				var ok = false;
				var userList = UserCfgName.ConfigName().LoadFromFile<UserList>();
	            foreach (var ui in userList)
	            {
					if (ui.Login.ToLower() != login || ui.Password != password) continue;
					ok = true;
					context["isAuth"] = true;
					context["level"] = ui.IsAdmin ? 2 : 1;
					context["user"] = ui.Login;
					context["comp"] = ui.CompName;
					JRobbot.Send(fromJid, "welcome, " + ui.Login + "!\r\n");
					break;
	            }
                if (!ok) JRobbot.Send(fromJid, "bad login-password pair" + pp[1]);
            }
            return true;
        }
    }
}