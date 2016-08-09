using PoGo.NecroBot.Logic.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.CLI.Plugin
{
    /// <summary>
    /// Plugin loader.
    /// </summary>
    class PluginManager
    { 
        // Private vars.
        private PluginInitializerInfo _initInfo;
        private List<INecroPlugin> _plugins;
        private Type _pluginType = typeof(INecroPlugin);


        /// <summary>
        /// Plugin loader.
        /// </summary>
        /// <param name="initInfo"></param>
        public PluginManager(PluginInitializerInfo initInfo)
        {
            _initInfo = initInfo;
            _plugins = new List<INecroPlugin>();
        }


        /// <summary>
        /// Load all the plugins found in the Necro root folder.
        /// </summary>
        public void InitPlugins()
        {
            // Get all the plugin DLLs.
            string pluginDir = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
            if (!Directory.Exists(pluginDir))
                Directory.CreateDirectory(pluginDir);

            var dllFiles = Directory.GetFiles(pluginDir, "*.dll");

            // Attempt to load all the assemblies from the DLLs.
            List<Assembly> assemblies = new List<Assembly>();
            foreach (string dll in dllFiles)
            {
                var assembly = AttemptAssemblyLoad(dll);
                if (assembly != null)
                    assemblies.Add(assembly);
            }

            // Attemp to load all plugins.
            LoadPlugins(assemblies);
        }


        /// <summary>
        /// Attempt to load an assembly.
        /// </summary>
        /// <param name="dll">List of DLLs</param>
        /// <returns>Assembly if found, null if not.</returns>
        private Assembly AttemptAssemblyLoad(string dll)
        {
            try
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dll);
                Assembly assembly = Assembly.Load(an);
                return assembly;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Load plugins from an assembly.
        /// </summary>
        /// <param name="assemblies">List of assemblies.</param>
        private void LoadPlugins(List<Assembly> assemblies)
        {
            
            // Get all the types from this assembly that match our plugin type.
            ICollection<Type> pluginTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                            continue;

                        if (type.GetInterface(_pluginType.FullName) != null)
                            pluginTypes.Add(type);
                    }
                }
                catch
                {
                    Logger.Write("Could not load assembly: " + assembly.GetName().FullName, LogLevel.Error);
                }
            }

            // Iterate through them all and create an instance of the plugin.
            foreach (Type type in pluginTypes)
            {
                try
                {
                    INecroPlugin plugin = (INecroPlugin)Activator.CreateInstance(type);
                    plugin.Initialize(_initInfo);
                    _plugins.Add(plugin);
                }
                catch
                {
                    Logger.Write("Could not load plugin: " + type.FullName, LogLevel.Error);
                }
            }
        }
    }
}
