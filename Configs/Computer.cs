using System;
using System.Xml.Serialization;

namespace jrobbot.Configs
{
	[Serializable]
	[XmlRoot("Computer")]
	public class Computer
	{
		public Computer()
		{
			
		}

		public Computer(string name, string ip, string mac)
		{
			Name = name;
			Ip = ip;
			Mac = mac;
		}

		[XmlAttribute("Name")]
		public string Name { get; set; }
		
		[XmlAttribute("Ip")]
		public string Ip { get; set; }
		
		[XmlAttribute("Mac")]
		public string Mac { get; set; }
	}
}