using Sitecore.Jobs;
using Sitecore.SecurityModel;

namespace FortisCollections.Toolcore.Publish
{
	public class PublisherTracker : IPublisherTracker
	{
		public IPublishProgress Check(string id)
		{
			var jobName = id;

			using (new SecurityDisabler())
			{
				var job = JobManager.GetJob(jobName);

				if (job == null)
				{
					return new PublishProgress { Complete = true, Message = string.Format("Unable to get job with handle {0}", jobName) };
				}

				var status = job.Status;

				if (status == null)
				{
					return new PublishProgress { Complete = true, Message = "Unable to get status" };
				}

				var stopped = status.Failed || job.IsDone;
				var message = string.Empty;

				if (status.Failed)
				{
					message = "Published Failed";
				}
				else if (job.IsDone)
				{
					message = string.Format("Publish Complete - Processed: {0}", status.Processed);
				}
				else
				{
					message = string.Format("Processed: {0}", status.Processed);
				}

				return new PublishProgress { Complete = stopped, Processed = status.Processed, Message = message };
			}
		}
	}
}
