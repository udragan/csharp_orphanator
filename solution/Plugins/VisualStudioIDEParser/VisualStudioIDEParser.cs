using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using com.udragan.csharp.Orphanator.Common.Interfaces;

namespace com.udragan.csharp.Orphanator.Plugins.VisualStudioIDEParser
{
	/// <summary>
	/// Ide parser for Visual Studio projects.
	/// </summary>
	/// <seealso cref="com.udragan.csharp.Orphanator.Common.Interfaces.ISolutionParser" />
	[Export(typeof(ISolutionParser))]
	public class VisualStudioIDEParser : ISolutionParser
	{
		#region Members

		private const string IDE = "vs";
		private string _defaultOutputPath = @"\obj\";

		#endregion

		#region ISolutionParser

		/// <summary>
		/// Gets the IDE that this parser instance can handle.
		/// </summary>
		public string Ide
		{
			get { return IDE; }
		}

		/// <summary>
		/// Determines whether this instance can handle the specified IDE.
		/// </summary>
		/// <param name="ide">The IDE.</param>
		/// <returns>
		///   <c>true</c> if this instance can handle the specified IDE; otherwise, <c>false</c>.
		/// </returns>
		public bool CanHandle(string ide)
		{
			return string.Equals(IDE, ide, StringComparison.OrdinalIgnoreCase);
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
			IEnumerable<string> result = new List<string>();

			if (args == null)
			{
				return result;
			}

			if (!Directory.Exists(args[3]))
			{
				Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "Directory {0} does not exist!",
					args[3]));

				return result;
			}

			// retrieve all project files
			//TODO: consider all vs project files, not only csproj
			IEnumerable<string> projectFilePaths = Directory.EnumerateFiles(args[3], "*.csproj", SearchOption.AllDirectories);

			Console.WriteLine("\nFound project files:");
			Console.WriteLine("---------------------------");
			Console.WriteLine(string.Join("\n", projectFilePaths.Select(x => Path.GetFullPath(x))));
			Console.WriteLine();
			Console.WriteLine("Parsing project files...");

			foreach (string projectFilePath in projectFilePaths)
			{
				Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "\t{0}",
					Path.GetFileName(projectFilePath)));

				XDocument projectDoc = XDocument.Load(projectFilePath);

				IEnumerable<string> outputPaths = GetAbsoluteOutputPaths(projectDoc, projectFilePath);


				// retrieve all file paths of interest but skip project output paths
				IEnumerable<string> allFiles = Directory.EnumerateFiles(args[3], "*.*", SearchOption.AllDirectories)
					.Where(x => outputPaths.All(y => !x.Contains(y)))
					.Where(x => Path.GetExtension(x).Equals(".cs"));

				Console.WriteLine(string.Join("\n", allFiles));
				Console.WriteLine();


				// retrieve file paths of interest (cs, xaml, xml...) from project file
				//TODO: consider all applicable file extensions, not only cs
				IEnumerable<string> parsedFiles = projectDoc.Descendants()
					.Where(x => x.Attribute("Include") != null)
					.Where(x => Path.GetExtension(x.Attribute("Include").Value).Equals(".cs"))
					.Select(x => Path.Combine(Path.GetDirectoryName(projectFilePath), x.Attribute("Include").Value));

				Console.WriteLine(string.Join("\n", parsedFiles));
				Console.WriteLine();


				result = allFiles.Except(parsedFiles);

				Console.WriteLine(string.Join("\n", result));

			}

			return result;
		}

		#endregion

		#region Private methods

		[Pure]
		private static XNamespace GetNamespace(XDocument doc)
		{
			return doc.Root.GetDefaultNamespace();
		}

		[Pure]
		private IEnumerable<string> GetAbsoluteOutputPaths(XDocument doc, string projectFilePath)
		{
			XNamespace documentNamespace = GetNamespace(doc);

			IList<string> outputPathNodes = doc.Descendants(documentNamespace + "OutputPath")
				.Select(x => Path.Combine(Path.GetDirectoryName(projectFilePath), x.Value))
				.ToList();
			outputPathNodes.Add(Path.Combine(Path.GetDirectoryName(projectFilePath), _defaultOutputPath));


			Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "\t\tOutput paths: {0}",
					string.Join(", ", outputPathNodes)));
			Console.WriteLine();

			return outputPathNodes;
		}

		#endregion
	}
}
