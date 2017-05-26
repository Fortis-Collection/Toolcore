using System.Collections.Generic;

namespace FortisCollections.Toolcore.Indexing
{
	public interface IIndexRebuilder
	{
		IEnumerable<IIndexProgress> Rebuild(string[] indexNames);
		IEnumerable<IIndexProgress> RebuildAll();
	}
}