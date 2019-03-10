using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.ViewModel.PaymentReceipt;

namespace AdvantShop.Controllers
{
    public class PaymentReceiptController : BaseClientController
    {
        #region Sberbank

        public ActionResult Sberbank(string ordernumber)
        {
            if (string.IsNullOrWhiteSpace(ordernumber))
                return Error404();

            var order = OrderService.GetOrderByNumber(ordernumber);
            if (order == null)
                return Error404();

            if (!(order.PaymentMethod is SberBank))
                return Error404();


            if (!CustomerContext.CurrentCustomer.IsAdmin &&
                (CustomerContext.CurrentCustomer.CustomerRole != Role.Moderator ||
                !CustomerContext.CurrentCustomer.HasRoleAction(RoleAction.Orders)))
            {
                if (order.OrderDate.AddDays(3) < DateTime.Now &&
                    (order.OrderCustomer == null || order.OrderCustomer.CustomerID != CustomerContext.CustomerId))
                    return Error404();
            }
            

            var sberbank = (SberBank)order.PaymentMethod;

            var model = new SberbankViewModel
            {
                PaymentDescription = T("PaymentReceipt.PayOrder") + " #" + order.Number,
                CompanyName = sberbank.CompanyName,
                TransactAccount = sberbank.TransAccount,
                BankName = sberbank.BankName,
                Inn = sberbank.INN,
                Kpp = sberbank.KPP,
                Bik = sberbank.BIK,
                CorrespondentAccount = sberbank.CorAccount,
                Payer = order.PaymentDetails != null && !string.IsNullOrEmpty(order.PaymentDetails.CompanyName)
                            ? Request["bill_companyname"]
                            : StringHelper.AggregateStrings(" ", order.OrderCustomer.LastName, order.OrderCustomer.FirstName, order.OrderCustomer.Patronymic),
                PayerInn = order.PaymentDetails != null && !string.IsNullOrEmpty(order.PaymentDetails.INN)
                            ? order.PaymentDetails.INN
                            : Request["bill_inn"] ?? @"___________________"
            };

            model.PayerAddress += StringHelper.AggregateStrings(", ", order.OrderCustomer.Country, order.OrderCustomer.Region, order.OrderCustomer.City);

            if (string.IsNullOrEmpty(order.OrderCustomer.Zip))
                model.PayerAddress += @", " + order.OrderCustomer.Zip;

            model.PayerAddress += @", " + order.OrderCustomer.GetCustomerShortAddress();

            float priceInBaseCurrency = order.Sum * sberbank.GetCurrencyRate(order.OrderCurrency);
            model.WholeSumPrice = Math.Floor(priceInBaseCurrency).ToString();
            model.FractSumPrice = SQLDataHelper.GetInt(Math.Round(priceInBaseCurrency - Math.Floor(priceInBaseCurrency), 2) * 100).ToString();

            return View(model);
        }

        #endregion

        #region Bill

