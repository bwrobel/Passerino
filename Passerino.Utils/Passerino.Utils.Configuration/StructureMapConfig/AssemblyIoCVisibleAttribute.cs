using System;
using System.Runtime.InteropServices;

namespace Passerino.Utils.Configuration.StructureMapConfig
{
    [ComVisible(true)]
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public class AssemblyIoCVisibleAttribute : Attribute
    {
        public bool Visible { get; private set; }

        public AssemblyIoCVisibleAttribute(bool visible)
        {
            Visible = visible;
        }
    }
}
