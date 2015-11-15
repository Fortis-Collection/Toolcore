namespace FortisCollections.Toolcore.Publish
{
	public interface IPublisherTracker
	{
		IPublishProgress Check(string id);
	}
}
