using System.Collections.Generic;

namespace jrobbot.Core
{
    public class Context : Dictionary<string,object>
    {
        public T GetAs<T>(string name)
        {
            return GetAs<T>(name, default(T));
        }

        public T GetAs<T>(string name, T defValue)
        {
            return ContainsKey(name) ? (T)this[name] : defValue;
        }
    }
}