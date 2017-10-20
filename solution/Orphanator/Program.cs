using System;
using System.Globalization;
using com.udragan.csharp.Orphanator.MEF;

namespace com.udragan.csharp.Orphanator
{
	class Program
	{
		static void Main(string[] args)
		{
			// parse arguments (TODO: create separate generic arguments parser)
			bool argumentsParsed = true;
			string ide = string.Empty;

			if (args.Length != 2)
			{
				System.Console.WriteLine("No sufficient parameters!");
				argumentsParsed = false;
			}

			if (!string.Equals(args[0], "-ide", StringComparison.InvariantCultureIgnoreCase))
			{
				Console.WriteLine("wrong argument");
				argumentsParsed = false;
			}

			ide = args[1];

			if (argumentsParsed)
			{
				// initialize plugins
				PluginImporter pluginImporter = new PluginImporter();
				pluginImporter.Import();

				if (pluginImporter.Initialized)
				{
					System.Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} plugin(s) loaded.",
					pluginImporter.Count));

					if (pluginImporter.CanHandle(ide))
					{
						System.Console.WriteLine("handled.");

					}
					else
					{
						System.Console.WriteLine("not handled.");
					}
				}
			}

			System.Console.ReadLine();
		}
	}
}
