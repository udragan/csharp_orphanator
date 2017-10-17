using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
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
			_parsers = new List<ISolutionParser>();
			ComposablePartCatalog catalog = new DirectoryCatalog(@".\plugins\");

			CompositionContainer cc = new CompositionContainer(catalog);
			cc.ComposeParts(this);
		}

		#endregion
	}
}
