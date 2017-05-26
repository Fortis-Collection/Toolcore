using System.Collections.Generic;

namespace FortisCollections.Toolcore.Tracker
{
	public class Progress : IProgress
	{
		public string Id { get; set; }
		public bool Complete { get; set; }
		public long Processed { get; set; }
		public IEnumerable<string> Messages { get; set; }
	}
}
