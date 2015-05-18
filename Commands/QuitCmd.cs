using agsXMPP.protocol.client;
using jrobbot.Core;

namespace jrobbot.Commands
{
    class QuitCmd : BaseCmd
    {
        public override bool Exec(Message msg)
        {
            var pp = GetCmdParts(msg);
            if (pp.Length == 0 || (pp[0] != "EXIT" && pp[0] != "QUIT" && pp[0] != "Q!")) return false;
            JRobbot.Send(msg.From, "see you!");
            context.Clear();
            throw new ClientStopException();
        }
    }
}