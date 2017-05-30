using FortisCollections.Toolcore.Tracker;
using System.Collections.Generic;

namespace FortisCollections.Toolcore.Indexing
{
	public interface IIndexRebuilder
	{
		IEnumerable<IProgress> Rebuild(string[] indexNames);
		IEnumerable<IProgress> RebuildAll();
	}
}