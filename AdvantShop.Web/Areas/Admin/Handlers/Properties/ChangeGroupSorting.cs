using System.Linq;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Properties
{
    public class ChangeGroupSorting
    {
        private readonly int _groupId;
        private readonly int? _prevGroupId;
        private readonly int? _nextGroupId;

        public ChangeGroupSorting(int groupId, int? prevGroupId, int? nextGroupId)
        {
            _groupId = groupId;
            _prevGroupId = prevGroupId;
            _nextGroupId = nextGroupId;
        }

        public bool Execute()
        {
            var group = PropertyGroupService.Get(_groupId);
            if (group == null)
                return false;

            PropertyGroup prevGroup = null, nextGroup = null;

            if (_prevGroupId != null)
                prevGroup = PropertyGroupService.Get(_prevGroupId.Value);

            if (_nextGroupId != null)
                nextGroup = PropertyGroupService.Get(_nextGroupId.Value);

            if (prevGroup == null && nextGroup == null)
                return false;

            if (prevGroup != null && nextGroup != null)
            {
                if (nextGroup.SortOrder - prevGroup.SortOrder > 1)
                {
                    group.SortOrder = prevGroup.SortOrder + 1;
                    PropertyGroupService.UpdateSortOrder(group.PropertyGroupId, group.SortOrder);
                }
                else
                {
                    UpdateSortOrderForAllGroups(group, prevGroup, nextGroup);
                }
            }
            else
            {
                UpdateSortOrderForAllGroups(group, prevGroup, nextGroup);
            }

            return true;
        }

        private void UpdateSortOrderForAllGroups(PropertyGroup group, PropertyGroup prevGroup, PropertyGroup nextGroup)
        {
            var groups =
                PropertyGroupService.GetList()
                    .Where(x => x.PropertyGroupId != group.PropertyGroupId)
                    .OrderBy(x => x.SortOrder)
                    .ToList();

            if (prevGroup != null)
            {
                var index = groups.FindIndex(x => x.PropertyGroupId == prevGroup.PropertyGroupId);
                groups.Insert(index + 1, group);
            }
            else if (nextGroup != null)
            {
                var index = groups.FindIndex(x => x.PropertyGroupId == nextGroup.PropertyGroupId);
                groups.Insert(index > 0 ? index - 1 : 0, group);
            }

            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].SortOrder = i * 10 + 10;
                PropertyGroupService.UpdateSortOrder(groups[i].PropertyGroupId, groups[i].SortOrder);
            }
        }
    }
}
