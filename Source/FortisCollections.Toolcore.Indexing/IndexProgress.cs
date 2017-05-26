namespace FortisCollections.Toolcore.Indexing
{
	public class IndexProgress : IIndexProgress
	{
		public string Id { get; set; }
		public bool Complete { get; set; }
		public long Processed { get; set; }
		public string Message { get; set; }
	}
}
