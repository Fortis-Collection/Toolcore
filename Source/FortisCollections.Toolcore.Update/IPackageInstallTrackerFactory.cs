namespace FortisCollections.Toolcore.Update
{
	public interface IPackageInstallTrackerFactory
	{
		IPackageInstallTracker Create(int totalCommands);
	}
}
