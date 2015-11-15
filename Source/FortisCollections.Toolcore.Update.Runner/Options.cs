using CommandLine;

namespace FortisCollections.Toolcore.Update.Runner
{
	public class Options
	{
		[Option('p', "packagePath", Required = true)]
		public string PackagePath { get; set; }
		[Option('u', "sitecoreUrl", Required = true)]
		public string SitecoreUrl { get; set; }
		[Option('o', "timeout")]
		public int Timeout { get; set; }
		[Option('m', "maxRetries")]
		public int MaxRetries { get; set; }
	}
}
