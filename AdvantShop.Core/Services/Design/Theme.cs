//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Design
{
    [Serializable]
    public class Theme
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("color")]
        public string Color { get; set; }


        [XmlAttribute("previewImage")]
        public string PreviewImage { get; set; }

        [XmlArray("Names")]
        [XmlArrayItem("Name", typeof(ThemeName))]
        public List<ThemeName> Names { get; set; }

        public string Title { get; set; }

        public string Source { get; set; }


        /// <summary>
        /// Checks if the provided object is equal to the current Theme
        /// </summary>
        /// <param name="obj">Object to compare to the current Theme</param>
        /// <returns>True if equal, false if not</returns>
        public override bool Equals(object obj)
        {
            var theme = obj as Theme;
            return Equals(theme);
        }

        /// <summary>
        /// Returns an identifier for this instance
        /// </summary>
        public override int GetHashCode()
        {
            return (Name + Title).GetHashCode();
        }

        /// <summary>
        /// Checks if the provided Theme is equal to the current theme
        /// </summary>
        /// <param name="themeToCompareTo">Theme to compare to the current theme</param>
        /// <returns>True if equal, false if not</returns>
        public bool Equals(Theme themeToCompareTo)
        {
            if (themeToCompareTo == null) return false;

            if (themeToCompareTo.Name.IsNullOrEmpty() || themeToCompareTo.Title.IsNullOrEmpty())
                return false;
            
            return Name.Equals(themeToCompareTo.Name) && Title.Equals(themeToCompareTo.Title);
        }

    }

    [Serializable]
    public class ThemeName
    {
        [XmlAttribute("lang")]
        public string Lang { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}