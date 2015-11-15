using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Publishing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FortisCollections.Toolcore.Publish
{
	public class Publisher : IPublisher
	{
		public string Publish(string sourceDatabaseName, string[] targetNames = null, string[] languageNames = null)
		{
			var sourceDatabase = Factory.GetDatabase(sourceDatabaseName);
			var languages = LanguageManager.GetLanguages(sourceDatabase).AsEnumerable();

			if (languageNames != null && languageNames.Length > 0)
			{
				languages = languages.Where(l => languageNames.Contains(l.Name));
			}

			var targets = PublishManager.GetPublishingTargets(sourceDatabase).AsEnumerable();
			var targetDatabases = new List<Database>();

			if (targetNames != null && targetNames.Length > 0)
			{
				targets = targets.Where(t => targetNames.Contains(t.Name));
			}

			foreach (var target in targets)
			{
				targetDatabases.Add(Factory.GetDatabase(target["Target database"]));
			}

			var publishOptions = new PublishOptions(
				sourceDatabase,
				targetDatabases.First(),
				PublishMode.Smart,
				languages.First(),
				DateTime.Now,
				targetNames.ToList())
			{
				Deep = true,
				RootItem = sourceDatabase.Items["/sitecore"]
			};

			var publisher = new Sitecore.Publishing.Publisher(publishOptions, languages);
			var job = publisher.PublishAsync();

			return job.Name;
		}
	}
}
