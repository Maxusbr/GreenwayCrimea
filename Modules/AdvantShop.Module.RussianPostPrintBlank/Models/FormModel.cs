using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using System.Web.Mvc;

namespace AdvantShop.Module.RussianPostPrintBlank.Models
{
    public enum FormType
    {
        F7 = 1,
        F107 = 2,
        F112 = 3,
        None = 4
    }

    public class FormModel
    {
        public FormType Type { get; set; }
        public string BlankName { get; set; }

        public string PackageType { get; set; }

        public string ToLastName { get; set; }
        public string ToFirstName { get; set; }
        public string ToCity { get; set; }
        public string ToStreet { get; set; }
        public string ToHome { get; set; }
        public string ToApartment { get; set; }
        public string ToPostCode { get; set; }
        public string FromLastName { get; set; }
        public string FromFirstName { get; set; }
        public string FromRegion { get; set; }
        public string FromCity { get; set; }
        public string FromStreet { get; set; }
        public string FromHome { get; set; }
        public string FromApartment { get; set; }
        public string FromPostCode { get; set; }
        public double DeclaredValue { get; set; }

        public string MailId { get; set; }
        public string OrganizationName { get; set; }
        public List<ProductForm> Products { get; set; }

        public string COD { get; set; }
        public string DeliveryHome { get; set; }
        public string Notification { get; set; }

        public string FromPhone { get; set; }
        public string ToPhone { get; set; }

        public string FirstString { get; set; }
        public string SecondString { get; set; }

        public string Inn { get; set; }
        public string CorrespondentAccount { get; set; }
        public string CheckingAccount { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }

        public string Patronymic { get; set; }
        public string DayMonthOfIssue { get; set; }
        public string YearOfIssue { get; set; }
    }

    public class ProductForm
    {
        public string ProductName { get; set; }
        public float Amount { get; set; }
        public float Price { get; set; }
    }

    public class F7Model
    {
        public string Apartment { get; set; }
        public string City { get; set; }
        public string House { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }
        public string Street { get; set; }
        public string PackageType { get; set; }
    }

    public class F107Model
    {
        public string OrganizationName { get; set; }
        public bool UseTrackNumber { get; set; }
    }

    public class F112Model
    {
        public string Apartment { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public bool COD { get; set; }
        public string CheckingAccount { get; set; }
        public string City { get; set; }
        public string CorrespondentAccount { get; set; }
        public bool DeliveryHome { get; set; }
        public string FirstMessageString { get; set; }
        public string House { get; set; }
        public string Inn { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public bool NoSendingAddress { get; set; }
        public bool Notification { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }
        public string ReceiverName { get; set; }
        public string Region { get; set; }
        public string SecondMessageString { get; set; }
        public string Street { get; set; }
        public bool UseReceiverName { get; set; }
    }
}
