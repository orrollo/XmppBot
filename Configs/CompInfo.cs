using System.Configuration;

namespace jrobbot
{
    public class CompInfo : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["Name"]; }
            set { base["Name"] = value; }
        }

        [ConfigurationProperty("Descr", IsKey = false, IsRequired = false, DefaultValue = "-")]
        public string Descr
        {
            get { return (string)base["Descr"]; }
            set { base["Descr"] = value; }
        }

        [ConfigurationProperty("Mac", IsKey = false, IsRequired = true, DefaultValue = "")]
        public string Mac
        {
            get { return (string)base["Mac"]; }
            set { base["Mac"] = value; }
        }

        [ConfigurationProperty("Ip", IsKey = false, IsRequired = true, DefaultValue = "")]
        public string Ip
        {
            get { return (string)base["Ip"]; }
            set { base["Ip"] = value; }
        }
    }
}