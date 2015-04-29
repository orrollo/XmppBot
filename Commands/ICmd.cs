using agsXMPP;
using agsXMPP.protocol.client;
using jrobbot.Core;

namespace jrobbot.Commands
{
    interface ICmd 
    {
        void Init(XmppClientConnection conn, Context ctx);
        bool Exec(Message msg);
    }
}