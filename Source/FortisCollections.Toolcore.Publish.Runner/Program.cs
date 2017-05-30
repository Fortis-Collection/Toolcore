using CommandLine;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using FortisCollections.Toolcore.Publish.Runner.Publishing;

namespace FortisCollections.Toolcore.Publish.Runner
{
	class Program
	{
		private static int maxRetries = Properties.Settings.Default.MaxRetries;
		private static TimeSpan timeout = Properties.Settings.Default.Timeout;
		private static TimeSpan retryInterval = Properties.Settings.Default.RetryInterval;

		static void Main(string[] args)
		{
			WriteMessage("------------------------");
			WriteMessage("Toolcore - Publish (2.0)");
			WriteMessage("------------------------");

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

			RunPublish(options);
		}

		static void RunPublish(Options options)
		{
			var sitecoreUrl = options.SitecoreUrl;
			var targets = options.TargetDatabaseNames ?? new string[] { };
			var languages = options.LanguageNames ?? new string[] { };
			var sourceDatabaseName = options.SourceDatabaseName;
			var deep = options.Deep;
			var rootItem = options.RootItem;
			var publishMode = options.PublishMode;
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
				WriteMessage(string.Format("	Deep: {0}", deep));
				WriteMessage(string.Format("	Root Item: {0}", rootItem ?? "/sitecore"));
				WriteMessage(string.Format("	Publish Mode: {0}", publishMode ?? "Smart"));

				var id = string.Empty;
				var success = false;
				var exceptions = new List<Exception>();
				var publishOptions = new PublishOptions
				{
					Deep = deep,
					LanguageNames = languages,
					PublishMode = publishMode,
					RootItem = rootItem,
					SourceDatabaseName = sourceDatabaseName,
					TargetNames = targets
				};

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

						id = publishingService.Publish(publishOptions);
						
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
			Console.Write(string.Format("[{0}] ", DateTime.Now.ToString("HH:mm:ss")));
			Console.WriteLine(format, args);
		}
	}
}
