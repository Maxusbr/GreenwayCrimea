//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Module.Callback.Services
{
    public class CallbackCustomer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime DateAdded { get; set; }
        public string Comment { get; set; }
        public string AdminComment { get; set; }
        public bool Processed { get; set; }
    }
}