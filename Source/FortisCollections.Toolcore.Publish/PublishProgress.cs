namespace FortisCollections.Toolcore.Publish
{
	public class PublishProgress : IPublishProgress
	{
		public bool Complete { get; set; }
		public long Processed { get; set; }
		public string Message { get; set; }
	}
}
