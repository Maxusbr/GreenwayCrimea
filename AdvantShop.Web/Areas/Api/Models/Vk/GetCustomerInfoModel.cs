namespace AdvantShop.Areas.Api.Models.Vk
{
    public class GetCustomerInfoModel
    {
        public string Fio { get; set; }
        public string CustomerId { get; set; }
        public string CustomerLink { get; set; }
        public string Manager { get; set; }

        public string LastOrder { get; set; }
        public string LastOrderLink { get; set; }
        public string TotalOrdersSum { get; set; }

        public string LastLead { get; set; }
        public string LastLeadLink { get; set; }

        public string LastOpenTask { get; set; }
        public string LastOpenTaskLink { get; set; }

        public string LastCall { get; set; }
        public string NewTaskLink { get; set; }
    }
}