using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using agsXMPP.protocol.client;

namespace jrobbot
{
    class UpCmd : BaseCmd
    {
        public class WOLClass : UdpClient
        {
            public WOLClass()
                : base()
            {

            }

            //this is needed to send broadcast packet
            public void SetClientToBrodcastMode()
            {
                if (Active) Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 0);
            }
        }

        //MAC_ADDRESS should  look like '013FA049'

        private void WakeFunction(string MAC_ADDRESS)
        {
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x7); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            
            //set sending bites
            int counter = 0;
            
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
            byte[] mac = new byte[6];
            
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
            {
                bytes[counter++] = 0xFF;
                mac[y] = byte.Parse(MAC_ADDRESS.Substring(y*2, 2), NumberStyles.HexNumber);
            }

            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++) for (int z = 0; z < 6; z++) bytes[counter++] = mac[z];

            //now send wake up packet
            int reterned_value = client.Send(bytes, counter);
        }

        public override bool Exec(Message msg)
        {
            if (!ctx.IsAuth()) return false;
            var pp = GetCmdParts(msg);
            if (pp.Length == 0 || pp[0] != "UP") return false;

            var comp = ctx.IsAdmin() && pp.Length > 1 ? pp[1] : ctx.GetAs<string>("comp", "");
            if (string.IsNullOrEmpty(comp))
            {
                JRobbot.Send(conn, msg.From, "computer name is not set");
                return true;
            }

            // ищем комп в списке компов
            var config = ConfigurationManager.GetSection("CompInfo") as CompInfoSection;
            var ok = false;
            foreach (CompInfo ci in config.CompInfo)
            {
                if (ci.Name.ToLower() != comp.ToLower()) continue;
                ok = true;
                var macAddress = ci.Mac.Trim().ToUpper();
                if (string.IsNullOrEmpty(macAddress))
                {
                    JRobbot.Send(conn, msg.From, "the MAC address not set for computer <" + comp + ">");
                    break;
                }
                if (macAddress.Length != 12)
                {
                    JRobbot.Send(conn, msg.From, "the MAC address must be 12 chars");
                    break;
                }
                //
                var corr = macAddress.All(ch => ((ch >= '0') && (ch <= '9')) || ((ch >= 'A') && (ch <= 'F')));
                if (!corr)
                {
                    JRobbot.Send(conn, msg.From, "the MAC address must be hex humber");
                    break;
                }
                WakeFunction(macAddress);
                JRobbot.Send(conn, msg.From, "wake up packed sended to <" + comp + ">");
            }
            if (!ok) JRobbot.Send(conn, msg.From, "computer with name <" + comp + "> not in list");
            return true;
        }
    }
}