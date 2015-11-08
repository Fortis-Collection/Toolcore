namespace FortisCollections.Toolcore.Update
{
	public class PackageInstallInfo : IPackageInstallInfo
	{
		public string Id { get; set; }
		public int CommandCount { get; set; }
		public IPackageInstallError Error { get; set; }
	}
}
