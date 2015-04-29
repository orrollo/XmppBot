using System.Configuration;
using System.Linq;
using agsXMPP.protocol.client;

namespace jrobbot
{
    //public class AddComp : BaseCmd
    //{
    //    public override bool Exec(Message msg)
    //    {
    //        if (!ctx.IsAuth()) return false;
    //        if (!ctx.IsAdmin()) return false;
    //        var pp = GetCmdParts(msg);
    //        if (pp.Length != 4 || pp[0] != "ADDCOMP") return false;
    //        // 1 - name, 2 - ip, 3 - mac
    //        string name = pp[1], ip = pp[2], mac = pp[3];
    //        var config = ConfigurationManager.GetSection("CompInfo") as CompInfoSection;

    //        var old = config.CompInfo[name];
    //        if (old != null)
    //        {
    //            JRobbot.Send(conn, msg.From, "error: computer {0} already exists!".Fmt(name));
    //            return true;
    //        }

    //        var ce = new CompInfo();
    //        ce.Name = name;
    //        ce.Ip = ip;
    //        ce.Mac = mac;
    //        ce.Descr = name;
    //        config.CompInfo[name] = ce;
    //        return true;
    //    }
    //}

    //class EchoCmd : BaseCmd
    //{
    //    public override bool Exec(Message msg)
    //    {
    //        var auth = ctx.GetAs<bool>("isAuth", false);
    //        if (!auth) return false;
    //        JRobbot.Send(conn, msg.From, "echo: " + msg.Body);
    //        return true;
    //    }
    //}
}