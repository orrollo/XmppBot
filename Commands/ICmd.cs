using agsXMPP;
using agsXMPP.protocol.client;

namespace jrobbot
{
    interface ICmd 
    {
        void Init(XmppClientConnection conn, Context ctx);
        bool Exec(Message msg);
    }
}