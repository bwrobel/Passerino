using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Passerino.Utils.Domain.IoC
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

        public static bool AssemblyVisible(Assembly assembly)
        {
            var customAttributes = assembly.GetCustomAttributes(typeof(AssemblyIoCVisibleAttribute), false);
            return customAttributes.Any(a => ((AssemblyIoCVisibleAttribute)a).Visible);
        }
    }
}
