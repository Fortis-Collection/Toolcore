using CommandLine;

namespace FortisCollections.Toolcore.Publish.Runner
{
	public class Options
	{
		[Option('s', "sourceDatabaseName", Required = true)]
		public string SourceDatabaseName { get; set; }
		[OptionArray('t', "targetDatabaseNames")]
		public string[] TargetDatabaseNames { get; set; }
		[OptionArray('l', "languageNames")]
		public string[] LanguageNames { get; set; }
		[Option('d', "deep")]
		public bool Deep { get; set; }
		[Option('p', "publishMode")]
		public string PublishMode { get; set; }
		[Option('r', "rootItem")]
		public string RootItem { get; set; }
		[Option('u', "sitecoreUrl", Required = true)]
		public string SitecoreUrl { get; set; }
		[Option('o', "timeout")]
		public int Timeout { get; set; }
		[Option('m', "maxRetries")]
		public int MaxRetries { get; set; }
	}
}
