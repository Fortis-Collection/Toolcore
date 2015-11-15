namespace FortisCollections.Toolcore.Update
{
	public class ActivePackageInstallTracker : IActivePackageInstallTracker
	{
		public static IPackageInstallTracker ActiveTracker { get; set; }

		public IPackageInstallTracker Tracker
		{
			get
			{
				return ActiveTracker;
			}

			set
			{
				ActiveTracker = value;
			}
		}
	}
}
