using AdvantShop.Diagnostics;
using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using AdvantShop.Customers;
using AdvantShop.Saas;
using AdvantShop.Orders;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Core.Services.Crm;
using System.Web;
using System.Linq;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Module.JivoSite.Domain
{
    public static class JivoService
    {

        private const string WidgetCode = @"
<!-- BEGIN JIVOSITE CODE {literal} -->
<script type='text/javascript'>
(function(){ var widget_id = '#WIDGET_ID#';var d=document;var w=window;function l(){
var s = document.createElement('script'); s.type = 'text/javascript'; s.async = true; s.src = '//code.jivosite.com/script/widget/'+widget_id; var ss = document.getElementsByTagName('script')[0]; ss.parentNode.insertBefore(s, ss);}if(d.readyState=='complete'){l();}else{if(w.attachEvent){w.attachEvent('onload',l);}else{w.addEventListener('load',l,false);}}})();</script>
<!-- {/literal} END JIVOSITE CODE -->";

        private const string CustomerScript = @"
<script>
function jivo_onLoadCallback() {
    setTimeout( function(){ jivo_api.setUserToken('#CUSTOMER_ID#');}, 5000);
}
</script>";

        public const string PartnerId = "AdvantShop";
        public const string PartnerPassword = "AdvantShop";


        public static string GetWidgetCode()
        {
            var code = ModuleSettingsProvider.GetSettingValue<string>("WidgetId", JivoSite.ModuleId);

            return code.IsNotEmpty()
                ? WidgetCode.Replace("#WIDGET_ID#", code) + 

                        (CustomerContext.CurrentCustomer.RegistredUser ?
                            CustomerScript.Replace("#CUSTOMER_ID#", CustomerContext.CustomerId.ToString())
                            : string.Empty)

                : string.Empty;
        }


        public static JivoChatAcceptedResponse ChatAccepted(JivoRequest request)
        {
            Customer customer = null;
            Guid customerID;


            if (request.token.IsNotEmpty() && Guid.TryParse(request.token, out customerID))
            {
                customer = CustomerService.GetCustomer(customerID);
            }

            if (customer == null && request.user_token.IsNotEmpty() && Guid.TryParse(request.user_token, out customerID))
            {
                customer = CustomerService.GetCustomer(customerID);
            }


            if (customer == null && request.visitor.email.IsNotEmpty())
            {
                customer = CustomerService.GetCustomerByEmail(request.visitor.email);
            }

           CreateLead(request, customer, false);

            List<Order> orders = new List<Order>();
            if (customer != null)
            {
                orders = OrderService.GetOrders(customer.EMail);
            }
            else if (request.visitor.email.IsNotEmpty())
            {
                orders = OrderService.GetOrders(request.visitor.email);
            }

            var response = new JivoChatAcceptedResponse();
            if (customer != null)
            {
                response.crm_link = Configuration.SettingsMain.SiteUrl + "/admin/ViewCustomer.aspx?Customerid=" + (customer != null ? customer.Id.ToString() : request.token);
            }

            if (customer != null)
            {
                response.contact_info = new ContactInfo()
                {
                    email = customer.EMail,
                    name = customer.FirstName + " " + customer.LastName,
                    phone = customer.Phone,
                    description = customer.AdminComment
                };
                response.enable_assign = true;
            }

            if (orders != null && orders.Any())
            {
                response.custom_data = new List<CustomData>
                            {
                                new CustomData()
                                {
                                    title = "Число заказов", content = orders.Count.ToString() }, new CustomData { title = "Общая сумма", content = PriceFormatService.FormatPrice(orders.Sum(o => o.Sum))
                                }
                            };
            }

            return response;
        }

        public static JivoBaseResponse ChatFinished(JivoRequest request)
        {
            Lead lead = null;

            Customer customer = null;
            Guid customerID = Guid.Empty;
            
            if (request.token.IsNotEmpty() && Guid.TryParse(request.token, out customerID))
            {
                customer = CustomerService.GetCustomer(customerID);
            }

            if (customer == null && request.user_token.IsNotEmpty() && Guid.TryParse(request.user_token, out customerID))
            {
                customer = CustomerService.GetCustomer(customerID);
            }


            if (customer == null && request.visitor.email.IsNotEmpty())
            {
                customer = CustomerService.GetCustomerByEmail(request.visitor.email);
            }

            var orederSource = OrderSourceService.GetOrderSource(OrderType.LiveChat);

            if (customer != null || customerID != Guid.Empty)
            {
                lead = LeadService.GetLeadsByCustomer(customer != null ? customer.Id : customerID)
                    .OrderBy(l => l.CreatedDate).LastOrDefault(l => orederSource != null && l.OrderSourceId == orederSource.Id);
            }

            if (lead != null)
            {
                if (lead.FirstName.IsNullOrEmpty())
                {
                    lead.FirstName = request.visitor != null ? HttpUtility.HtmlEncode(request.visitor.name) : string.Empty;
                }

                if (lead.Phone.IsNullOrEmpty())
                {
                    lead.Phone = request.visitor != null ? HttpUtility.HtmlEncode(request.visitor.phone) : string.Empty;
                }

                if (lead.Phone.IsNullOrEmpty())
                {
                    lead.Email = request.visitor != null ? HttpUtility.HtmlEncode(request.visitor.email) : string.Empty;
                }

                LeadService.UpdateLead(lead);
            }
            else {
                CreateLead(request, customer, false);
            }
            
            return new JivoBaseResponse();
        }


        public static JivoBaseResponse OfflineMessage(JivoRequest request)
        {
            Customer customer = null;
            Guid customerID;
            if (request.token.IsNotEmpty() && Guid.TryParse(request.token, out customerID))
            {
                customer = CustomerService.GetCustomer(customerID);
            }

            if (customer == null && request.user_token.IsNotEmpty() && Guid.TryParse(request.user_token, out customerID))
            {
                customer = CustomerService.GetCustomer(customerID);
            }


            if (customer == null && request.visitor.email.IsNotEmpty())
            {
                customer = CustomerService.GetCustomerByEmail(request.visitor.email);
            }

            CreateLead(request, customer, true);

            return new JivoBaseResponse();
        }


        public static bool InstallNewWidget(string email, string userName, string userPassword, out string error)
        {
            Guid authToken = Guid.NewGuid();
            try
            {

                using (var client = new WebClient())
                {
                    var values = new NameValueCollection()  {
                       {"partnerId", PartnerId },
                       {"partnerPassword", PartnerPassword },
                       {"siteUrl", Configuration.SettingsMain.SiteUrl },
                       {"email", email},
                       {"userPassword", userPassword},
                       {"userDisplayName", userName },
                       {"authToken", authToken.ToString() }
                    };

                    var response = client.UploadValues("https://admin.jivosite.com/integration/install", values);
                    var responseString = Encoding.UTF8.GetString(response);
                    if (responseString.ToLower().Contains("error"))
                    {
                        error = responseString;
                        return false;
                    }
                    else
                    {
                        if (responseString.IsNotEmpty())
                        {
                            ModuleSettingsProvider.SetSettingValue("WidgetId", responseString, JivoSite.ModuleId);
                            ModuleSettingsProvider.SetSettingValue("AuthToken", authToken, JivoSite.ModuleId);
                            error = string.Empty;
                            return true;
                        }
                        else
                        {
                            error = "Empty widget id";
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                error = ex.Message;
                return false;
            }
        }

        private static void CreateLead(JivoRequest request, Customer customer, bool isOffline)
        {
            var crmEnable = (!SaasDataService.IsSaasEnabled ||
                           (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveCrm));

            if (crmEnable)
            {

                Manager manager = null;

                if (request.agent != null && request.agent.email.IsNotEmpty())
                {
                    var managerCust = CustomerService.GetCustomerByEmail(request.agent.email);
                    if (managerCust != null)
                    {
                        manager = ManagerService.GetManager(managerCust.Id);
                    }
                }


                var orederSource = OrderSourceService.GetOrderSource(OrderType.LiveChat);
                var lead = new Lead()
                {
                    Email = customer != null ? customer.EMail : request.visitor.email.IsNotEmpty() ? HttpUtility.HtmlEncode(request.visitor.email) : string.Empty,
                    FirstName = customer != null ? customer.FirstName + " " + customer.LastName : request.visitor.name.IsNotEmpty() ? HttpUtility.HtmlEncode(request.visitor.name) : string.Empty,
                    Phone = customer != null ? customer.Phone : request.visitor.phone.IsNotEmpty() ? HttpUtility.HtmlEncode(request.visitor.phone) : string.Empty,
                    CustomerId = customer != null ? customer.Id : request.token.TryParseGuid(true),
                    Customer = customer != null ? customer : null,
                    Comment = request.message != null ? HttpUtility.HtmlEncode(request.message) : string.Empty,
					Description = isOffline 
                                        ? HttpUtility.HtmlEncode("Оффлайн сообщение JivoSite. Номер чата: " + request.offline_message_id + "\n Ссылка: https://admin.jivosite.com/chats/view?chat_id=" + request.offline_message_id)
					                    : HttpUtility.HtmlEncode("Чат JivoSite. Номер чата: " + request.chat_id + "\n Ссылка: https://admin.jivosite.com/chats/view?chat_id=" + request.chat_id),
                    OrderSourceId = orederSource != null ? orederSource.Id : 0,
                    ManagerId = manager != null ? manager.ManagerId : (int?)null,
                };

                if((!string.IsNullOrEmpty(lead.Email) || !string.IsNullOrEmpty(lead.Phone)) && !string.IsNullOrEmpty(lead.FirstName))
                {
                    LeadService.AddLead(lead);
                }
            }
        }
        
    }
}
