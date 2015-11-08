using Sitecore.Update;
using Sitecore.Update.Installer;

namespace FortisCollections.Toolcore.Update
{
	public class PackageInstaller : IPackageInstaller
	{
		protected IPackageInstallTracker Tracker;
		protected readonly IPackageInstallTrackerFactory TrackerFactory;
		protected readonly IPackageInstallLoggerFactory LoggerFactory;

		public PackageInstaller(
			IPackageInstallTrackerFactory trackerFactory,
			IPackageInstallLoggerFactory loggerFactory)
		{
			TrackerFactory = trackerFactory;
			LoggerFactory = loggerFactory;
		}

		public IPackageInstallProgress CheckProgress()
		{
			if (Tracker == null)
			{
				return new PackageInstallProgress
				{
					Error = new PackageInstallError
					{
						Message = "No update package is currently being tracked"
					}
				};
			}

			return new PackageInstallProgress
			{
				LastCommandProcessed = Tracker.CommandsProcessed,
				PercentageComplete = Tracker.PercentageComplete
			};
		}

		public IPackageInstallInfo Install(string path)
		{
			var packageMetaData = UpdateHelper.LoadMetadata(path);

			if (packageMetaData == null)
			{
				return new PackageInstallInfo
				{
					Error = new PackageInstallError
					{
						Message = "Package not found"
					}
				};
			}

			Tracker = TrackerFactory.Create(packageMetaData.CommandsCount);
			var logger = LoggerFactory.Create(Tracker);
			var packageInstallationInfo = new PackageInstallationInfo
			{
				Action = Sitecore.Update.Installer.Utils.UpgradeAction.Upgrade,
				Mode = Sitecore.Update.Utils.InstallMode.Install,
				Path = path
			};
			var historyPath = string.Empty;
			var entries = UpdateHelper.Install(packageInstallationInfo, logger, out historyPath);
			var installer = new DiffInstaller(packageInstallationInfo.Action);

			installer.ExecutePostInstallationInstructions(packageInstallationInfo.Path, historyPath, packageInstallationInfo.Mode, packageMetaData, logger, ref entries);

			Tracker = null;

			return new PackageInstallInfo
			{
				Id = historyPath,
				CommandCount = packageMetaData.CommandsCount
			};
		}
	}
}
