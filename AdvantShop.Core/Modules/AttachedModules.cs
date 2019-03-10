//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Diagnostics;

namespace AdvantShop.Core.Modules
{
    public class AttachedModules
    {
        private static List<Type> _allModules;
        private static List<Module> _activeModules;
        private static List<Type> _allTypes;
        private static bool _isLoaded;

        public static void LoadModules()
        {
            _allModules = new List<Type>();
            _activeModules = new List<Module>();

            var moduleType = typeof(IModuleBase);

            try
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(item => item.FullName.Contains("AdvantShop.Module")))
                {
                    try
                    {
                        var types = assembly.GetTypes().Where(t => t.GetInterface(moduleType.Name) != null && t.IsClass).ToList();
                        if (types.Count > 0)
                            _allModules.AddRange(types);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error("exception at loading module " + assembly.FullName, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("exception at global modules loading", ex);
                _isLoaded = true;
            }

            _activeModules = ModulesRepository.GetModulesFromDb().Where(m => m.IsInstall && m.Active).ToList();

            _isLoaded = true;
        }

        /// <summary>
        /// Get active modules
        /// </summary>
        /// <typeparam name="T">IModule interface</typeparam>
        /// <returns></returns>
        public static List<Type> GetModules<T>()
        {
            if (!_isLoaded || _activeModules == null || _allModules == null)
                LoadModules();

            var type = typeof(T);

            var modules =
                _allModules.Where(
                    item =>
                        type.IsAssignableFrom(item) &&
                        _activeModules.Any(
                            m => String.Equals(item.Name, m.StringId, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

            return modules;
        }

        /// <summary>
        /// Get module by id
        /// </summary>
        /// <param name="stringId">module id</param>
        /// <param name="active">if true return active module</param>
        /// <returns></returns>
        public static Type GetModuleById(string stringId, bool active = false)
        {
            if (!_isLoaded || _activeModules == null || _allModules == null)
                LoadModules();

            var module =
                _allModules.FirstOrDefault(
                    item => String.Equals(item.Name, stringId, StringComparison.OrdinalIgnoreCase));

            if (active)
                return module != null &&
                       _activeModules.Any(m => String.Equals(module.Name, m.StringId, StringComparison.OrdinalIgnoreCase))
                    ? module
                    : null;

            return module;
        }

        public static List<Type> GetCore<T>()
        {
            var type = typeof(T);
            _allTypes = _allTypes ?? new List<Type>();
            var res = _allTypes.Where(item => item.IsClass && type.IsAssignableFrom(item)).ToList();
            if (res.Any())
                return res;

            var items = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(item => item.FullName.StartsWith("AdvantShop") && !item.FullName.Contains("AdvantShop.Module")).SelectMany(s => s.GetTypes())
                        .Where(item => item.IsClass && !item.IsAbstract && type.IsAssignableFrom(item));

            if (items != null && items.Any())
            {
                _allTypes.AddRange(items);
                return items.ToList();
            }
            else
                return new List<Type>();
        }

        public static List<Type> GetModules()
        {
            if (!_isLoaded || _allModules == null)
                LoadModules();

            return _allModules;
        }
    }
}