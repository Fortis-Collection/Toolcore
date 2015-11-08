namespace FortisCollections.Toolcore.Update
{
	public interface IPackageInstallProgress
	{
		double PercentageComplete { get; }
		int LastCommandProcessed { get; }
		IPackageInstallError Error { get; }
	}
}
