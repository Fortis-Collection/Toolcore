using Sitecore.Update;
using Sitecore.Update.Installer;

namespace FortisCollections.Toolcore.Update
{
	public class PackageInstaller : IPackageInstaller
	{
		protected readonly IActivePackageInstallTracker ActiveTracker;
        protected readonly IPackageInstallTrackerFactory TrackerFactory;
		protected readonly IPackageInstallLoggerFactory LoggerFactory;

		public PackageInstaller(
			IActivePackageInstallTracker activeTracker,
			IPackageInstallTrackerFactory trackerFactory,
			IPackageInstallLoggerFactory loggerFactory)
		{
			ActiveTracker = activeTracker;
			TrackerFactory = trackerFactory;
			LoggerFactory = loggerFactory;
		}

		public IPackageInstallProgress CheckProgress()
		{
			if (ActiveTracker.Tracker == null)
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
				LastCommandProcessed = ActiveTracker.Tracker.CommandsProcessed,
				PercentageComplete = ActiveTracker.Tracker.PercentageComplete
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

			var tracker = TrackerFactory.Create(packageMetaData.CommandsCount);
			var logger = LoggerFactory.Create(tracker);
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

			ActiveTracker.Tracker = null;

			return new PackageInstallInfo
			{
				Id = historyPath,
				CommandCount = packageMetaData.CommandsCount
			};
		}
	}
}
