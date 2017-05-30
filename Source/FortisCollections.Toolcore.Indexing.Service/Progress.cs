using System.Collections.Generic;

namespace FortisCollections.Toolcore.Indexing.Service
{
	public class Progress
	{
		public string Id { get; set; }
		public bool Complete { get; set; }
		public long Processed { get; set; }
		public List<string> Messages { get; set; }
	}
}