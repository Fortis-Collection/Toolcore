namespace FortisCollections.Toolcore.Publish
{
	public interface IPublisher
	{
		string Publish(string sourceDatabaseName, string[] targetNames = null, string[] languageNames = null);
    }
}
