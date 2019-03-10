//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Module.ZakupiImport.Domain
{
    public class ZakupiImportCategory
    {
        public string YmlCategoryName { get; set; }

        public string YmlCategoryId { get; set; }

        public string YmlParentCategoryId { get; set; }

        public int AdvCategoryId { get; set; }
    }
}