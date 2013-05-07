
using System;

namespace Passerino.Utils.Extensions.DateTimeExtensions
{
    public interface IDateTimeExtension
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }

    public class DateTimeExtension : IDateTimeExtension
    {
        public DateTime Now { get { return DateTime.Now; } }
        public DateTime UtcNow { get { return DateTime.UtcNow; } }
    }
}
