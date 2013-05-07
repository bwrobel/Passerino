using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passerino.Utils.Domain.Logging
{
    public interface ILogFactory
    {
        ILog New(Type source);
    }
}
