namespace FortisCollections.Toolcore.Update
{
	public interface IPackageInstallInfo
	{
		string Id { get; }
		int CommandCount { get; }
		IPackageInstallError Error { get; }
	}
}
