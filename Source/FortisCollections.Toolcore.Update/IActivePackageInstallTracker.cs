namespace FortisCollections.Toolcore.Update
{
	public interface IActivePackageInstallTracker
	{
		IPackageInstallTracker Tracker { get; set; }
	}
}
