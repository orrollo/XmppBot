using System.Configuration;

namespace jrobbot
{
    public class JRobotConfig : ConfigurationSection
    {
        public static JRobotConfig X
        {
            get
            {
                return ConfigurationManager.GetSection("JRobotConfig") as JRobotConfig;
            }
        }

        [ConfigurationProperty("Username", DefaultValue = "guest", IsRequired = true)]
        public string Username
        {
            get { return (string)this["Username"]; }
            set { this["Username"] = value; }
        }

        [ConfigurationProperty("Password", DefaultValue = "", IsRequired = true)]
        public string Password
        {
            get { return (string)this["Password"]; }
            set { this["Password"] = value; }
        }

        [ConfigurationProperty("Server", DefaultValue = "gmail.com", IsRequired = true)]
        public string Server
        {
            get { return (string)this["Server"]; }
            set { this["Server"] = value; }
        }

        [ConfigurationProperty("ConnectionServer", DefaultValue = "gtalk.google.com", IsRequired = true)]
        public string ConnectionServer
        {
            get { return (string)this["ConnectionServer"]; }
            set { this["ConnectionServer"] = value; }
        }
    }
}