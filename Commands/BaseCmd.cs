using System;
using agsXMPP;
using agsXMPP.protocol.client;

namespace jrobbot
{
    public abstract class BaseCmd : ICmd
    {
        protected XmppClientConnection conn;
        protected Context ctx;

        public void Init(XmppClientConnection conn, Context ctx)
        {
            this.conn = conn;
            this.ctx = ctx;
        }

        public abstract bool Exec(Message msg);

        protected string[] GetCmdParts(Message msg)
        {
            var txt = (msg.Body ?? "").Trim();
            var pp = txt.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (pp.Length > 0) pp[0] = pp[0].ToUpper();
            return pp;
        }
    }
}