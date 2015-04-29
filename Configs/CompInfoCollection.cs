using System.Configuration;

namespace jrobbot.Configs
{
    public class CompInfoCollection : ConfigurationElementCollection
    {
        public CompInfo this[object key]
        {
            get
            {
                return (CompInfo)base.BaseGet(key);
            }
            set
            {
                if (base.BaseGet(key) != null) base.BaseRemove(key);
                if (value != null) base.BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new CompInfo();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CompInfo)element).Name;
        }
    }
}