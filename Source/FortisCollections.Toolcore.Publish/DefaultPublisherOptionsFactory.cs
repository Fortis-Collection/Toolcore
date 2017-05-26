namespace FortisCollections.Toolcore.Publish
{
	public class DefaultPublisherOptionsFactory
	{
		private const bool deep = true;
		private const string publishMode = "smart";
		private const string rootItem = "/sitecore";

		public IPublisherOptions Create()
		{
			return new PublisherOptions
			{
				Deep = deep,
				PublishMode = publishMode,
				RootItem = rootItem
			};
		}

		public IPublisherOptions Create(IPublisherOptions publisherOptions)
		{
			var defaultPublisherOptions = Create();

			if (string.IsNullOrWhiteSpace(publisherOptions.PublishMode))
			{
				publisherOptions.PublishMode = defaultPublisherOptions.PublishMode;
			}

			if (string.IsNullOrWhiteSpace(publisherOptions.RootItem))
			{
				publisherOptions.RootItem = defaultPublisherOptions.RootItem;
			}

			return publisherOptions;
		}
	}
}
