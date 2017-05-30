using FortisCollections.Toolcore.Tracker;
using System.Collections.Generic;
using System.Web.Services;

namespace FortisCollections.Toolcore.Indexing.Service
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	public class Indexing : WebService
	{
		[WebMethod(Description = "Rebuild all indexes")]
		public IEnumerable<IProgress> RebuildAll()
		{
			var indexRebuilder = new IndexRebuilder();

			return indexRebuilder.RebuildAll();
		}

		[WebMethod(Description = "Rebuild specific indexes")]
		public IEnumerable<IProgress> Rebuild(string[] indexNames)
		{
			var indexRebuilder = new IndexRebuilder();

			return indexRebuilder.Rebuild(indexNames);
		}

		[WebMethod(Description = "Check progress")]
		public IProgress Check(string id)
		{
			var tracker = new JobTracker();

			return tracker.Check(id);
		}
	}
}
