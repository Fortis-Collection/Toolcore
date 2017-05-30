using FortisCollections.Toolcore.Tracker;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.Jobs;
using System.Collections.Generic;
using System.Linq;

namespace FortisCollections.Toolcore.Indexing
{
	public class IndexRebuilder : IIndexRebuilder
	{
		public IEnumerable<IProgress> RebuildAll()
		{
			return IndexCustodian.RebuildAll().Select(j => Create(j)).ToList();
		}

		public IEnumerable<IProgress> Rebuild(string[] indexNames)
		{
			var jobs = new List<IProgress>();

			foreach (var index in ContentSearchManager.Indexes.Where(i => indexNames.Contains(i.Name)))
			{
				jobs.Add(Create(IndexCustodian.FullRebuild(index)));
			}

			return jobs;
		}

		public IProgress Create(Job job)
		{
			return new Progress
			{
				Id = job.Name,
				Complete = false,
				Messages = new string[] { $"Rebuilding: {job.DisplayName}" },
				Processed = job.Status.Processed
			};
		}
	}
}
