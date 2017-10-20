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

			if (argumentsParsed &&
				args.Length != 2)
			{
				System.Console.WriteLine("No sufficient parameters!");
				argumentsParsed = false;
			}

			if (argumentsParsed)
			{
				if (!string.Equals(args[0], "-ide", StringComparison.OrdinalIgnoreCase))
				{
					Console.WriteLine("wrong argument");
					argumentsParsed = false;
				}
				else
				{
					ide = args[1];
				}
			}
			//////////////////////////////////////////////

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
