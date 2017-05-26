using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Publishing;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FortisCollections.Toolcore.Publish
{
	public class Publisher : IPublisher
	{
		public string Publish(IPublisherOptions options)
		{
			using (new SecurityDisabler())
			{
				var publisherOptions = SetDefaults(options);
				var sourceDatabase = Factory.GetDatabase(publisherOptions.SourceDatabaseName);
				var languages = LanguageManager.GetLanguages(sourceDatabase).AsEnumerable();

				if (publisherOptions.LanguageNames != null && publisherOptions.LanguageNames.Length > 0)
				{
					languages = languages.Where(l => publisherOptions.LanguageNames.Contains(l.Name));
				}

				var targets = PublishManager.GetPublishingTargets(sourceDatabase).AsEnumerable();
				var targetDatabases = new List<Database>();

				if (publisherOptions.TargetNames != null && publisherOptions.TargetNames.Length > 0)
				{
					targets = targets.Where(t => publisherOptions.TargetNames.Contains(t.Name));
				}

				foreach (var target in targets)
				{
					targetDatabases.Add(Factory.GetDatabase(target["Target database"]));
				}

				var publishMode = ParsePublishMode(publisherOptions.PublishMode);

				var publishOptions = new PublishOptions(
					sourceDatabase,
					targetDatabases.First(),
					publishMode,
					languages.First(),
					DateTime.Now,
					publisherOptions.TargetNames.ToList())
				{
					Deep = publisherOptions.Deep,
					RootItem = sourceDatabase.Items[publisherOptions.RootItem]
				};

				var publisher = new Sitecore.Publishing.Publisher(publishOptions, languages);
				var job = publisher.PublishAsync();

				// Add publish to history

				return job.Name;
			}
		}

		public IPublisherOptions SetDefaults(IPublisherOptions publisherOptions)
		{
			return (new DefaultPublisherOptionsFactory()).Create(publisherOptions);
		}

		public PublishMode ParsePublishMode(string unparsedPublishMode)
		{
			PublishMode publishMode;

			if (!Enum.TryParse(unparsedPublishMode, out publishMode))
			{
				publishMode = PublishMode.Smart;
			}

			return publishMode;
		}
	}
}
