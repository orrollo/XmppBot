using System.Configuration;

namespace jrobbot.Configs
{
    public class UserInfoCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UserInfo();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UserInfo)element).Login;
        }
    }
}