//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Customers;

namespace AdvantShop.Orders
{
    [Serializable]
    public class OrderCustomer : IOrderCustomer
    {
        public int OrderID { get; set; }

        public Guid CustomerID { get; set; }

        [Compare("Core.Orders.OrderCustomer.CustomerIP")]
        public string CustomerIP { get; set; }

        [Compare("Core.Orders.OrderCustomer.FirstName")]
        public string FirstName { get; set; }

        [Compare("Core.Orders.OrderCustomer.LastName")]
        public string LastName { get; set; }

        [Compare("Core.Orders.OrderCustomer.Patronymic")]
        public string Patronymic { get; set; }

        [Compare("Core.Orders.OrderCustomer.Email")]
        public string Email { get; set; }

        [Compare("Core.Orders.OrderCustomer.Phone")]
        public string Phone { get; set; }

        [Compare("Core.Orders.OrderCustomer.StandardPhone")]
        public long? StandardPhone { get; set; }



        [Compare("Core.Orders.OrderContact.Country")]
        public string Country { get; set; }

        [Compare("Core.Orders.OrderContact.Zone")]
        public string Region { get; set; }

        [Compare("Core.Orders.OrderContact.City")]
        public string City { get; set; }

        [Compare("Core.Orders.OrderContact.Zip")]
        public string Zip { get; set; }

        //[Compare("Core.Orders.OrderContact.Address")]
        //public string Address { get; set; }

        [Compare("Core.Orders.OrderContact.CustomField1")]
        public string CustomField1 { get; set; }

        [Compare("Core.Orders.OrderContact.CustomField2")]
        public string CustomField2 { get; set; }

        [Compare("Core.Orders.OrderContact.CustomField3")]
        public string CustomField3 { get; set; }


        [Compare("Core.Orders.OrderContact.Street")]
        public string Street { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        [Compare("Core.Orders.OrderContact.House")]
        public string House { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        [Compare("Core.Orders.OrderContact.Apartment")]
        public string Apartment { get; set; }

        /// <summary>
        /// Строение/Корпус
        /// </summary>
        [Compare("Core.Orders.OrderContact.Structure")]
        public string Structure { get; set; }

        /// <summary>
        /// Подъезд
        /// </summary>
        [Compare("Core.Orders.OrderContact.Entrance")]
        public string Entrance { get; set; }

        /// <summary>
        /// Этаж
        /// </summary>
        [Compare("Core.Orders.OrderContact.Floor")]
        public string Floor { get; set; }


        public static explicit operator Customer(OrderCustomer customer)
        {
            return new Customer
            {
                Id = customer.CustomerID,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Patronymic = customer.Patronymic,
                EMail = customer.Email,
                Phone = customer.Phone,
                StandardPhone = customer.StandardPhone,
                Contacts = new List<CustomerContact>()
                {
                    new CustomerContact()
                    {
                        Country = customer.Country,
                        Region = customer.Region,
                        City = customer.City,
                        Zip = customer.Zip,
                        Street = customer.Street,
                        House = customer.House,
                        Apartment = customer.Apartment,
                        Structure = customer.Structure,
                        Entrance = customer.Entrance,
                        Floor = customer.Floor
                    }
                }
            };
        }

        public static explicit operator OrderCustomer(Customer customer)
        {
            var orderCustomer = new OrderCustomer()
            {
                CustomerID = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Patronymic = customer.Patronymic,
                Email = customer.EMail,
                Phone = customer.Phone,
                StandardPhone = customer.StandardPhone,
            };

            if (customer.Contacts != null && customer.Contacts.Count > 0)
            {
                var contact = customer.Contacts[0];

                orderCustomer.Country = contact.Country;
                orderCustomer.Region = contact.Region;
                orderCustomer.City = contact.City;
                orderCustomer.Zip = contact.Zip;
                orderCustomer.Street = contact.Street;
                orderCustomer.House = contact.House;
                orderCustomer.Apartment = contact.Apartment;
                orderCustomer.Structure = contact.Structure;
                orderCustomer.Entrance = contact.Entrance;
                orderCustomer.Floor = contact.Floor;
            }

            return orderCustomer;
        }
    }
}