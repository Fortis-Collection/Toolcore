using System.Collections.Generic;

namespace FortisCollections.Toolcore.Update
{
	public class PackageInstallTrackerFactory : IPackageInstallTrackerFactory
	{
		public IPackageInstallTracker Create(int totalCommands)
		{
			return new PackageInstallTracker
			{
				CommandMessages = new List<string>(),
				TotalCommands = totalCommands
			};
		}
	}
}
