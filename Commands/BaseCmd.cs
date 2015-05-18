using System;
using agsXMPP;
using agsXMPP.protocol.client;
using jrobbot.Core;

namespace jrobbot.Commands
{
    public abstract class BaseCmd : ICmd
    {
		public static readonly string CompCfgName = "comps.xml";
		public static readonly string UserCfgName = "users.xml";

        //protected XmppClientConnection conn;
        protected Context context;

        public void Init(/*XmppClientConnection conn,*/ Context ctx)
        {
            //this.conn = conn;
            this.context = ctx;
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