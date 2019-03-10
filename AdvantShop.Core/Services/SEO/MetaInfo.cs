//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;

namespace AdvantShop.SEO
{
    public class MetaInfo : ICloneable
    {
        public int MetaId { get; set; }
        public int ObjId { get; set; }
        public MetaType Type { get; set; }
        public string Title { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string H1 { get; set; }

        public MetaInfo()
        {
        }

        public MetaInfo(string str)
        {
            Title = str;
            MetaDescription = str;
            MetaKeywords = str;
            H1 = str;
        }

        public MetaInfo(int metaId, int objId, MetaType type, string title, string metaKeywords, string metaDescription,
            string h1)
        {
            MetaId = metaId;
            ObjId = objId;
            Type = type;
            Title = title;
            MetaKeywords = metaKeywords;
            MetaDescription = metaDescription;
            H1 = h1;
        }

        public object Clone()
        {
            // we dont need deep clone here
            return this.MemberwiseClone();
        }

    }
}
