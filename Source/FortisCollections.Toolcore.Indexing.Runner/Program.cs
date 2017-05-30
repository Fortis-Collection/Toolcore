using CommandLine;
using FortisCollections.Toolcore.Indexing.Runner.Indexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FortisCollections.Toolcore.Indexing.Runner
{
	class Program
	{
		private static int maxRetries = Properties.Settings.Default.MaxRetries;
		private static TimeSpan timeout = Properties.Settings.Default.Timeout;
		private static TimeSpan retryInterval = Properties.Settings.Default.RetryInterval;

		static void Main(string[] args)
		{
			WriteMessage("-------------------------");
			WriteMessage("Toolcore - Indexing (1.0)");
			WriteMessage("-------------------------");

			var options = new Options();
			var parser = new Parser(config => config.HelpWriter = Console.Out);
			var result = parser.ParseArguments(args, options);

			if (options.Timeout > 0)
			{
				timeout = TimeSpan.FromMilliseconds(options.Timeout);
			}

			if (options.MaxRetries > 0)
			{
				maxRetries = options.MaxRetries;
			}

			Run(options);
		}

		static void Run(Options options)
		{
			var sitecoreUrl = options.SitecoreUrl;
			var indexNames = options.IndexNames ?? new string[] { };
			var hostUrl = sitecoreUrl.LastIndexOf("/") != sitecoreUrl.Length - 1 ? sitecoreUrl + "/" : sitecoreUrl;
			var serviceUrl = string.Concat(hostUrl, Properties.Settings.Default.ServiceFolder, Properties.Settings.Default.ServiceFileName);

			using (var indexingService = new Indexing.Indexing())
			{
				indexingService.Url = serviceUrl;
				indexingService.Timeout = Convert.ToInt32(timeout.TotalMilliseconds);

				WriteMessage("Initializing publishing process");
				WriteMessage(string.Format("	Sitecore URL: {0}", serviceUrl));
				WriteMessage(string.Format("	Indexes: {0}", indexNames.Any() ? indexNames.ToString() : "All"));

				var progresses = Enumerable.Empty<Progress>();
				var success = false;
				var exceptions = new List<Exception>();

				for (int retry = 0; retry < maxRetries && !success; retry++)
				{
					try
					{
						if (retry > 0)
						{
							WriteMessage("Waiting {0}m {1}s before attempting indexing again.", retryInterval.Minutes, retryInterval.Seconds);
							Thread.Sleep(retryInterval);
						}
						else
						{
							WriteMessage("Starting indexing process | Max Timeout: {0}m {1}s | Max Retries: {2}", timeout.Minutes, timeout.Seconds, maxRetries);
						}

						progresses = indexNames.Any() ? indexingService.Rebuild(indexNames) : indexingService.RebuildAll();

						success = true;
					}
					catch (TimeoutException ex)
					{
						WriteMessage("Failed to contact Sitecore instance, retrying ({0}).", retry + 1);
						exceptions.Add(ex);
					}
					catch (Exception ex)
					{
						WriteMessage("An error has ocurred while trying to index. Check the Sitecore log file for any exceptions.");
						exceptions.Add(ex);
						break;
					}
				}

				if (!success)
				{
					Environment.Exit(103);
				}

				var ids = progresses.Select(p => p.Id).ToList();
				var previousProgresses = progresses;

				while (ids.Any())
				{
					WriteMessage("----------------");

					foreach (var progress in progresses)
					{
						var previousProgress = previousProgresses.FirstOrDefault(tp => string.Equals(tp.Id, progress.Id));

						foreach(var message in progress.Messages)
						{
							WriteMessage($"{progress.Id}: {message}");
						}

						if (progress.Complete)
						{
							ids.Remove(progress.Id);
						}
					}

					Thread.Sleep(1500);

					previousProgresses = progresses;
					progresses = indexingService.CheckMultiple(ids.ToArray());
				}

				WriteMessage("Finished");
			}
		}

		static void WriteMessage(string format, params object[] args)
		{
			Console.Write(string.Format("[{0}] ", DateTime.Now.ToString("HH:mm:ss")));
			Console.WriteLine(format, args);
		}
	}
}
