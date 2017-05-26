using System.Collections.Generic;

namespace FortisCollections.Toolcore.Tracker
{
	public interface IProgress
	{
		bool Complete { get; }
		string Id { get; }
		IEnumerable<string> Messages { get; }
		long Processed { get; }
	}
}