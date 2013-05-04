using System;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace Passerino.Utils.RemoteLoggingSink.WinService
{
    public class Log4NetRemoteLoggingSink : MarshalByRefObject, RemotingAppender.IRemoteLoggingSink
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Log4NetRemoteLoggingSink));

        public void LogEvents(LoggingEvent[] events)
        {
            var fs = new FileStream(@"c:\temp\XXX.txt",
            FileMode.OpenOrCreate, FileAccess.Write);
            var m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine("WindowsService: Service Started \n");
            m_streamWriter.Flush();
            m_streamWriter.Close();

            if (events == null) return;
            lock (this)
            {
                foreach (var le in events)
                {
                    _log.Logger.Log(le);
                }
            }
        }
    }
}