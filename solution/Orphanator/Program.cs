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
			string solutionPath = string.Empty;
			bool isDryRun = false;

			if (argumentsParsed &&
				args.Length < 2)
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

			if (argumentsParsed)
			{
				if (!string.Equals(args[2], "-path", StringComparison.OrdinalIgnoreCase))
				{
					Console.WriteLine("wrong argument");
					argumentsParsed = false;
				}
				else
				{
					solutionPath = args[3];
				}
			}

			if (argumentsParsed)
			{
				if (args.Length < 5)
				{
					Console.WriteLine("no dry-run argument.");
				}
				else
				{
					isDryRun = true;
				}
			}

			Console.WriteLine("Arguments");
			Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "\tide\t: {0}", ide));
			Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "\tpath\t: {0}", solutionPath));
			Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "\tdry-run\t: {0}", isDryRun));
			Console.WriteLine("---------------------------");
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

						pluginImporter.Handle(args);

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
