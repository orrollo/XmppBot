using System;
using System.Xml.Serialization;

namespace jrobbot.Configs
{
	[Serializable]
	[XmlRoot("User")]
	public class User
	{
		public User()
		{
			
		}

		public User(string login, string password, bool isAdmin, string compName)
		{
			Login = login;
			Password = password;
			IsAdmin = isAdmin;
			CompName = compName;
		}

		[XmlAttribute("Login")]
		public string Login { get; set; }
		
		[XmlAttribute("Password")]
		public string Password { get; set; }

		[XmlAttribute("IsAdmin")]
		public bool IsAdmin { get; set; }
	
		[XmlAttribute("CompName")]
		public string CompName { get; set; }
	}
}