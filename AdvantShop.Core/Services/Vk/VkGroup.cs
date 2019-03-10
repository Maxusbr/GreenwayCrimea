using VkNet.Model;

namespace AdvantShop.Core.Services.Vk
{
    public class VkGroup
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Photo100 { get; set; }
        public string ScreenName { get; set; }

        public VkGroup()
        {
            
        }

        public VkGroup(Group x)
        {
            Id = x.Id;
            Name = x.Name;
            Photo100 = x.Photo100 != null ? x.Photo100.ToString() : null;
            ScreenName = x.ScreenName;
        }
    }
}
