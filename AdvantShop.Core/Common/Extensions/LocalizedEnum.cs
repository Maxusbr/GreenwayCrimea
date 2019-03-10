//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Linq;
//using System.Resources;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Helpers;
//using Resources;

namespace AdvantShop
{
    /// <summary>
    /// Summary description for LocalizedEnum
    /// </summary>
    public class LocalizedEnum<T> // where T : System.Enum :-( can't
    {
        public T Value { get; private set; }

        public string Name { get; private set; }

        public int IntValue { get { return SQLDataHelper.GetInt(Value); } }
        // get the resource manager according to .config info
        //private static readonly ResourceManager ResourceManager = new ResourceManager(typeof (Resource));

        public LocalizedEnum(T value)
        {
            Type type = typeof (T);

            // Since generic constrains for Enum are not supported
            // by design, we'll check it at run-time...
            if (type.BaseType != typeof (Enum))
                throw new ArgumentException("T must be an enumeration.");

            Name = LocalizationService.GetResource(string.Format("Enums_{0}_{1}", type.Name, value)); //ResourceManager.GetString(string.Format("Enums_{0}_{1}", type.Name, value));
            Value = value;
        }

        // we don't want no one creating this directly
        private LocalizedEnum(string name, T value)
        {
            Name = name;
            Value = value;
        }
        
        public static ReadOnlyCollection<LocalizedEnum<T>> GetValues()
        {
            Type type = typeof (T);

            // Since generic constrains for Enum are not supported
            // by design, we'll check it at run-time...
            if (type.BaseType != typeof (Enum))
                throw new ArgumentException("T must be an enumeration.");

            return ((T[]) Enum.GetValues(type)).Select(
                (val) =>
                new LocalizedEnum<T>(
                    LocalizationService.GetResource(string.Format("Enums_{0}_{1}", type.Name, val.ToString())), val)).ToList()
                    //ResourceManager.GetString(string.Format("Enums_{0}_{1}", type.Name, val.ToString())), val)).ToList()
                .AsReadOnly();
        }
        /*
        public static string GetName(T value)
        {
            Type type = typeof (T);
            if (type.BaseType != typeof (Enum))
                throw new ArgumentException("T must be an enumeration.");
            return LocalizationService.GetResource(string.Format("Enums_{0}_{1}", type.Name)); //ResourceManager.GetString(string.Format("Enums_{0}_{1}", type.Name));
        }
        */
    }
}