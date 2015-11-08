namespace FortisCollections.Toolcore.Update
{
	public interface IPackageInstaller
	{
		IPackageInstallInfo Install(string path);
		IPackageInstallProgress CheckProgress();
	}
}
