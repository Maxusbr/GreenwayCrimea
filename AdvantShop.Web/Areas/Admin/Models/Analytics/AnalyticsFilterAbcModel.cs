using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;

namespace AdvantShop.Web.Admin.Models.Analytics
{
    public class AnalyticsFilterAbcModel
    {
        private Offer _offer;
        private Offer Offer
        {
            get
            {
                if (_offer != null) return _offer;
                _offer = OfferService.GetOffer(ArtNo);
                return _offer;
            }
        }

        public int ProductId
        {
            get { return Offer != null ? Offer.ProductId : 0; }
        }

        public string ArtNo { get; set; }

        public string Name
        {
            get
            {
                if (Offer != null) return Offer.Product.Name;

                var name = Convert.ToString(SQLDataAccess.ExecuteScalar(
                    "Select top(1) Name From [Order].[OrderItems] Where ArtNo = @ArtNo", CommandType.Text,
                    new SqlParameter("@ArtNo", ArtNo)));

                return name;
            }
        }

        public float Price { get { return Offer != null ? Offer.RoundedPrice : 0; } }
        

        public string PriceFormatted
        {
            get
            {
                return Price.FormatPrice();
                //!string.IsNullOrEmpty(CurrencyIso3)
                //? PriceFormatService.FormatPrice(Price, CurrencyValue, Code, CurrencyIso3, IsCodeBefore)
                //: String.Format("{0:##,##0.00}", Price);
            }
        }
    }

    public class AnalyticsFilterRfmModel
    {
        private Customer _customer = null;
        public Customer Customer
        {
            get
            {
                if (_customer != null)
                    return _customer;

                _customer = CustomerService.GetCustomer(CustomerId);
                if (_customer != null)
                    return _customer;

                _customer =
                    SQLDataAccess.Query<Customer>(
                        "Select top(1) CustomerId,[LastName],[FirstName],[Email],[Phone] From [Order].[OrderCustomer] Where CustomerId=@CustomerId",
                        new {CustomerId}).FirstOrDefault();
                
                return null;
            }
        }
        public Guid CustomerId { get; set; }
        public string Name { get { return Customer != null ? Customer.FirstName + " " + Customer.LastName : ""; } }
        public string Email { get { return Customer != null ? Customer.EMail : ""; } }
        public string Phone { get { return Customer != null ? Customer.Phone : ""; } }
    }
    //public class AnalyticsFilterRfmModel
    //{
    //    public Guid CustomerId { get; set; }
    //    public string Name { get; set; }
    //    public string Email { get; set; }
    //    public string Phone { get; set; }
    //    public int OrderId { get; set; }
    //}
}
