using log4net;

namespace FortisCollections.Toolcore.Update
{
	public class SitecoreLoggerFactory : ILog4NetFactory
	{
		public ILog Create()
		{
			return LogManager.GetLogger("root");
			//XmlConfigurator.Configure((XmlElement)ConfigurationManager.GetSection("log4net"));
		}
	}
}
