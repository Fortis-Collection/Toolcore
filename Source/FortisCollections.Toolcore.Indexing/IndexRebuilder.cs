using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.Jobs;
using System.Collections.Generic;
using System.Linq;

namespace FortisCollections.Toolcore.Indexing
{
	public class IndexRebuilder : IIndexRebuilder
	{
		public IEnumerable<IIndexProgress> RebuildAll()
		{
			return IndexCustodian.RebuildAll().Select(j => Create(j)).ToList();
		}

		public IEnumerable<IIndexProgress> Rebuild(string[] indexNames)
		{
			var jobs = new List<IIndexProgress>();

			foreach (var index in ContentSearchManager.Indexes.Where(i => indexNames.Contains(i.Name)))
			{
				jobs.Add(Create(IndexCustodian.FullRebuild(index)));
			}

			return jobs;
		}

		public IIndexProgress Create(Job job)
		{
			return new IndexProgress
			{
				Id = job.Name,
				Complete = false,
				Message = $"Rebuilding: {job.DisplayName}",
				Processed = job.Status.Processed
			};
		}
	}
}
