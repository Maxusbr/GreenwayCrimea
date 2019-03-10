using System;
using System.Collections.Generic;

namespace AdvantShop.Modules.RetailCRM.Models
{
    public class SerializedCustomer
    {
        public string externalId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string patronymic { get; set; }
        public string email { get; set; }
        public List<CustomerPhone> phones { get; set; }

        public CustomerAddress address { get; set; } 

        public DateTime createdAt { get; set; }
        
        //public bool vip { get; set; }
        //public bool bad { get; set; }
        //public string commentary { get; set; }

        //public int managerId { get; set; }
    }

    public class CustomerPhone
    {
        public string number { get; set; }
    }

    public class CustomerAddress
    {
        public string index { get; set; }
        public string country { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        
        public string notes { get; set; }
        public string text { get; set; }
        // поля дял склейки адреса, если text поле пришло пустым
        public string metro { get; set; }
        public string street { get; set; }
        public string building { get; set; }
        public string flat { get; set; }
        public string house { get; set; }
        public int block { get; set; }
        public int floor { get; set; }
    }

    public class CreateCustomerResponse : ResponseSimple
    {
    }
}