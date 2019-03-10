using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Web.Admin.ViewModels.Properties;

namespace AdvantShop.Web.Admin.Handlers.Properties
{
    public class GetPropertiesModel
    {
        private readonly int? _groupId;

        public GetPropertiesModel(int? groupId)
        {
            _groupId = groupId;
        }

        public PropertiesViewModel Execute()
        {
            var groups = new List<PropertyGroup>()
            {
                new PropertyGroup() { Name = LocalizationService.GetResource("Admin.Properties.AllProperties") },
                new PropertyGroup() { Name = LocalizationService.GetResource("Admin.Properties.UngroupedProperties"), PropertyGroupId = -1 }
            };

            groups.AddRange(PropertyGroupService.GetList());

            var groupId = _groupId ?? 0;
            var selectedGroup = groups.Find(x => x.PropertyGroupId == groupId);

            var model = new PropertiesViewModel()
            {
                GroupId = groupId,
                GroupName = selectedGroup != null ? selectedGroup.Name : "",
                //Groups = groups,
            };


            return model;
        }
    }
}
