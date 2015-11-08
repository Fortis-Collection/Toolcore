using log4net;
using log4net.spi;
using System;

namespace FortisCollections.Toolcore.Update
{
	public class PackageInstallLogger : IPackageInstallLogger
	{
		protected readonly IPackageInstallTracker Tracker;
		protected readonly ILog Log;

        public PackageInstallLogger(
			IPackageInstallTracker tracker,
			ILog log)
		{
			Tracker = tracker;
			Log = log;
		}

		public bool IsDebugEnabled
		{
			get
			{
				return true;
			}
		}

		public bool IsErrorEnabled
		{
			get
			{
				return true;
			}
		}

		public bool IsFatalEnabled
		{
			get
			{
				return true;
			}
		}

		public bool IsInfoEnabled
		{
			get
			{
				return true;
			}
		}

		public bool IsWarnEnabled
		{
			get
			{
				return true;
			}
		}

		ILogger ILoggerWrapper.Logger
		{
			get
			{
				return Log.Logger;
			}
		}

		public void Debug(object message)
		{
			Log.Debug(message);
			Track(message);
		}

		public void Debug(object message, Exception t)
		{
			Log.Debug(message, t);
			Track(message);
		}

		public void Error(object message)
		{
			Log.Error(message);
			Track(message);
		}

		public void Error(object message, Exception t)
		{
			Log.Error(message, t);
			Track(message);
		}

		public void Fatal(object message)
		{
			Log.Fatal(message);
			Track(message);
		}

		public void Fatal(object message, Exception t)
		{
			Log.Fatal(message, t);
			Track(message);
		}

		public void Info(object message)
		{
			Log.Info(message);
			Track(message);
		}

		public void Info(object message, Exception t)
		{
			Log.Info(message, t);
			Track(message);
		}

		public void Warn(object message)
		{
			Log.Warn(message);
			Track(message);
		}

		public void Warn(object message, Exception t)
		{
			Log.Warn(message, t);
			Track(message);
		}

		public void Track(object message)
		{
			Tracker.TrackCommand((message as string) ?? "NULL");
		}
	}
}
