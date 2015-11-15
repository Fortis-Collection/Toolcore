namespace FortisCollections.Toolcore.Publish
{
	public interface IPublishProgress
	{
		bool Complete { get; }
		long Processed { get; }
		string Message { get; }
	}
}
