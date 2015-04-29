using System.Configuration;

namespace jrobbot.Configs
{
    public class CompInfoSection : ConfigurationSection
    {
        [ConfigurationProperty("",IsRequired = true,IsDefaultCollection = true)]
        public CompInfoCollection CompInfo
        {
            get { return (CompInfoCollection)this[""]; }
            set { this[""] = value; }
        }
    }
}