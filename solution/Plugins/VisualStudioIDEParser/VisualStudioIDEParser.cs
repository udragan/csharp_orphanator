﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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

			return result;
		}

		#endregion
	}
}
