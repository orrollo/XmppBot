using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using agsXMPP.protocol.client;
using jrobbot.Configs;
using jrobbot.Core;

namespace jrobbot.Commands
{
    class UpCmd : BaseCmd
    {
        public class WolClass : UdpClient
        {
	        //this is needed to send broadcast packet
            public void SetClientToBrodcastMode()
            {
                if (Active) Client.SetSocketOption(SocketOptionLevel.Socket, 
					SocketOptionName.Broadcast, 0);
            }
        }

        //MAC_ADDRESS should  look like '013FA049'

        private void WakeFunction(string macAddress)
        {
            var client = new WolClass();
            client.Connect(new
               IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
               0x7); // port=12287 let's use this one 
            client.SetClientToBrodcastMode();
            
            //set sending bites
            var counter = 0;
            
            //buffer to be send
            var bytes = new byte[1024];   // more than enough :-)
            var mac = new byte[6];
            
            //first 6 bytes should be 0xFF
            for (var y = 0; y < 6; y++)
            {
                bytes[counter++] = 0xFF;
                mac[y] = byte.Parse(macAddress.Substring(y*2, 2), NumberStyles.HexNumber);
            }

            //now repeate MAC 16 times
            for (var y = 0; y < 16; y++) for (var z = 0; z < 6; z++) bytes[counter++] = mac[z];

            //now send wake up packet
            client.Send(bytes, counter);
        }

        public override bool Exec(Message msg)
        {
            if (!context.IsAuth()) return false;
            var pp = GetCmdParts(msg);
            if (pp.Length == 0 || pp[0] != "UP") return false;

			var fromJid = msg.From;
			var comp = context.IsAdmin() && pp.Length > 1 ? pp[1] : context.GetAs("comp", "");

	        if (string.IsNullOrEmpty(comp))
            {
                JRobbot.Send(fromJid, "computer name is not set");
                return true;
            }

            var ok = false;
			var fileName = CompCfgName.ConfigName();
			var computerList = fileName.LoadFromFile<ComputerList>();
			foreach (var ci in computerList)
			{
				if (ci.Name.ToLower() != comp.ToLower()) continue;
				ok = true;
				var macAddress = ci.Mac.Trim().ToUpper();
				if (string.IsNullOrEmpty(macAddress))
				{
					JRobbot.Send(fromJid, "the MAC address not set for computer <" + comp + ">");
					break;
				}
				if (macAddress.Length != 12)
				{
					JRobbot.Send(fromJid, "the MAC address must be 12 chars");
					break;
				}
				//
				var corr = macAddress.All(ch => ((ch >= '0') && (ch <= '9')) || ((ch >= 'A') && (ch <= 'F')));
				if (!corr)
				{
					JRobbot.Send(fromJid, "the MAC address must be hex humber");
					break;
				}
				WakeFunction(macAddress);
				JRobbot.Send(fromJid, "wake up packed sended to <" + comp + ">");
			}

            if (!ok) JRobbot.Send(fromJid, "computer with name <" + comp + "> not in list");
            return true;
        }
    }
}