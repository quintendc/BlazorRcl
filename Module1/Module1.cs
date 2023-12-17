using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Module1
{
    public class Module1 : IModule
    {
        // Voeg hier je specifieke implementatie toe
        public Assembly GetModuleAssembly()
        {
            return typeof(Module1).Assembly;
        }
    }

}
