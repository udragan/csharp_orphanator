using System.Globalization;
using com.udragan.csharp.Orphanator.MEF;

namespace com.udragan.csharp.Orphanator
{
	class Program
	{
		static void Main(string[] args)
		{
			// parse arguments (create separate generic arguments parser)


			// initialize plugins
			PluginImporter pluginImporter = new PluginImporter();
			pluginImporter.Import();


			System.Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} plugin(s) loaded.",
				pluginImporter.Count));

			System.Console.ReadLine();
		}
	}
}
