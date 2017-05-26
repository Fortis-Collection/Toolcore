namespace FortisCollections.Toolcore.Publish
{
	public interface IPublisherOptions
	{
		string SourceDatabaseName { get; set; }
		string[] TargetNames { get; set; }
		string[] LanguageNames { get; set; }
		bool Deep { get; set; }
		string RootItem { get; set; }
		string PublishMode { get; set; }
	}
}
