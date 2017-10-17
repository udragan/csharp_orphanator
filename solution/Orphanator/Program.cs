using com.udragan.csharp.Orphanator.MEF;

namespace com.udragan.csharp.Orphanator
{
	class Program
	{
		static void Main(string[] args)
		{
			PluginImporter pluginImporter = new PluginImporter();

			pluginImporter.Import();

			System.Console.ReadLine();
		}
	}
}
