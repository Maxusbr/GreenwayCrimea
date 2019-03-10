using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Attachments;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Crm.DealStatuses;
using AdvantShop.Core.Services.Loging.Emails;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Repository.Currencies;
using AdvantShop.Saas;
using AdvantShop.Shipping;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Attachments;
using AdvantShop.Web.Admin.Handlers.Leads;
using AdvantShop.Web.Admin.Models;
using AdvantShop.Web.Admin.Models.Attachments;
using AdvantShop.Web.Admin.Models.Leads;
using AdvantShop.Web.Admin.ViewModels.Leads;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using Newtonsoft.Json;

namespace AdvantShop.Web.Admin.Controllers.Crm
{
    [Auth(RoleAction.Crm)]
    [SaasFeature(ESaasProperty.HaveCrm)]
    public partial class LeadsController : BaseAdminController
    {
        #region Leads list

        public ActionResult Index()
        {
            var model = new LeadsListViewModel();

            SetMetaInformation(T("Admin.Leads.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.LeadsCtrl);

            if (SettingsCheckout.BuyInOneClickCreateOrder)
            {
                ShowNotification(NotifyType.Notice,
                    "Покупка в один клик стоит в режиме \"Создавать заказ\". Переключить на \"Создавать лид\"? " +
                    "<a class=\"btn btn-sm btn-success\" ng-click=\"leads.changeBuyInOneClickCreateOrder()\">Переключить</a>");
            }

            TrialService.TrackEvent(ETrackEvent.Trial_VisitCRM);

            return View(model);
        }
        
        public JsonResult GetLeads(LeadsFilterModel model)
        {
            return Json(new GetLeads(model).Execute());
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeBuyInOneClickCreateOrder()
        {
            SettingsCheckout.BuyInOneClickCreateOrder = false;
            return JsonOk();
        }

        #region Command

        private void Command(LeadsFilterModel model, Action<int, LeadsFilterModel> func)
        {
            if (model.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in model.Ids)
                    func(id, model);
            }
            else
            {
                var ids = new GetLeads(model).GetItemsIds();
                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        public JsonResult DeleteLeads(LeadsFilterModel model)
        {
            Command(model, (id, c) => LeadService.DeleteLead(id));
            return JsonOk();
        }

        public JsonResult GetManagers()
        {
            var managers = ManagerService.GetCustomerManagersList().OrderBy(x => x.FirstName);

            return Json(managers.Select(x => new SelectItemModel(string.Format("{0} {1}", x.FirstName, x.LastName), x.Id.ToString())));
        }

        public JsonResult GetOrderSources()
        {
            var sources = OrderSourceService.GetOrderSources();

            return Json(sources.Select(x => new SelectItemModel(x.Name, x.Id.ToString())));
        }

        #endregion

        #endregion

        #region Add | Edit Lead

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Add(AddLeadModel model)
        {
            foreach (var key in new List<string>() { "Sum" })
            {
                if (ModelState.ContainsKey(key))
                    ModelState[key].Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                var id = new AddLead(model).Execute();
                return Json(new {result = id != 0, leadId = id});
            }

            var errors =
                (from modelState in ViewData.ModelState.Values
                    from error in modelState.Errors
                    select error.ErrorMessage).ToList();

            return Json(new {result = false, errors = String.Join(", ", errors) });
        }

        public ActionResult Edit(int id)
        {
            var model = new GetLead(id).Execute();
            if (model == null)
                return RedirectToAction("Index");

            SetMetaInformation(T("Admin.Leads.Edit.Title") + " " + model.Id);
            SetNgController(NgControllers.NgControllersTypes.LeadCtrl);

            return View(model);
        }
        
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(LeadModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new SaveLead(model).Execute();
                if (result)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                    return RedirectToAction("Edit", new { id = model.Id });
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.Leads.Edit.Title") + " " + model.Id);
            SetNgController(NgControllers.NgControllersTypes.LeadCtrl);

            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteLead(int leadId)
        {
            if (LeadService.CheckAccess(LeadService.GetLead(leadId)) == false)
                return JsonError();

            LeadService.DeleteLead(leadId);
            return JsonOk();
        }

        public JsonResult GetLeadForm(Guid? customerId, bool fromCart)
        {
            var manager = ManagerService.GetManager(CustomerContext.CurrentCustomer.Id);

            List<LeadTempProduct> products = null;
            float sum = 0;

            if (fromCart && customerId.HasValue)
            {
                var cart = ShoppingCartService.GetShoppingCart(ShoppingCartType.ShoppingCart, customerId.Value, false);
                products = cart.Select(x => new LeadTempProduct()
                {
                    OfferId = x.OfferId,
                    ArtNo = x.Offer.ArtNo,
                    Amount = x.Amount,
                    Name = x.Offer.Product.Name +
                        (x.Offer.Size != null ? ", " + x.Offer.Size.SizeName : "") +
                        (x.Offer.Color != null ? ", " + x.Offer.Color.ColorName : ""),
                    Price = x.Offer.RoundedPrice,
                    PreparedPrice = x.Offer.RoundedPrice.FormatPrice()
                }).ToList();
                sum = products.Sum(x => x.Price);
            }

            return Json(new
            {
                currencySymbol = CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3).Symbol,
                managers = ManagerService.GetManagersList().OrderBy(x => x.FullName).Select(x => new SelectItemModel(x.FullName, x.ManagerId.ToString())),
                managerId = manager != null ? manager.ManagerId.ToString() : "",
                statuses = DealStatusService.GetList().Select(x => new SelectItemModel(x.Name, x.Id.ToString())),
                products,
                sum
            });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CreateOrder(int leadId)
        {
            var lead = LeadService.GetLead(leadId);
            if (lead == null || LeadService.CheckAccess(lead) == false)
                return Json(new { result = false });

            var order = OrderService.CreateOrder(lead);
            if (order == null)
                return Json(new { result = false });

            return Json(new {result = true, orderId = order.OrderID});
        }

        #region Lead Items 

        [HttpGet]
        public JsonResult GetLeadItems(int leadId, string sorting, string sortingType)
        {
            var leadItems = new GetLeadItems(leadId, sorting, sortingType).Execute();
            return Json(new { DataItems = leadItems });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddLeadItems(int leadId, List<int> offerIds)
        {
            var lead = LeadService.GetLead(leadId);

            if (lead == null || offerIds == null || offerIds.Count == 0)
                return Json(new { result = false });

            var result = new AddLeadItems(lead, offerIds).Execute();
            
            return Json(new { result });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateLeadItem(LeadItemModel model)
        {
            var lead = LeadService.GetLead(model.LeadId);
            if (lead == null)
                return Json(new { result = false });

            var leadItem = lead.LeadItems.Find(x => x.LeadItemId == model.LeadItemId);
            if (leadItem == null)
                return Json(new { result = false });

            leadItem.Price = model.Price;
            leadItem.Amount = model.Amount;

            LeadService.UpdateLeadItem(lead.Id, leadItem);
            
            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteLeadItem(int leadId, int leadItemId)
        {
            var lead = LeadService.GetLead(leadId);
            if (lead == null)
                return Json(new { result = false });

            var leadItem = lead.LeadItems.Find(x => x.LeadItemId == leadItemId);
            if (leadItem == null)
                return Json(new { result = false });
            
            LeadService.DeleteLeadItem(leadItem);

            lead = LeadService.GetLead(leadId);
            if (lead.LeadItems.Count == 0)
            {
                lead.Sum = 0;
                LeadService.UpdateLead(lead);
            }

            return Json(new { result = true });
        }


        public JsonResult GetLeadItemsSummary(int leadId)
        {
            var model = new GetLeadItemsSummary(leadId).Execute();
            return Json(model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeDiscount(int leadId, float discount, bool isValue)
        {
            if (!isValue && (discount < 0 || discount > 100))
                return Json(new { result = false });

            var lead = LeadService.GetLead(leadId);
            if (lead == null)
                return Json(new { result = false });

            if (!isValue)
            {
                lead.Discount = discount;
                lead.DiscountValue = 0;
            }
            else
            {
                lead.Discount = 0;
                lead.DiscountValue = discount;
            }

            LeadService.UpdateLead(lead);

            return Json(new { result = true });
        }

        #endregion
        
        #region Add Lead Modal

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult GetTempProducts(List<LeadTempProduct> model)
        {
            if (model == null)
                return Json(null);

            var ids = model.Select(x => x.OfferId).Distinct();

            var offers = new List<Offer>();

            foreach (var offerId in ids)
            {
                var offer = OfferService.GetOffer(offerId);
                if (offer == null)
                    continue;

                offers.Add(offer);
            }
            
            return Json(new
            {
                products = offers.Select(x => new LeadTempProduct()
                {
                    OfferId = x.OfferId,
                    ArtNo = x.ArtNo,
                    Amount = model.Find(m => m.OfferId == x.OfferId) != null ? model.Find(m => m.OfferId == x.OfferId).Amount : 1,
                    Name = x.Product.Name + (x.Size != null ? ", " + x.Size.SizeName : "") + (x.Color != null ? ", " + x.Color.ColorName : ""),
                    Price = x.RoundedPrice,
                    PreparedPrice = x.RoundedPrice.FormatPrice()
                }),
                sum = offers.Sum(x => x.RoundedPrice)
            });
        }

        public JsonResult GetCustomerFields(Guid? customerId)
        {
            var id = customerId != null ? customerId.Value : Guid.Empty;
            var fields = CustomerFieldService.GetCustomerFieldsWithValue(id) ?? new List<CustomerFieldWithValue>();
            return Json(fields);
        }

        #endregion

        #endregion
        
        #region Deal Statuses

        public JsonResult GetDealStatuses()
        {
            return Json(new {items = DealStatusService.GetList() });
        }

        public JsonResult GetDealStatus(int id)
        {
            return Json(DealStatusService.Get(id));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddDealStatus(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return JsonError();

            var list = DealStatusService.GetList();
            var sortOrder = (list.Count > 0 ? list.Max(x => x.SortOrder) : 0) + 10;

            DealStatusService.Add(new DealStatus() {Name = name, SortOrder = sortOrder});
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateDealStatus(DealStatus status)
        {
            var s = DealStatusService.Get(status.Id);
            if (s != null && !string.IsNullOrWhiteSpace(status.Name))
            {
                s.Name = status.Name;
                s.SortOrder = status.SortOrder;
                DealStatusService.Update(s);
            }
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeDealStatusSorting(int id, int? prevId, int? nextId)
        {
            var status = DealStatusService.Get(id);
            if (status == null)
                return JsonError();

            var statuses = DealStatusService.GetList().Where(x => x.Id != status.Id).ToList();

            if (prevId != null)
            {
                var index = statuses.FindIndex(x => x.Id == prevId);
                statuses.Insert(index + 1, status);
            }
            else if (nextId != null)
            {
                var index = statuses.FindIndex(x => x.Id == nextId);
                statuses.Insert(index > 0 ? index - 1 : 0, status);
            }

            for (int i = 0; i < statuses.Count; i++)
            {
                statuses[i].SortOrder = i * 10 + 10;
                DealStatusService.Update(statuses[i]);
            }

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteDealStatus(int id)
        {
            DealStatusService.Delete(id);
            return JsonOk();
        }

        #endregion

        #region Shippings

        public JsonResult GetShippings(int id, string country, string city, string region)
        {
            return Json(new GetLeadShippings(id, country, city, region).Execute());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CalculateShipping(int id, string country, string city, string region, BaseShippingOption shipping)
        {
            var model = new GetLeadShippings(id, country, city, region, shipping, false).Execute();

            var option = model.Shippings != null ? model.Shippings.FirstOrDefault(x => x.Id == shipping.Id) : null;

            if (option != null)
                option.Update(shipping);

            if (option == null && shipping.MethodId == 0)
                option = shipping;

            return Json(new { selectShipping = option });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveShipping(int id, string country, string city, string region, BaseShippingOption shipping)
        {
            var lead = LeadService.GetLead(id);
            if (lead == null || shipping == null)
                return JsonError();

            lead.ShippingMethodId = shipping.MethodId;
            lead.ShippingName = shipping.Name ?? shipping.NameRate;
            lead.ShippingCost = shipping.Rate;

            var pickPoint = shipping.GetOrderPickPoint();
            lead.ShippingPickPoint = pickPoint != null ? JsonConvert.SerializeObject(pickPoint) : null;
            
            LeadService.UpdateLead(lead);

            return JsonOk();
        }

        [HttpGet]
        public JsonResult GetDeliveryTime(int id)
        {
            var lead = LeadService.GetLead(id);
            if (lead == null)
                return JsonError();

            return Json(new
            {
                DeliveryDate = lead.DeliveryDate != null ? lead.DeliveryDate.Value.ToString("dd.MM.yyyy") : "",
                DeliveryTime = lead.DeliveryTime
            });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveDeliveryTime(int id, string deliveryDate, string deliveryTime)
        {
            var lead = LeadService.GetLead(id);
            if (lead == null)
                return JsonError();

            lead.DeliveryDate = !string.IsNullOrWhiteSpace(deliveryDate) ? deliveryDate.TryParseDateTime() : default(DateTime?);
            lead.DeliveryTime = deliveryTime;
            
            LeadService.UpdateLead(lead);

            return JsonOk();
        }

        [HttpGet]
        public JsonResult GetShippingCity(int leadId)
        {
            var lead = LeadService.GetLead(leadId);
            if (lead == null)
                return JsonError();

            var customer = lead.Customer;
            if (customer != null)
            {
                var contact = customer.Contacts.FirstOrDefault();
                if (contact != null)
                    return Json(new
                    {
                        Country = contact.Country,
                        Region = contact.Region,
                        City = contact.City,
                        Street = contact.Street
                    });
            }

            var country = CountryService.GetCountry(SettingsMain.SellerCountryId);
            var region = RegionService.GetRegion(SettingsMain.SellerRegionId);

            return Json(new
            {
                Country = country != null ? country.Name : "",
                Region = region != null ? region.Name : "",
                City = SettingsMain.City,
                Street = ""
            });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveShippingCity(int leadId, string country, string region, string city, string street)
        {
            var lead = LeadService.GetLead(leadId);
            if (lead == null)
                return JsonError();

            if (lead.Customer == null)
                lead.Customer = new Customer(CustomerGroupService.DefaultCustomerGroup)
                {
                    FirstName = lead.FirstName ?? "",
                    LastName = lead.LastName ?? "",
                    Patronymic = lead.Patronymic ?? "",
                    EMail = lead.Email ?? "",
                    Phone = lead.Phone ?? ""
                };

            if (lead.Customer.Contacts == null)
                lead.Customer.Contacts = new List<CustomerContact>();

            if (lead.Customer.Contacts.Count == 0)
                lead.Customer.Contacts.Add(new CustomerContact() {CustomerGuid = lead.Customer.Id});

            lead.Customer.Contacts[0].Country = country.DefaultOrEmpty();
            lead.Customer.Contacts[0].Region = region.DefaultOrEmpty();
            lead.Customer.Contacts[0].City = city.DefaultOrEmpty();
            lead.Customer.Contacts[0].Street = street.DefaultOrEmpty();

            LeadService.UpdateLead(lead);

            return JsonOk();
        }

        #endregion
    }

    [Auth(RoleAction.Crm)]
    [SaasFeature(ESaasProperty.HaveCrm)]
    public partial class LeadsExtController : BaseAdminController
    {
        #region Events

        public JsonResult GetLeadEvents(int leadId)
        {
            return Json(new GetLeadEvents(leadId).Execute());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddLeadEvent(LeadEvent leadEvent)
        {
            var lead = LeadService.GetLead(leadEvent.LeadId);
            if (lead == null || string.IsNullOrWhiteSpace(leadEvent.Message))
                return Json(new { result = false });

            leadEvent.Title = leadEvent.Type.Localize();
            leadEvent.CreatedBy = CustomerContext.CurrentCustomer.GetShortName();

            var eventId = LeadEventService.AddEvent(leadEvent);

            return Json(new { result = eventId != 0 });
        }

        public JsonResult GetEmail(string id, string folder)
        {
            return Json(CustomerService.GetEmailImap(id, folder));
        }

        public JsonResult GetSendedEmail(LeadEventEmailDataModel model)
        {
            if (model == null || model.CustomerId == Guid.Empty || string.IsNullOrEmpty(model.CustomerEmail))
                return Json(null);

            var emails = CustomerService.GetEmails(model.CustomerId, model.CustomerEmail);
            if (emails != null)
            {
                var email = emails.FirstOrDefault(x => x.CreateOn == model.CreateOn);
                if (email != null)
                    return Json(new EmailImap()
                    {
                        Subject = email.Subject,
                        HtmlBody = email.Body,
                        Date = email.CreateOn,
                        From = SettingsMail.From,
                        FromEmail = SettingsMail.From,
                        To = email.EmailAddress,
                    });
            }

            return Json(null);
        }

        #endregion
        
        #region Attachments

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadAttachments(int leadId)
        {
            var lead = LeadService.GetLead(leadId);
            if (lead == null || LeadService.CheckAccess(lead) == false)
                return Json(new UploadAttachmentsResult());

            var handler = new UploadAttachmentsHandler(leadId);
            var result = handler.Execute<LeadAttachment>();

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteAttachment(int id, int leadId)
        {
            var result = AttachmentService.DeleteAttachment<LeadAttachment>(id);
            return Json(new { result = result });
        }

        public JsonResult GetAttachments(int leadId)
        {
            return Json(AttachmentService.GetAttachments<LeadAttachment>(leadId)
                .Select(x => new AttachmentModel
                {
                    Id = x.Id,
                    ObjId = x.ObjId,
                    FileName = x.FileName,
                    FilePath = x.Path,
                    FilePathAdmin = x.PathAdmin,
                    FileSize = x.FileSizeFormatted
                })
            );
        }

        #endregion
    }
}
