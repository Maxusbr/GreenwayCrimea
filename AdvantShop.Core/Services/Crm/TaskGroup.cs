using System;

namespace AdvantShop.Core.Services.Crm
{
    public class TaskGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
