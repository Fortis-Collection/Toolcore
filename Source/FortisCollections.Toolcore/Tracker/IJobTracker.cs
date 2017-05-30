using System.Collections.Generic;

namespace FortisCollections.Toolcore.Tracker
{
	public interface IJobTracker
	{
		IEnumerable<IProgress> Check(IEnumerable<string> ids);
		IProgress Check(string id);
	}
}