//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.CMS
{
    public class BreadCrumbs
    {
        public string Name;
        public string Url;

        public BreadCrumbs()
        {
            
        }

        public BreadCrumbs(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}