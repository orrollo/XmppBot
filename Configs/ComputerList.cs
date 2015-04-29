using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace jrobbot.Configs
{
	[Serializable]
	[XmlRoot("Computers")]
	public class ComputerList : List<Computer> { }
}