using System.Collections.Generic;


namespace AdvantShop.Core.Services.Payment.SberBankAcquiring
{
    public class Receipt
    {
        //public long orderCreationDate;                    // not required
        public CustomerDetails customerDetails { get; set; } // not required
        public CartItems cartItems { get; set; }
    }

    public class CustomerDetails
    {
        public string email { get; set; }
        //public string phone { get; set; }         // not required
        //public string contact { get; set; }       // not required
        //public object deliveryInfo { get; set; }  // not required
    }


    public class CartItems
    {
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string positionId { get; set; }
        public string name { get; set; }
        public Quantity quantity { get; set; }

        public int itemAmount { get; set; }
        public string itemCode { get; set; }
        public int itemPrice { get; set; }

        public Tax tax { get; set; } // not required

    }

    public class Quantity
    {
        public float value { get; set; }
        public string measure { get; set; }
    }

    public class Tax
    {
        public int taxType { get; set; }
        //public float taxSum { get; set; }
    }

}
