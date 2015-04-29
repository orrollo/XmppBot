using System.IO;
using System.Xml.Serialization;

namespace jrobbot.Configs
{
	public static class XmlHelper
	{
		public static void SaveToFile<T>(this T src, string fileName)
		{
			using (var fo = File.Create(fileName))
			{
				var xs = new XmlSerializer(typeof(T));
				xs.Serialize(fo, src);
			}
		}

		public static T LoadFromFile<T>(this string fileName) where T:new()
		{
			if (!File.Exists(fileName)) return new T();
			using (var fi = File.OpenRead(fileName))
			{
				var xs = new XmlSerializer(typeof(T));
				return (T)xs.Deserialize(fi);
			}
		}
	}
}