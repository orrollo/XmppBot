using System;
using System.Threading;
using agsXMPP.protocol.client;

namespace jrobbot
{
    class QuitCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            var pp = GetCmdParts(msg);
            if (pp.Length == 0 || (pp[0] != "EXIT" && pp[0] != "QUIT" && pp[0] != "Q!")) return false;
            JRobbot.Send(conn, msg.From, "see you!");
            ctx.Clear();
            throw new ClientStopException();
            return true;
        }
    }
}