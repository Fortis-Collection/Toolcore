using FortisCollections.Toolcore.Tracker;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

namespace FortisCollections.Toolcore.Indexing.Service
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	public class Indexing : WebService
	{
		[WebMethod(Description = "Rebuild all indexes")]
		public List<Progress> RebuildAll()
		{
			var progresses = (new IndexRebuilder()).RebuildAll();
			
			return Create(progresses);
		}

		[WebMethod(Description = "Rebuild specific indexes")]
		public List<Progress> Rebuild(string[] indexNames)
		{
			var progresses = (new IndexRebuilder()).Rebuild(indexNames);

			return Create(progresses);
		}

		[WebMethod(Description = "Check progress")]
		public Progress Check(string id)
		{
			var tracker = new JobTracker();
			var progress = tracker.Check(id);

			return Create(progress);
		}

		private List<Progress> Create(IEnumerable<IProgress> progresses)
		{
			return progresses.Select(p => Create(p)).ToList();
		}

		private Progress Create(IProgress progress)
		{
			return new Progress
			{
				Complete = progress.Complete,
				Id = progress.Id,
				Messages = progress.Messages.ToList(),
				Processed = progress.Processed
			};
		}
	}
}
