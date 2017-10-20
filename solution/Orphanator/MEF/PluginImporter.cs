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

		private string _pluginsFolder = "plugins";

		[ImportMany(typeof(ISolutionParser))]
		private ICollection<ISolutionParser> _parsers;

		#endregion

		#region Properties

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
				}
			}
		}

		#endregion
	}
}
