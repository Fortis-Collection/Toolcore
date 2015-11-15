using System.Web.Services;

namespace FortisCollections.Toolcore.Update.Service
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	public class PackageInstall : WebService
	{
		public IPackageInstaller PackageInstaller = new PackageInstaller(
			new ActivePackageInstallTracker(),
			new PackageInstallTrackerFactory(
				new ActivePackageInstallTracker()
			),
			new PackageInstallLoggerFactory(
				new SitecoreLoggerFactory()
			)
		);

		[WebMethod(Description = "Starts the installation of a Sitecore Update Package")]
		public void InstallPackage(string packagePath)
		{
			PackageInstaller.Install(packagePath);
		}

		[WebMethod(Description = "Check the progress of the package installation")]
		public Progress Check()
		{
			var progress = PackageInstaller.CheckProgress();

			return new Progress
			{
				LastCommandProcessed = progress.LastCommandProcessed,
				PercentageComplete = progress.PercentageComplete
			};
		}
	}
}
