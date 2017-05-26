using FortisCollections.Toolcore.Tracker;
using Sitecore.Jobs;
using Sitecore.SecurityModel;
using System.Collections.Generic;

namespace FortisCollections.Toolcore.Indexing
{
	public class JobTracker
	{
		protected virtual string TrackerName => "Job";
		protected virtual string JobNotFoundMessage => "Unable to find job with ID {0}";
		protected virtual string NoStatusMessage => "Unable to get status";
		protected virtual string JobFailedMessage => $"{TrackerName} Failed";
		protected virtual string JobDoneMessage => "Index Rebuild Complete";
		protected virtual string JobProcessedMessage => "Processed: {0}";

		public IProgress Check(string id)
		{
			var jobName = id;
			var messages = new List<string>();

			using (new SecurityDisabler())
			{
				var job = JobManager.GetJob(jobName);

				if (job == null)
				{
					messages.Add(string.Format(JobNotFoundMessage, jobName));

					return new Progress { Complete = true, Messages = messages };
				}

				var status = job.Status;

				if (status == null)
				{
					messages.Add(NoStatusMessage);

					return new Progress { Complete = true, Messages = messages };
				}

				var stopped = status.Failed || job.IsDone;
				var message = string.Empty;

				messages.Add(string.Format(JobProcessedMessage, status.Processed));

				if (status.Failed)
				{
					messages.Add(JobFailedMessage);
				}
				else if (job.IsDone)
				{
					messages.Add(string.Format(JobDoneMessage, status.Processed));
				}

				return new Progress { Complete = stopped, Processed = status.Processed, Messages = messages };
			}
		}

		public Progress Create(Job job, bool stopped, IEnumerable<string> messages)
		{
			return new Progress
			{
				Complete = stopped,
				Id = job.Name,
				Messages = messages,
				Processed = job.Status.Processed
			};
		}
	}
}
