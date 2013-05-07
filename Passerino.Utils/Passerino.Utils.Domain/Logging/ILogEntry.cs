using System;

namespace Passerino.Utils.Domain.Logging
{
    public interface ILogEntry
    {
        ILogEntry WithException(Exception ex);
        void Proceed();
    }
}