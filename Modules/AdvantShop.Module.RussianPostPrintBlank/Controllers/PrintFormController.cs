using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Module.RussianPostPrintBlank.Models;
using AdvantShop.Module.RussianPostPrintBlank.Service;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace AdvantShop.Module.RussianPostPrintBlank.Controllers
{
    [Module(Type = "RussianPostPrintBlank")]
    public class PrintFormController : ModuleController
    {
        public ActionResult PrintByTemplate(int formType, int orderId, float? prepayment)
        {
            if (formType <= 0 || orderId < 1)
            {
                throw new HttpException(500, "Внутренняя ошибка сервера 1.");
            }
            
            var order = AdvantShop.Orders.OrderService.GetOrder(orderId);
            if (order == null)
            {
                throw new HttpException(500, "Внутренняя ошибка сервера 2.");
            }

            var template = TemplatesService.GetTemplate(formType);
            if(template == null)
            {
                throw new HttpException(500, "Внутренняя ошибка сервера 3.");
            }
            
            var type = template.Type;

            var model = new FormModel();
            model.Type = type;

            //model.DeclaredValue = !order.Payed ? (ModuleSettings.RoundToInteger ? Math.Round(order.Sum - subtractionSum) : order.Sum - subtractionSum) : 0;
            if (!prepayment.HasValue || prepayment.Value < 0) prepayment = 0;

            model.DeclaredValue = order.Sum - (prepayment ?? 0);
            if (model.DeclaredValue < 0) model.DeclaredValue = 0;

            var reverseAddressOrder = false;//ModuleSettings.ReverseAddressOrder;


            var customer = order.OrderCustomer;
            var addr = string.Format("{0}{1}{2}{3}", !string.IsNullOrEmpty(customer.Street) ? customer.Street : string.Empty,
                                                     !string.IsNullOrEmpty(customer.House) ? ", д. " + customer.House : string.Empty,
                                                     !string.IsNullOrEmpty(customer.Structure) ? ", " + customer.Structure : string.Empty,
                                                     !string.IsNullOrEmpty(customer.Apartment) ? ", кв." + customer.Apartment : string.Empty);

            var city = !string.IsNullOrEmpty(customer.City) ? RPPBService.CorrectAddress(customer.City) : string.Empty;
            var zone = !string.IsNullOrEmpty(customer.Region) ? RPPBService.CorrectAddress(customer.Region) : string.Empty;

            zone = zone == city ? string.Empty : zone;

            addr = RPPBService.DeleteCityAndZoneFromAddress(addr, city, zone);
            addr = RPPBService.CorrectAddress(addr);

            var region = string.Empty;

            if (reverseAddressOrder)
            {
                region = !string.IsNullOrEmpty(city) ? city : string.Empty;
                region += !string.IsNullOrEmpty(zone) ? (!string.IsNullOrEmpty(region) ? ", " : string.Empty) + zone : string.Empty;

            }
            else
            {
                region = !string.IsNullOrEmpty(zone) ? zone : string.Empty;
                region += !string.IsNullOrEmpty(city) ? (!string.IsNullOrEmpty(region) ? ", " : string.Empty) + city : string.Empty;
            }

            switch (type)
            {
                case FormType.F7:
                    var contentF7 = JsonConvert.DeserializeObject<F7Model>(template.Content);
                    model.BlankName = "blank7";
                    model.PackageType = !string.IsNullOrEmpty(contentF7.PackageType) ? contentF7.PackageType : "package";
                    model.FromFirstName = string.Format("{0} {1}", contentF7.Name, contentF7.Patronymic);
                    model.FromLastName = contentF7.LastName;
                    model.FromPostCode = contentF7.PostalCode;
                    
                    model.FromRegion = !string.IsNullOrEmpty(contentF7.Region) ? RPPBService.CorrectAddress(contentF7.Region) : string.Empty;
                    model.FromCity = !string.IsNullOrEmpty(contentF7.City) ? RPPBService.CorrectAddress(contentF7.City) : string.Empty;
                    model.FromStreet = !string.IsNullOrEmpty(contentF7.Street) ? RPPBService.CorrectAddress(contentF7.Street) : string.Empty;
                    model.FromHome = !string.IsNullOrEmpty(contentF7.House) ? RPPBService.CorrectAddress(contentF7.House) : string.Empty;
                    model.FromApartment = !string.IsNullOrEmpty(contentF7.Apartment) ? RPPBService.CorrectAddress(contentF7.Apartment) : string.Empty;

                    model.ToFirstName = string.Format("{0} {1}", customer.FirstName, customer.Patronymic);
                    model.ToLastName = customer.LastName;

                    model.ToCity = reverseAddressOrder ? addr : region;
                    model.ToStreet = reverseAddressOrder ? region : addr;

                    model.ToPostCode = !string.IsNullOrEmpty(customer.Zip) ? customer.Zip :
                                       Regex.Match(addr, @"\d{6}", RegexOptions.IgnoreCase).ToString();
                    break;

                case FormType.F107:
                    var contentF107 = JsonConvert.DeserializeObject<F107Model>(template.Content);
                    model.BlankName = "blank107";
                    model.MailId = contentF107.UseTrackNumber ? order.TrackNumber : string.Empty;
                    model.OrganizationName = contentF107.OrganizationName;
                    model.Products = new List<ProductForm>();
                    model.Products.AddRange(order.OrderItems.Select(item => new ProductForm { ProductName = item.Name, Price = item.Price, Amount = item.Amount }));
                    break;

                case FormType.F112:
                    var contentF112 = JsonConvert.DeserializeObject<F112Model>(template.Content);
                    model.BlankName = "blank112";
                    model.COD = contentF112.COD ? "1" : "0";
                    model.DeliveryHome = contentF112.DeliveryHome ? "1" : "0";
                    model.Notification = contentF112.Notification ? "1" : "0";

                    model.ToPhone = contentF112.Phone ?? string.Empty;
                    model.ToPhone = !string.IsNullOrEmpty(model.ToPhone) ? model.ToPhone.Replace('+', ' ') : string.Empty;

                    model.FromPhone = string.IsNullOrEmpty(order.OrderCustomer.Phone) ? order.OrderCustomer.Phone : string.Empty;
                    model.FromPhone = !string.IsNullOrEmpty(model.FromPhone) ? model.FromPhone.Replace('+', ' ') : string.Empty;

                    if (!contentF112.NoSendingAddress)
                    {
                        model.FromFirstName = string.Format("{0} {1} {2}", order.OrderCustomer.FirstName, order.OrderCustomer.Patronymic, order.OrderCustomer.LastName);

                        model.FromCity = reverseAddressOrder ? addr : region;
                        model.FromStreet = reverseAddressOrder ? region : addr;

                        model.FromPostCode = !string.IsNullOrEmpty(customer.Zip) ? customer.Zip :
                                           Regex.Match(addr, @"\d{6}", RegexOptions.IgnoreCase).ToString();
                    }
                    
                    model.FirstString = contentF112.FirstMessageString;
                    model.SecondString = contentF112.SecondMessageString;

                    model.Inn = contentF112.Inn;
                    model.CorrespondentAccount = contentF112.CorrespondentAccount;
                    model.CheckingAccount = contentF112.CheckingAccount;
                    model.BankName = contentF112.BankName;
                    model.BankCode = contentF112.BankCode;

                    model.ToFirstName = contentF112.UseReceiverName ? contentF112.ReceiverName : string.Format("{0} {1} {2}", contentF112.Name, contentF112.Patronymic, contentF112.LastName);

                    model.ToCity = string.Format("{0}{1}", !string.IsNullOrEmpty(contentF112.Region) ? contentF112.Region: string.Empty, !string.IsNullOrEmpty(contentF112.City) ? ", " + contentF112.City : string.Empty);
                    model.ToStreet = string.Format("{0}{1}{2}", contentF112.Street, !string.IsNullOrEmpty(contentF112.House) ? ", " + contentF112.House : string.Empty, !string.IsNullOrEmpty(contentF112.Apartment) ? ", " + contentF112.Apartment : string.Empty);
                    model.ToHome = !string.IsNullOrEmpty(contentF112.House) ? RPPBService.CorrectAddress(contentF112.House) : string.Empty;
                    model.ToApartment = !string.IsNullOrEmpty(contentF112.Apartment) ? RPPBService.CorrectAddress(contentF112.Apartment) : string.Empty;

                    model.ToCity = RPPBService.CorrectAddress(model.ToCity);
                    model.ToStreet = RPPBService.CorrectAddress(model.ToStreet);

                    model.ToPostCode = contentF112.PostalCode;
                    break;
            }

            return View("~/modules/" + RussianPostPrintBlank.ModuleStringId + "/Views/PrintForm/Index.cshtml", model);
        }
    }
}
