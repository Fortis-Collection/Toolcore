using System.Collections.Generic;

namespace FortisCollections.Toolcore.Update
{
	public class PackageInstallTrackerFactory : IPackageInstallTrackerFactory
	{
		protected readonly IActivePackageInstallTracker ActivePackageInstallTracker;

		public PackageInstallTrackerFactory(
			IActivePackageInstallTracker activePackageInstallTracker)
		{
			ActivePackageInstallTracker = activePackageInstallTracker;
        }

		public IPackageInstallTracker Create(int totalCommands)
		{
			ActivePackageInstallTracker.Tracker = new PackageInstallTracker
			{
				CommandMessages = new List<string>(),
				TotalCommands = totalCommands
			};

			return ActivePackageInstallTracker.Tracker;
		}
	}
}
