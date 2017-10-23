using System.Collections.Generic;
namespace com.udragan.csharp.Orphanator.Common.Interfaces
{
	/// <summary>
	/// A base interface for plugins.
	/// </summary>
	public interface ISolutionParser
	{
		/// <summary>
		/// Gets the IDE that this parser instance can handle.
		/// </summary>
		string Ide { get; }

		/// <summary>
		/// Determines whether this instance can handle the specified IDE.
		/// </summary>
		/// <param name="ide">The IDE.</param>
		/// <returns>
		///   <c>true</c> if this instance can handle the specified IDE; otherwise, <c>false</c>.
		/// </returns>
		bool CanHandle(string ide);

		/// <summary>
		/// Parse the solution/projects at provided path.
		/// </summary>
		/// <param name="args">The arguments.</param>
		/// <returns>A collection of paths to orphans.</returns>
		IEnumerable<string> Handle(string[] args);
	}
}
