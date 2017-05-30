using FortisCollections.Toolcore.Tracker;

namespace FortisCollections.Toolcore.Indexing
{
	public class IndexRebuildTracker : JobTracker, IIndexRebuildTracker
	{
		protected override string TrackerName => "Index Rebuild";
	}
}
