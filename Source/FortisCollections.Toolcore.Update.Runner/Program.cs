using CommandLine;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FortisCollections.Toolcore.Update.Runner
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

			RunPackageInstall(options.PackagePath, options.SitecoreUrl);
		}

		static void RunPackageInstall(string packagePath, string sitecoreUrl)
		{
			var hostUrl = sitecoreUrl.LastIndexOf("/") != sitecoreUrl.Length - 1 ? sitecoreUrl + "/" : sitecoreUrl;
			var serviceUrl = string.Concat(hostUrl, Properties.Settings.Default.ServiceFolder, Properties.Settings.Default.ServiceFileName);

			using (var packageInstallService = new PackageInstall.PackageInstall())
			{
				packageInstallService.Url = serviceUrl;
				packageInstallService.Timeout = Convert.ToInt32(timeout.TotalMilliseconds);

				WriteMessage("Initializing package install process | Package: {0} | Sitecore URL: {1}", packagePath, serviceUrl);

				var success = false;
				var exceptions = new List<Exception>();

				for (int retry = 0; retry < maxRetries && !success; retry++)
				{
					try
					{
						if (retry > 0)
						{
							WriteMessage("Waiting {0}m {1}s before attempting install again.", retryInterval.Minutes, retryInterval.Seconds);
							Thread.Sleep(retryInterval);
						}
						else
						{
							WriteMessage("Starting package installation | Max Timeout: {0}m {1}s | Max Retries: {2}", timeout.Minutes, timeout.Seconds, maxRetries);
						}

						packageInstallService.InstallPackageAsync(packagePath);

						var progress = packageInstallService.Check();

                        // The web service will sometimes report 0% complete instead of 100% shortly after a package installation finishes.
                        // Since we only check the progress once every 5 seconds, it is possible we'll miss the window where the service 
                        // is reporting 100% complete.  This would result in a non-terminating loop.
                        // Therefore, we also track the maximum completion percentage seen thus far.
                        // If the current completion percentage ever drops to 0, but we've already seen higher percentages, 
                        // we know the installation is actually complete.
                        var maximumProgress = progress.PercentageComplete;

						WriteMessage("Package install progress {0}%", progress.PercentageComplete);

                        // Stop if we see 100% complete, or if we see PercentageComplete at 0% with a non-zero MaximumProgress.
						while (progress.PercentageComplete < 100 && !(Math.Abs(progress.PercentageComplete) < 0.1 && maximumProgress > 0))
						{
							Thread.Sleep(TimeSpan.FromSeconds(5));
							progress = packageInstallService.Check();
                            
						    if (progress.PercentageComplete > maximumProgress)
						    {
						        maximumProgress = progress.PercentageComplete;
						    }

							WriteMessage("Package install progress {0}%", progress.PercentageComplete);
						}

						success = true;
					}
					catch (TimeoutException ex)
					{
						WriteMessage("Failed to contact Sitecore instance, retrying ({0}).", retry + 1);
						exceptions.Add(ex);
					}
					catch (Exception ex)
					{
						WriteMessage("An error has ocurred while trying to install the update package. Check the Sitecore log file for any exceptions.");
						exceptions.Add(ex);
						break;
					}
				}

				if (!success)
				{
					Environment.Exit(103);
				}

				WriteMessage("Update package installed successfully.");
			}
		}

		static void WriteMessage(string format, params object[] args)
		{
			Console.Write(string.Format("[{0}] ", DateTime.Now.ToString("hh:mm:ss")));
			Console.WriteLine(format, args);
		}
	}
}
