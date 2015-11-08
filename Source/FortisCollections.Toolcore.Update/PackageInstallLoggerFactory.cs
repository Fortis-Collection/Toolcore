using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortisCollections.Toolcore.Update
{
	public class PackageInstallLoggerFactory : IPackageInstallLoggerFactory
	{
		protected readonly ILog4NetFactory LoggerFactory;

        public PackageInstallLoggerFactory(
			ILog4NetFactory loggerFactory)
		{
			LoggerFactory = loggerFactory;
		}

		public IPackageInstallLogger Create(IPackageInstallTracker tracker)
		{
			return new PackageInstallLogger(
				tracker,
				LoggerFactory.Create());
		}
	}
}
