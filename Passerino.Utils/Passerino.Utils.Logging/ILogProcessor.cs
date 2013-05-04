using System;

namespace Passerino.Utils.Logging
{
    public interface ILogProcessor
    {
        ILogProcessor SetSource(Type source);
        ILogEntry Debug(string message, params object[] args);
        ILogEntry Info(string message, params object[] args);
        ILogEntry Warn(string message, params object[] args);
        ILogEntry Error(string message, params object[] args);
        ILogEntry Fatal(string message, params object[] args);
    }
}