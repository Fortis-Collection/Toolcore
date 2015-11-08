namespace FortisCollections.Toolcore.Update
{
	public class PackageInstallProgress : IPackageInstallProgress
	{
		public double PercentageComplete { get; set; }
		public int LastCommandProcessed { get; set; }
		public IPackageInstallError Error { get; set; }
	}
}
