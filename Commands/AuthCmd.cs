using System;
using System.Configuration;
using agsXMPP;
using agsXMPP.protocol.client;

namespace jrobbot
{
    class AuthCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            if (ctx.IsAuth()) return false;
            //var auth = ctx.GetAs<bool>("isAuth", false);
            //if (auth) return false;

            var pp = GetCmdParts(msg);
            if (pp.Length == 0 || pp.Length != 3 || pp[0] != "USER")
            {
                JRobbot.Send(conn, msg.From, "command must be: USER login password");
            }
            else 
            {
                var login = pp[1].ToLower();
                var password = pp[2];

                var config = ConfigurationManager.GetSection("UserInfo") as UserInfoSection;
                var ok = false;
                foreach (UserInfo ui in config.UserInfo)
                {
                    if (ui.Login.ToLower() != login || ui.Password != password) continue;
                    ok = true;
                    ctx["isAuth"] = true;
                    ctx["level"] = ui.IsAdmin ? 2 : 1;
                    ctx["user"] = ui.Login;
                    ctx["comp"] = ui.CompName;
                    JRobbot.Send(conn, msg.From, "welcome, " + ui.Login + "!\r\n");
                    break;
                }
                if (!ok) JRobbot.Send(conn, msg.From, "bad login-password pair" + pp[1]);
            }
            return true;
        }
    }
}