        public ActionResult Bill(string ordernumber)
        {
            if (string.IsNullOrWhiteSpace(ordernumber))
                return Error404();

            var order = OrderService.GetOrderByNumber(ordernumber);
            if (order == null)
                return Error404();

            if (!(order.PaymentMethod is Bill))
                return Error404();

            if (!CustomerContext.CurrentCustomer.IsAdmin &&
                (CustomerContext.CurrentCustomer.CustomerRole != Role.Moderator ||
                !CustomerContext.CurrentCustomer.HasRoleAction(RoleAction.Orders)))
            {
                if (order.OrderDate.AddDays(3) < DateTime.Now &&
                    (order.OrderCustomer == null || order.OrderCustomer.CustomerID != CustomerContext.CustomerId))
                    return Error404();
            }

            var bill = (Bill)order.PaymentMethod;

            var model = new BillViewModel()
            {
                PayeesBank = bill.BankName,
                TransactAccount = bill.TransAccount,
                Bik = bill.BIK,
                Inn = bill.INN,
                Kpp = bill.KPP,
                CompanyName = bill.CompanyName,
                CorrespondentAccount = bill.CorAccount,
                StampImageName = bill.StampImageName,
                Vendor =
                    string.Format("ИНН {0}, {1}{2}, {3}{4}",
                        bill.INN,
                        string.IsNullOrWhiteSpace(bill.KPP) ? string.Empty : "КПП " + bill.KPP + ", ",
                        bill.CompanyName,
                        bill.Address,
                        (string.IsNullOrEmpty(bill.Telephone) ? string.Empty : ", тел. " + bill.Telephone)),
                Director = (string.IsNullOrEmpty(bill.Director)) ? "______________________" : bill.Director,
                PosDirector = (string.IsNullOrEmpty(bill.PosDirector)) ? "______________________" : bill.PosDirector,
                Accountant = (string.IsNullOrEmpty(bill.Accountant)) ? "______________________" : bill.Accountant,
                PosAccountant = (string.IsNullOrEmpty(bill.PosAccountant)) ? "______________________" : bill.PosAccountant,
                Manager = (string.IsNullOrEmpty(bill.Manager)) ? "______________________" : bill.Manager,
                PosManager = (string.IsNullOrEmpty(bill.PosManager)) ? "______________________" : bill.PosManager,
                OrderNumber = order.Number,
                OrderDate = order.OrderDate.ToString("dd.MM.yy")
            };

            var userAddress = StringHelper.AggregateStrings(", ", order.OrderCustomer.Country,
                order.OrderCustomer.Region, order.OrderCustomer.City, order.OrderCustomer.GetCustomerShortAddress());

            model.Buyer = order.PaymentDetails != null
                ? string.Format("{0}{1}{2}",
                    (string.IsNullOrEmpty(order.PaymentDetails.INN)) ? "" : ("ИНН " + order.PaymentDetails.INN + ", "),
                    (string.IsNullOrEmpty(order.PaymentDetails.CompanyName)) ? "" : order.PaymentDetails.CompanyName + ", ",
                    userAddress)
                : userAddress;

            var currency = order.OrderCurrency;
            //currency.CurrencyValue = bill.GetCurrencyRate(order.OrderCurrency);
            model.OrderCurrency = currency;
            model.RenderCurrency = bill.PaymentCurrency;

            if (order.TotalDiscount != 0)
                model.DiscountCost = order.TotalDiscount.RoundAndFormatPrice(model.RenderCurrency, currency.CurrencyValue);

            if (order.ShippingCost != 0)
                model.ShippingCost = order.ShippingCost.RoundAndFormatPrice(model.RenderCurrency, currency.CurrencyValue);

            if (order.PaymentCost != 0)
                model.PaymentCost = order.PaymentCost.RoundAndFormatPrice(model.RenderCurrency, currency.CurrencyValue);

            var productsCost = order.OrderItems.Sum(oi => oi.Price * oi.Amount) + order.ShippingCost + order.PaymentCost;
            model.ProductsCost = productsCost.RoundAndFormatPrice(model.RenderCurrency, currency.CurrencyValue);
            model.TotalCost = order.Sum.RoundAndFormatPrice(model.RenderCurrency, currency.CurrencyValue);

            var sumToRender = order.Sum * bill.GetCurrencyRate(currency);
            model.IntPartPrice = ((int)(Math.Floor(sumToRender))).ToString();
            var floatPart = sumToRender != 0
                ? SQLDataHelper.GetInt(Math.Round(sumToRender - Math.Floor(sumToRender), 2) * 100)
                : 0;

            switch (floatPart % 10)
            {
                case 1:
                    model.TotalKop = floatPart.ToString("0#") + @" копейка";
                    break;
                case 2:
                case 3:
                case 4:
                    model.TotalKop = floatPart.ToString("0#") + @" копейки";
                    break;
                default:
                    model.TotalKop = floatPart.ToString("0#") + @" копеек";
                    break;
            }

            model.OrderItems = order.OrderItems;
            model.OrderCertificates = order.OrderCertificates;
            model.Taxes = order.Taxes.Where(p => p.Sum > 0).ToList();

            return View(model);
        }

        #endregion

        #region Bill Ukraine

        private string GetPrice(float price, OrderCurrency currency)
        {
            return (price * currency.CurrencyValue).ToString("##,##0.00").Replace(",", ".");
        }

