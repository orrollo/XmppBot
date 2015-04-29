using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace jrobbot.Configs
{
	[Serializable]
	[XmlRoot("Users")]
	public class UserList : List<User> { }
}