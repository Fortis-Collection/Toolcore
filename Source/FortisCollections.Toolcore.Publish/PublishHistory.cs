using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortisCollections.Toolcore.Publish
{
	public class PublishHistory
	{
		public Guid Id { get; set; }
		public DateTime Started { get; set; }
		public DateTime Ended { get; set; }
		public string RootItem { get; set; }
		public string PublishMode { get; set; }
		public int Processed { get; set; }
		public bool Completed { get; set; }
	}
}
