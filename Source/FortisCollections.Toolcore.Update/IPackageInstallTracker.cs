namespace FortisCollections.Toolcore.Update
{
	public interface IPackageInstallTracker
	{
		void TrackCommand(string commandMessage);
		int CommandsProcessed { get; }
		double PercentageComplete { get; }
	}
}
