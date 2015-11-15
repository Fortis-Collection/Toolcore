using CommandLine;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace FortisCollections.Toolcore.Publish.Runner
{
	class Program
	{
		private static int maxRetries = Properties.Settings.Default.MaxRetries;
		private static TimeSpan timeout = Properties.Settings.Default.Timeout;
		private static TimeSpan retryInterval = Properties.Settings.Default.RetryInterval;

		static void Main(string[] args)
		{
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

			Thread.Sleep(TimeSpan.FromSeconds(30));

			RunPublish(options.SitecoreUrl, options.SourceDatabaseName, options.TargetDatabaseNames, options.LanguageNames);
		}

		static void RunPublish(string sitecoreUrl, string sourceDatabaseName, string[] targetDatabaseNames, string[] languageNames)
		{
			var targets = targetDatabaseNames ?? new string[] { };
			var languages = languageNames ?? new string[] { };
			var hostUrl = sitecoreUrl.LastIndexOf("/") != sitecoreUrl.Length - 1 ? sitecoreUrl + "/" : sitecoreUrl;
			var serviceUrl = string.Concat(hostUrl, Properties.Settings.Default.ServiceFolder, Properties.Settings.Default.ServiceFileName);

			using (var publishingService = new Publishing.Publishing())
			{
				publishingService.Url = serviceUrl;
				publishingService.Timeout = Convert.ToInt32(timeout.TotalMilliseconds);

				WriteMessage("Initializing publishing process");
				WriteMessage(string.Format("	Sitecore URL: {0}", serviceUrl));
				WriteMessage(string.Format("	Source: {0}", sourceDatabaseName));
				WriteMessage(string.Format("	Targets: {0}", targets.Any() ? targets.ToString() : "All"));
				WriteMessage(string.Format("	Languages: {0}", languages.Any() ? languages.ToString() : "All"));

				var id = string.Empty;
				var success = false;
				var exceptions = new List<Exception>();

				for (int retry = 0; retry < maxRetries && !success; retry++)
				{
					try
					{
						if (retry > 0)
						{
							WriteMessage("Waiting {0}m {1}s before attempting publishing again.", retryInterval.Minutes, retryInterval.Seconds);
							Thread.Sleep(retryInterval);
						}
						else
						{
							WriteMessage("Starting publishing process | Max Timeout: {0}m {1}s | Max Retries: {2}", timeout.Minutes, timeout.Seconds, maxRetries);
						}

						id = publishingService.Publish(sourceDatabaseName, targets, languages);
						
						success = true;
					}
					catch (TimeoutException ex)
					{
						WriteMessage("Failed to contact Sitecore instance, retrying ({0}).", retry + 1);
						exceptions.Add(ex);
					}
					catch (Exception ex)
					{
						WriteMessage("An error has ocurred while trying to publish. Check the Sitecore log file for any exceptions.");
						exceptions.Add(ex);
						break;
					}
				}

				if (!success)
				{
					Environment.Exit(103);
				}

				var status = publishingService.Check(id);
				var message = string.Empty;

				while (!status.Complete)
				{
					if (!message.Equals(status.Message))
					{
						message = status.Message;

						WriteMessage(status.Message);
					}

					Thread.Sleep(1500);

					status = publishingService.Check(id);
				}

				WriteMessage(status.Message);
			}
		}

		static void WriteMessage(string format, params object[] args)
		{
			Console.Write(string.Format("[{0}] ", DateTime.Now.ToString("hh:mm:ss")));
			Console.WriteLine(format, args);
		}
	}
}
