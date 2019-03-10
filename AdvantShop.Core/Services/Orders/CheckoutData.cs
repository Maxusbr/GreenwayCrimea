using System;
using System.Collections.Generic;
using AdvantShop.Customers;
using AdvantShop.Payment;
using AdvantShop.Shipping;

namespace AdvantShop.Orders
{
    #region EnActiveTab, EnUserType

    public enum EnActiveTab
    {
        NoTab = 0,
        DefaultTab = 1,
        UserTab = 2,
        ShippingTab = 3,
        PaymentTab = 4,
        SumTab = 5,
        FinalTab = 6
    }

    public enum EnUserType
    {
        /// <summary>
        /// User without registration
        /// </summary>
        NoUser,

        /// <summary>
        /// User without registration
        /// </summary>
        NewUserWithOutRegistration,

        /// <summary>
        /// User registration on checkout
        /// </summary>
        JustRegistredUser,

        /// <summary>
        /// Registered user
        /// </summary>
        RegisteredUser,

        /// <summary>
        /// Registered user without shipping contact
        /// </summary>
        RegisteredUserWithoutAddress
    }

    #endregion

    public class CheckoutUser
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid? BonusCardId { get; set; }
        public bool WantRegist { get; set; }
        public bool WantBonusCard { get; set; }
        public string Password { get; set; }
        public string Confirm { get; set; }
        public int? ManagerId { get; set; }

        public List<CustomerFieldWithValue> CustomerFields { get; set; }
    }

    public class CheckoutAddress : IEquatable<CheckoutAddress>
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        //public string Address { get; set; }
        public string Zip { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }

        public string Street { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public string Structure { get; set; }
        public string Entrance { get; set; }
        public string Floor { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return Equals(obj as CheckoutAddress);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Country != null ? Country.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (City != null ? City.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Region != null ? Region.GetHashCode() : 0);
                //hashCode = (hashCode*397) ^ (Address != null ? Address.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Zip != null ? Zip.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CustomField1 != null ? CustomField1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CustomField2 != null ? CustomField2.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CustomField3 != null ? CustomField3.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Street != null ? Street.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (House != null ? House.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Apartment != null ? Apartment.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Structure != null ? Structure.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Entrance != null ? Entrance.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Floor != null ? Floor.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(CheckoutAddress other)
        {
            if (other == null)
                return false;

            return
                this.Country == other.Country &&
                this.City == other.City &&
                this.Region == other.Region &&
                //this.Address == other.Address &&
                this.Zip == other.Zip &&
                this.CustomField1 == other.CustomField1 &&
                this.CustomField2 == other.CustomField2 &&
                this.CustomField3 == other.CustomField3 &&
                this.Street == other.Street &&
                this.House == other.House &&
                this.Apartment == other.Apartment &&
                this.Structure == other.Structure &&
                this.Entrance == other.Entrance &&
                this.Floor == other.Floor;
        }

        public static bool operator ==(CheckoutAddress address1, CheckoutAddress address2)
        {
            if ((object)address1 == null || (object)address2 == null)
                return Object.Equals(address1, address2);

            return address1.Equals(address2);
        }

        public static bool operator !=(CheckoutAddress address1, CheckoutAddress address2)
        {
            return !(address1 == address2);
        }
    }

    public class CheckoutBonus
    {
        public bool UseIt { get; set; }
        public float BonusPlus { get; set; }
    }

    public class CheckoutData
    {
        public BaseShippingOption SelectShipping { get; set; }
        public BasePaymentOption SelectPayment { get; set; }

        public int ShopCartHash { get; set; }
        public string CustomerComment { get; set; }

        public CheckoutAddress Contact { get; set; }
        public CheckoutUser User { get; set; }
        public CheckoutBonus Bonus { get; set; }

        public CheckoutData()
        {
            Contact = new CheckoutAddress();
            User = new CheckoutUser();
            Bonus = new CheckoutBonus();
        }
    }

}
