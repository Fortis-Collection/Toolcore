namespace FortisCollections.Toolcore.Update
{
	public interface IPackageInstallLoggerFactory
	{
		IPackageInstallLogger Create(IPackageInstallTracker tracker);
	}
}
