using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Module1
{
    public class Module : IModule
    {
        public Assembly GetAssembly()
        {
            return Assembly.GetExecutingAssembly(); // Of gebruik een andere methode om de assembly te verkrijgen
        }
    }

}
