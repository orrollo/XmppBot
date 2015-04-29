using System.Configuration;

namespace jrobbot
{
    public class UserInfoSection : ConfigurationSection
    {
        [ConfigurationProperty("",IsRequired = true,IsDefaultCollection = true)]
        public UserInfoCollection UserInfo
        {
            get { return (UserInfoCollection)this[""]; }
            set { this[""] = value; }
        }
    }
}