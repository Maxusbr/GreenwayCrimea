//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Customers
{
    [Serializable]
    public class CustomerContact
    {
        public Guid ContactId { get; set; }

        public Guid CustomerGuid { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public int? RegionId { get; set; }

        public string Region { get; set; }

        //public string Address { get; set; }

        public string Zip { get; set; }
        

        public string Street { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public string Structure { get; set; }
        public string Entrance { get; set; }
        public string Floor { get; set; }

        public CustomerContact()
        {
            ContactId = Guid.Empty;
            CustomerGuid = Guid.Empty;
            Name = string.Empty;
            Country = string.Empty;
            Region = string.Empty;
            City = string.Empty;
            Zip = string.Empty;
        }
    }
}