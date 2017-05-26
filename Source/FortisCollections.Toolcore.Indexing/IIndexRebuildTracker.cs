using FortisCollections.Toolcore.Tracker;

namespace FortisCollections.Toolcore.Indexing
{
	public interface IIndexRebuildTracker
	{
		IProgress Check(string id);
	}
}