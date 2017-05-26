using System;

namespace FortisCollections.Toolcore.Publish
{
	internal class PublisherOptions : IPublisherOptions
	{
		public bool Deep { get; set; }
		public string[] LanguageNames { get; set; }
		public string PublishMode { get; set; }
		public string RootItem { get; set; }
		public string SourceDatabaseName { get; set; }
		public string[] TargetNames { get; set; }
	}
}