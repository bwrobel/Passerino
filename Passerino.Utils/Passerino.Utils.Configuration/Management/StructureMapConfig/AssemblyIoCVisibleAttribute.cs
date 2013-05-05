using System;
using System.Runtime.InteropServices;

namespace Passerino.Utils.Configuration.Management.StructureMapConfig
{
    [ComVisible(true)]
    [AttributeUsageAttribute(AttributeTargets.Assembly, Inherited = false)]
    public class AssemblyIoCVisibleAttribute : Attribute
    {
        public bool Visible { get; private set; }

        public AssemblyIoCVisibleAttribute(bool visible)
        {
            Visible = visible;
        }
    }
}