        public ActionResult BillUa(string ordernumber)
        {
            if (string.IsNullOrWhiteSpace(ordernumber))
                return Error404();

            var order = OrderService.GetOrderByNumber(ordernumber);
            if (order == null)
                return Error404();

            if (!(order.PaymentMethod is BillUa))
                return Error404();

            if (!CustomerContext.CurrentCustomer.IsAdmin &&
                (CustomerContext.CurrentCustomer.CustomerRole != Role.Moderator ||
                !CustomerContext.CurrentCustomer.HasRoleAction(RoleAction.Orders)))
            {
                if (order.OrderDate.AddDays(3) < DateTime.Now &&
                    (order.OrderCustomer == null || order.OrderCustomer.CustomerID != CustomerContext.CustomerId))
                    return Error404();
            }

            var billUa = (BillUa)order.PaymentMethod;

            var months = new List<string> { "січня", "лютого", "березня", "квітня", "травня", "червня", "липня", "серпня", "вересня", "жовтня", "листопада", "грудня" };

            var model = new BillUaViewModel()
            {
                CompanyName = billUa.CompanyName,
                CompanyCode = billUa.CompanyCode,
                Credit = billUa.Credit,
                BankCode = billUa.BankCode,
                BankName = billUa.BankName,
                CompanyEssencials = billUa.CompanyEssentials,

                OrderNumber = string.Format("Рахунок на оплату № {0} від {1} {2} {3} р.",
                    order.Number, order.OrderDate.Day.ToString("0#"),
                    months[order.OrderDate.Month - 1], order.OrderDate.Year),

                BuyerInfo = order.OrderCustomer.LastName + " " + order.OrderCustomer.FirstName
            };

            var currency = order.OrderCurrency;
            currency.CurrencyValue = currency.CurrencyValue / billUa.PaymentCurrency.Rate;
            model.OrderCurrency = currency;

            model.TotalCount = string.Format("Всього найменувань {0}, на суму {1} грн.", order.OrderItems.Count,
                GetPrice(order.Sum, currency));

            var taxes = order.Taxes.Where(p => p.Sum > 0).ToList();
            if (taxes.Count > 0)
            {
                var taxSum = (float)Math.Round(taxes.Sum(t => t.Sum), 2);

                model.TaxSum = GetPrice(taxSum, currency);
                model.Tax = (taxSum * currency.CurrencyValue).ToString("####0.00").Replace(",", ".");
            }

            if (order.OrderDiscount != 0 || order.OrderDiscountValue != 0)
                model.Discount = GetPrice(order.TotalDiscount, currency);

            if (order.ShippingCost != 0)
                model.ShippingCost = GetPrice(order.ShippingCost, currency);

            model.OrderItems = order.OrderItems;
            model.Total = GetPrice(order.Sum, currency);

            return View(model);
        }

        #endregion

        #region Check 

        public ActionResult Check(string ordernumber)
        {
            if (string.IsNullOrWhiteSpace(ordernumber))
                return Error404();

            var order = OrderService.GetOrderByNumber(ordernumber);
            if (order == null)
                return Error404();

            if (!(order.PaymentMethod is Check))
                return Error404();

            if (!CustomerContext.CurrentCustomer.IsAdmin &&
                (CustomerContext.CurrentCustomer.CustomerRole != Role.Moderator ||
                !CustomerContext.CurrentCustomer.HasRoleAction(RoleAction.Orders)))
            {
                if (order.OrderDate.AddDays(3) < DateTime.Now &&
                    (order.OrderCustomer == null || order.OrderCustomer.CustomerID != CustomerContext.CustomerId))
                    return Error404();
            }

            var check = (Check)order.PaymentMethod;

            var currency = order.OrderCurrency;

            var model = new CheckViewModel()
            {
                CompanyName = check.CompanyName,
                Address = check.Adress,
                Country = check.Country,
                State = check.State,
                City = check.City,

                CompanyPhone = check.Phone,
                InterPhone = check.IntPhone,
                CompanyFax = check.Fax,

                OrderDate = Localization.Culture.ConvertDate(order.OrderDate),
                OrderId = @"#" + order.Number,
                ShippingMethod = order.ShippingMethodName,

                Name = StringHelper.AggregateStrings(" ", order.OrderCustomer.LastName, order.OrderCustomer.FirstName, order.OrderCustomer.Patronymic),
                Phone = order.OrderCustomer.Phone,
                Email = order.OrderCustomer.Email,

                BillingAddress = order.OrderCustomer.GetCustomerShortAddress(),
                BillingCity = order.OrderCustomer.City,
                BillingState = order.OrderCustomer.Region,
                BillingCountry = order.OrderCustomer.Country,
                BillingZip = order.OrderCustomer.Zip,

                ShippingAddress = order.OrderCustomer.GetCustomerShortAddress(),
                ShippingCity = order.OrderCustomer.City,
                ShippingState = order.OrderCustomer.Region,
                ShippingCountry = order.OrderCustomer.Country,
                ShippingZip = order.OrderCustomer.Zip,

                OrderItems = order.OrderItems,
                OrderCurrency = currency,
                RenderCurrency = check.PaymentCurrency
            };

            model.SubTotal =
                ((order.Sum - order.ShippingCost + order.OrderDiscountValue) * 100.0F / (100 - order.OrderDiscount)).RoundAndFormatPrice(model.RenderCurrency, currency.CurrencyValue);
            model.ShippingCost = order.ShippingCost.RoundAndFormatPrice(model.RenderCurrency, currency.CurrencyValue);
            model.Discount = order.DiscountCost.RoundAndFormatPrice(model.RenderCurrency, currency.CurrencyValue);
            //(order.OrderDiscount*((order.Sum - order.ShippingCost - order.OrderDiscountValue) /(100 - order.OrderDiscount))).RoundAndFormatPrice(model.RenderCurrency, currency.CurrencyValue);
            model.Total = order.Sum.RoundAndFormatPrice(model.RenderCurrency, currency.CurrencyValue);

            return View(model);
        }

        #endregion
    }
}