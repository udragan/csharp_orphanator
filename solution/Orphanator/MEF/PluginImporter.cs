using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using com.udragan.csharp.Orphanator.Common.Interfaces;

namespace com.udragan.csharp.Orphanator.MEF
{
	/// <summary>
	/// The class for performing dynamic import of available plugins.
	/// </summary>
	internal sealed class PluginImporter
	{
		#region Members

		[ImportMany(typeof(ISolutionParser))]
		private ICollection<ISolutionParser> _parsers;

		private string _pluginsFolder = "plugins";

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="PluginImporter"/> is initialized.
		/// </summary>
		public bool Initialized { get; set; }

		/// <summary>
		/// Gets the count of loaded plugins.
		/// </summary>
		public int Count
		{
			get { return _parsers != null ? _parsers.Count : 0; }
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Dynamically imports available plugins.
		/// </summary>
		public void Import()
		{
			string pluginsAbsolutePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
					_pluginsFolder);

			if (!Directory.Exists(pluginsAbsolutePath))
			{
				System.Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Plugins directory does not exist! \n\t{0}",
					pluginsAbsolutePath));

				return;
			}

			_parsers = new List<ISolutionParser>();

			using (ComposablePartCatalog catalog = new DirectoryCatalog(pluginsAbsolutePath))
			{
				using (CompositionContainer cc = new CompositionContainer(catalog))
				{
					cc.ComposeParts(this);
					Initialized = true;
				}
			}
		}

		/// <summary>
		/// Determines whether any of the loaded plugins can handle the provided IDE.
		/// </summary>
		/// <param name="ide">The IDE.</param>
		/// <returns>
		///   <c>true</c> if any of the loaded plugins ca handle the provided IDE; otherwise, <c>false</c>.
		/// </returns>
		public bool CanHandle(string ide)
		{
			int result = _parsers.Where(x => x.CanHandle(ide)).Count();

			if (result == 1)
			{
				return true;
			}

			if (result == 0)
			{
				System.Console.WriteLine("No plugins can handle the request.");

				return false;
			}

			System.Console.WriteLine("Multiple plugins found that can handle the request.");

			return false;
		}

		/// <summary>
		/// Handles the specified arguments.
		/// </summary>
		/// <param name="args">The arguments.</param>
		/// <returns>
		/// A collection of paths to orphans.
		/// </returns>
		public IEnumerable<string> Handle(string[] args)
		{
			ISolutionParser handler = _parsers.First(x => x.CanHandle(args[1]));

			return handler.Handle(args);
		}

		#endregion
	}
}
