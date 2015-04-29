using System.Configuration;

namespace jrobbot.Configs
{
    public class UserInfo : ConfigurationElement
    {
        [ConfigurationProperty("Login",IsKey = true,IsRequired = true)]
        public string Login
        {
            get { return (string)base["Login"]; }
            set { base["Login"] = value; }
        }

        [ConfigurationProperty("Password", IsKey = false, IsRequired = true)]
        public string Password
        {
            get { return (string)base["Password"]; }
            set { base["Password"] = value; }
        }

        [ConfigurationProperty("IsAdmin", IsKey = false, IsRequired = false, DefaultValue = false)]
        public bool IsAdmin
        {
            get { return (bool)base["IsAdmin"]; }
            set { base["IsAdmin"] = value; }
        }

        [ConfigurationProperty("CompName", IsKey = false, IsRequired = false, DefaultValue = "")]
        public string CompName
        {
            get { return (string)base["CompName"]; }
            set { base["CompName"] = value; }
        }

    }
}