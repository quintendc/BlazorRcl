using Core.Interfaces;
using System.Reflection;

namespace Core.BaseClasses
{
    public abstract class ModuleBase : IModule
    {
        public Assembly GetAssembly()
        {
            return Assembly.GetExecutingAssembly(); // return assembly
        }
    }
}
