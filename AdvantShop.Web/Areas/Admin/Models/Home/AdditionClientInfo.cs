using System.Collections.Generic;

namespace AdvantShop.Web.Admin.Models.Home
{
    public class AdditionClientInfo
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        public string Mobile { get; set; }
        public string CompanyName { get; set; }
        public List<PropertyMap> Map { get; set; }

        public bool Show { get; set; }
    }

    public class PropertyMap
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
