using log4net;

namespace FortisCollections.Toolcore.Update
{
	public interface ILog4NetFactory
	{
		ILog Create();
	}
}
