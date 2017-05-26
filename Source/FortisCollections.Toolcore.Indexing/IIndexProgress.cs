namespace FortisCollections.Toolcore.Indexing
{
	public interface IIndexProgress
	{
		string Id { get; set; }
		bool Complete { get; }
		string Message { get; }
		long Processed { get; }
	}
}