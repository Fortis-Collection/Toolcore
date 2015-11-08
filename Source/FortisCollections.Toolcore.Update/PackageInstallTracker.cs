using System;
using System.Collections.Generic;
using System.Linq;

namespace FortisCollections.Toolcore.Update
{
	public class PackageInstallTracker : IPackageInstallTracker
	{
		public int TotalCommands { get; set; }
		public List<string> CommandMessages { get; set; }

		public void TrackCommand(string commandMessage)
		{
			CommandMessages.Add(commandMessage);
		}

		public int CommandsProcessed { get { return CommandMessages.Count(); } }
		public double PercentageComplete { get { return Math.Round(((double)CommandsProcessed * 100) / TotalCommands); } }
	}
}
