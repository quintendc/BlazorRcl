using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BlazorAppWithRcl.Helpers
{
    public class AssemblyScanner
    {
        public List<Assembly> GetAssembliesWithModule()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var assemblies = Directory.GetFiles(directory, "*.dll")
                                      .Select(Assembly.LoadFrom)
                                      .ToList();

            var relevantAssemblies = new List<Assembly>();

            foreach (var assembly in assemblies)
            {
                try
                {
                    // Try loading all classes in the assembly that implement IModule
                    var moduleTypes = GetLoadableTypes(assembly)
                        .Where(type => type.IsClass &&
                                       type.GetInterfaces().Contains(typeof(IModule)))
                        .ToList();

                    // If at least one class implements IModule, add this assembly to the list
                    if (moduleTypes.Any())
                    {
                        relevantAssemblies.Add(assembly);
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // If an error occurs while loading types, display the error messages
                    foreach (var loaderException in ex.LoaderExceptions)
                    {
                        Console.WriteLine(loaderException.Message);
                    }
                }
            }

            return relevantAssemblies;
        }

        // This method handles issues with loading types in RCLs during runtime.
        private IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(type => type != null);
            }
        }

        public void FindModulesInAssemblies(List<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var moduleTypes = assembly.GetTypes()
                    .Where(type => type.IsClass &&
                                   type.GetInterfaces().Contains(typeof(IModule)))
                    .ToList();

                foreach (var moduleType in moduleTypes)
                {
                    var moduleInstance = (IModule)Activator.CreateInstance(moduleType);
                    var moduleAssembly = moduleInstance.GetAssembly();

                    Console.WriteLine($"Found in assembly {assembly.FullName}: {moduleType.FullName}");
                    Console.WriteLine($"Assembly of the module: {moduleAssembly.FullName}");
                }
            }
        }
    }
}
