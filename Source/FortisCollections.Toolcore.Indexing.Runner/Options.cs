using CommandLine;

namespace FortisCollections.Toolcore.Indexing.Runner
{
	public class Options
	{
		[Option('i', "indexNames")]
		public string[] IndexNames { get; set; }
		[Option('u', "sitecoreUrl", Required = true)]
		public string SitecoreUrl { get; set; }
		[Option('o', "timeout")]
		public int Timeout { get; set; }
		[Option('m', "maxRetries")]
		public int MaxRetries { get; set; }
	}
}
