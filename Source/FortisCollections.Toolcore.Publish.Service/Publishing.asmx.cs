using System.Web.Services;

namespace FortisCollections.Toolcore.Publish.Service
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	public class Publishing : WebService
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns>An ID for checking the progress of the publish</returns>
		[WebMethod(Description = "Publishes the master database to one or more targets.")]
		public string Publish(PublishOptions options)
		{
			var publisher = new Publisher();

			return publisher.Publish(options);
		}

		[WebMethod(Description = "Checks publish progress for a given ID")]
		public Progress Check(string id)
		{
			var tracker = new PublisherTracker();
			var progress = tracker.Check(id);

			return new Progress
			{
				Complete = progress.Complete,
				Message = progress.Message,
				Processed = progress.Processed
			};
		}
    }
}
