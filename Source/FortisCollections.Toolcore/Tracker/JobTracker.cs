using Sitecore.Jobs;
using Sitecore.SecurityModel;
using System.Collections.Generic;
using System.Linq;

namespace FortisCollections.Toolcore.Tracker
{
	public class JobTracker : IJobTracker
	{
		protected virtual string TrackerName => "Job";
		protected virtual string JobNotFoundMessage => "Unable to find job with ID {0}";
		protected virtual string NoStatusMessage => "Unable to get status";
		protected virtual string JobFailedMessage => $"{TrackerName} Failed";
		protected virtual string JobDoneMessage => "Index Rebuild Complete";
		protected virtual string JobProcessedMessage => "Processed: {0}";

		public IEnumerable<IProgress> Check(IEnumerable<string> ids)
		{
			using (new SecurityDisabler())
			{
				return ids.Select(i => CheckJob(i)).ToList();
			}
		}

		public IProgress Check(string id)
		{
			using (new SecurityDisabler())
			{
				return CheckJob(id);
			}
		}

		public IProgress CheckJob(string jobName)
		{
			var messages = new List<string>();
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
