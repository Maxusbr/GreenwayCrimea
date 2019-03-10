//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;
using AdvantShop.Shipping.CheckoutRu;
using AdvantShop.Taxes;
using Resources;

namespace Admin
{
    public partial class EditOrder : AdvantShopAdminPage
    {
        protected Guid CustomerId;

        protected string OrderNumber
        {
            get { return (string)ViewState["OrderNumber"] ?? string.Empty; }
            set { ViewState["OrderNumber"] = value; }
        }

        protected string OrderCode
        {
            get { return (string)ViewState["OrderCode"] ?? string.Empty; }
            set { ViewState["OrderCode"] = value; }
        }

        protected int OrderID
        {
            get
            {
                if (ViewState["OrderID"] != null)
                {
                    return (int)ViewState["OrderID"];
                }
                return 0;
            }
            set
            {
                ViewState["OrderID"] = value;
            }
        }

        protected bool AddingNewOrder
        {
            get { return (Request["orderid"] != null && Request["orderid"].ToLower() == "addnew"); }
        }

        protected string RenderSplitter()
        {
            var str = new StringBuilder();
            str.Append("<td class=\'splitter\'  onclick=\'togglePanel();return false;\' >");
            str.Append("<div class=\'leftPanelTop\'></div>");
            switch (Resource.Admin_Catalog_SplitterLang)
            {
                case "rus":
                    str.Append("<div id=\'divHide\' class=\'hide_rus\'></div>");
                    str.Append("<div id=\'divShow\' class=\'show_rus\'></div>");
                    break;
                case "eng":
                    str.Append("<div id=\'divHide\' class=\'hide_en\'></div>");
                    str.Append("<div id=\'divShow\' class=\'show_en\'></div>");
                    break;
            }
            str.Append("</td>");
            return str.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CustomerId = Request["CustomerId"].TryParseGuid();
            MsgErr.Text = string.Empty;
            //CalendarExtender1.Format = SettingsMain.AdminDateFormat;

            if (!IsPostBack)
            {
                //Check item count for region dropDownList
                ddlCustomerRole.Items.Add(new ListItem(Resource.Admin_ViewCustomer_CustomerRole_User, "0"));
                ddlCustomerRole.Items.Add(new ListItem(Resource.Admin_ViewCustomer_CustomerRole_Moderator, "50"));
                if (CustomerContext.CurrentCustomer.IsAdmin)
                    ddlCustomerRole.Items.Add(new ListItem(Resource.Admin_ViewCustomer_CustomerRole_Administrator, "150"));

                foreach (var group in CustomerGroupService.GetCustomerGroupList())
                {
                    ddlCustomerGroup.Items.Add(new ListItem(string.Format("{0} - {1}%", group.GroupName, group.GroupDiscount), group.CustomerGroupId.ToString()));
                }

                //foreach (OrderType type in Enum.GetValues(typeof(OrderType)))
                //{
                //    ddlOrdertype.Items.Add(new ListItem(type.ResourceKey(), type.ToString()));
                //}

                ddlOrdertype.DataSource = OrderSourceService.GetOrderSources();
                ddlOrdertype.DataBind();


                divUseIn1c.Visible = Settings1C.Enabled;
                divManagerConfirm.Visible = SettingsCheckout.ManagerConfirmed;

                if (AddingNewOrder)
                {
                    btnSave.Text = Resource.Admin_OrderSearch_AddOrder;
                    btnSaveBottom.Text = Resource.Admin_OrderSearch_AddOrder;
                    cellPrint1.Visible = false;
                    cellPrint2.Visible = false;
                    cellPrint3.Visible = false;
                    cellPrint4.Visible = false;
                    lblOrderID.Text = Resource.Admin_OrderSearch_CreateNew;
                    ddlBillingCountry.DataBind();
                    ddlShippingCountry.DataBind();
                    if (ddlShippingCountry.Items.FindByValue(SettingsMain.SellerCountryId.ToString()) != null)
                        ddlShippingCountry.SelectedValue = SettingsMain.SellerCountryId.ToString();//CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString();
                    if (ddlBillingCountry.Items.FindByValue(SettingsMain.SellerCountryId.ToString()) != null)
                        ddlBillingCountry.SelectedValue = SettingsMain.SellerCountryId.ToString();//CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString();
                    lblOrderStatus.Text = string.Format("({0})", OrderStatusService.GetStatusName(OrderStatusService.DefaultOrderStatus));
                    lOrderDate.Text = DateTime.Now.ToString("dd.MM.yyyy");
                    txtOrderTime.Text = DateTime.Now.ToString("HH:mm");
                    lCustomerIP.Text = Request.UserHostAddress;

                    chkCopyAddress.Checked = true;
                    txtBillingAddress.Enabled = false;
                    txtBillingCity.Enabled = false;
                    txtBillingName.Enabled = false;
                    txtBillingZip.Enabled = false;
                    txtBillingZone.Enabled = false;
                    ddlBillingCountry.Enabled = false;
                    lblGroupDiscount.Text = "";

                    if (!string.IsNullOrWhiteSpace(Request["phone"]))
                    {
                        txtOrderMobilePhone.Text = Request["phone"];
                        ddlOrdertype.SelectedValue = OrderType.Phone.ToString();
                    }

                    List<PaymentMethod> listPayments = PaymentService.GetAllPaymentMethods(true).ToList();
                    ddlPaymentMethod.DataSource = listPayments;
                    ddlPaymentMethod.DataBind();

                    var currency = CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);
                    orderItems.SetCurrency(currency.Iso3, currency.Rate, currency.NumIso3, currency.Symbol, currency.IsCodeBefore);

                    if (BonusSystem.IsActive)
                    {
                        bonusPurchaise.Visible = true;
                        useBonuses.Visible = true;

                    }
                }
                else
                {
                    int id;
                    if (!string.IsNullOrEmpty(Request["orderid"]) && int.TryParse(Request["orderid"], out id))
                    {
                        OrderID = id;
                        LoadOrder();
                    }
                    else if (OrderID == 0)
                    {
                        int ordId = OrderService.GetLastOrderId();
                        if (ordId == 0)
                        {
                            OrderID = 0;
                            pnOrder.Visible = false;
                            pnEmpty.Visible = true;
                        }
                        else
                        {
                            OrderID = ordId;
                            LoadOrder();
                        }
                    }
                }
            }
            else
            {
                SetEnabled();
                //CalendarExtender1.SelectedDate = null;
            }

            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName,
                OrderID != 0
                    ? string.Format("{0} {1}", Resource.Admin_ViewOrder_ItemNum, OrderID)
                    : Resource.Admin_OrderSearch_CreateNew));
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (PopupGridCustomers.SelectedCustomers != null && PopupGridCustomers.SelectedCustomers.Count > 0)
            {
                LoadCustomer(PopupGridCustomers.SelectedCustomers[0], null);
                PopupGridCustomers.CleanSelection();
            }
            else if (CustomerId != Guid.Empty)
            {
                LoadCustomer(CustomerId, null);
            }
        }

        protected void sds_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
        }

        private void LoadTotal()
        {
            Order ord = OrderService.GetOrder(OrderID);

            float currencyValue = orderItems.CurrencyValue;
            string currencyCode = orderItems.CurrencyCode;
            float orderDiscount = orderItems.OrderDiscount;
            string currencySymbol = orderItems.CurrencySymbol;
            bool isCodeBefore = orderItems.IsCodeBefore;

            var currency = ord != null ? (Currency)ord.OrderCurrency : CurrencyService.Currency(orderItems.CurrencyCode);


            lblCurrencySymbol.Text = currency != null ? currency.Symbol : @"Get currency error";

            float shippingCost;

            if (float.TryParse(txtShippingPrice.Text, out shippingCost))
            {
                //shippingCost = shippingCost * currencyValue;
                lblShippingPrice.Text = string.Format("+{0}", PriceFormatService.FormatPrice(shippingCost, currency));
            }
            else
            {
                lblShippingPrice.Text = string.Format("+{0}", PriceFormatService.FormatPrice(0, currency));
            }

            float taxCost = -0;
            if (ord != null)
            {
                ord.ShippingCost = shippingCost;
                var shippingMethodId = hfOrderShippingId.Value.TryParseInt();
                if (shippingMethodId != 0)
                {
                    var shippping = ShippingMethodService.GetShippingMethod(shippingMethodId);
                    if (shippping != null)
                        ord.ShippingTaxType = shippping.TaxType;
                }

                taxCost = PriceService.RoundPrice(ord.Taxes.Where(tax => !tax.ShowInPrice).Sum(tax => tax.Sum), ord.OrderCurrency, ord.OrderCurrency.CurrencyValue);
                literalTaxCost.Text = TaxService.BuildTaxTable(ord.Taxes, currency, Resource.Admin_ViewOrder_Taxes);
            }

            float prodTotal = 0;

            if (ord != null && ord.OrderCertificates != null && ord.OrderCertificates.Count > 0)
            {
                prodTotal = ord.OrderCertificates.Sum(item => item.Sum);
            }
            else
            {
                prodTotal = orderItems.OrderItems.Sum(oi => oi.Price * oi.Amount);
            }

            lblTotalOrderPrice.Text = PriceFormatService.FormatPrice(prodTotal, currency);

            lblOrderDiscount.Text = string.Format("-{0}", PriceFormatService.FormatDiscountPercent(prodTotal, orderDiscount, 0, currencyValue, currencySymbol,
                                                                                                    isCodeBefore, false));
            trDiscount.Visible = orderDiscount != 0;

            var tempCurrency = CurrencyService.GetCurrencyByIso3(ord != null ? ord.OrderCurrency.CurrencyCode : currency.Iso3);
            tempCurrency.Rate = ord != null ? ord.OrderCurrency.CurrencyValue : currency.Rate;

            float totalDiscount = 0;
            totalDiscount += orderDiscount > 0 ? (orderDiscount * prodTotal / 100).RoundPrice(tempCurrency) : 0;
            if (ord != null && ord.Certificate != null)
            {
                trCertificatePrice.Visible = ord.Certificate.Price != 0;
                lblCertificatePrice.Text = string.Format("-{0}", PriceFormatService.FormatPrice(ord.Certificate.Price, currency));
                totalDiscount += ord.Certificate.Price;
            }

            if (ord != null && ord.Coupon != null)
            {
                float couponValue;
                trCoupon.Visible = ord.Coupon.Value != 0;
                switch (ord.Coupon.Type)
                {
                    case CouponType.Fixed:
                        var productsPrice = orderItems.OrderItems.Where(p => p.IsCouponApplied).Sum(p => p.Price * p.Amount);
                        couponValue = productsPrice >= ord.Coupon.Value ? ord.Coupon.Value : productsPrice;
                        totalDiscount += couponValue;
                        lblCoupon.Text = String.Format("-{0} ({1})", PriceFormatService.FormatPrice(couponValue.RoundPrice(ord.OrderCurrency.CurrencyValue, ord.OrderCurrency), currency), ord.Coupon.Code);
                        break;
                    case CouponType.Percent:
                        couponValue = orderItems.OrderItems.Where(p => p.IsCouponApplied).Sum(p => ord.Coupon.Value * p.Price / 100 * p.Amount);
                        totalDiscount += couponValue;

                        lblCoupon.Text = String.Format("-{0} ({1}%) ({2})", PriceFormatService.FormatPrice(couponValue.RoundPrice(ord.OrderCurrency.CurrencyValue, ord.OrderCurrency), currency),
                                                       PriceFormatService.FormatPriceInvariant(ord.Coupon.Value),
                                                       ord.Coupon.Code);
                        break;
                }
            }

            totalDiscount = totalDiscount.RoundPrice(ord != null ? ord.OrderCurrency.CurrencyValue : currency.Rate, ord != null ? ord.OrderCurrency : (OrderCurrency)currency);
            float orderBonuses = 0;
            float orderBonusesPlus = 0;

            if (hfContactID.Value.IsNotEmpty() && BonusSystem.IsActive && (chkUseBonuses.Checked || chkMakePurchaise.Checked))
            {
                Card bonusCard = null;

                if (ord != null && ord.BonusCardNumber != null)
                    bonusCard = BonusSystemService.GetCard(ord.BonusCardNumber);

                if (bonusCard == null)
                {
                    var customer = CustomerService.GetCustomer(new Guid(hfContactID.Value));
                    if (customer != null)
                        bonusCard = BonusSystemService.GetCard(customer.Id);
                }

                if (bonusCard != null)
                {
                    if (chkUseBonuses.Checked && bonusCard.BonusAmount > 0)
                    {
                        chkUseBonuses.Text = Resource.Admin_EditOrder_UseBonuses +
                                             string.Format(Resource.Admin_EditOrder_UseBonusesHint, bonusCard.BonusAmount);

                        orderBonuses = BonusSystemService.GetBonusCost(prodTotal - totalDiscount + shippingCost, prodTotal - totalDiscount, (float)bonusCard.BonusAmount);
                        totalDiscount += orderBonuses;
                    }

                    if (chkMakePurchaise.Checked)
                    {
                        orderBonusesPlus = BonusSystemService.GetBonusPlus(prodTotal + shippingCost - totalDiscount, prodTotal - totalDiscount, bonusCard.Grade.BonusPercent);
                    }
                }
            }
            else if (ord != null && ord.BonusCost != 0)
            {
                orderBonuses = ord.BonusCost;
                totalDiscount += orderBonuses;
            }

            lblOrderBonuses.Text = "-" + PriceFormatService.FormatPrice(orderBonuses, currency);
            trBonuses.Visible = orderBonuses != 0;

            lblOrderBonusesPlus.Text = PriceFormatService.FormatPrice(orderBonusesPlus, currency);
            trBonusesPlus.Visible = orderBonusesPlus != 0;

			float paymentCost = 0;
			if (ord != null)
			{
				paymentCost = ord.PaymentCost;
				lblPaymentPrice.Text = (ord.PaymentCost > 0 ? "+" : "") + PriceFormatService.FormatPrice(paymentCost, currency);
			}

            float sum = taxCost + prodTotal + shippingCost + paymentCost - totalDiscount;
            lblTotalPrice.Text = PriceFormatService.FormatPrice(sum < 0 ? 0 : sum, currency);
            upItems.Update();
        }

        private Order LoadOrder()
        {
            //orderList.SelectedOrder = OrderID;

            hlExport.NavigateUrl = "HttpHandlers/ExportOrderExcel.ashx?OrderID=" + OrderID;
            hlExport2.NavigateUrl = "HttpHandlers/ExportOrderExcel.ashx?OrderID=" + OrderID;
            Order ord = OrderService.GetOrder(OrderID);
            if (ord != null)
            {
                lblOrderID.Text = ord.OrderCustomer != null
                                      ? string.Format("{0} {1} - {2} {3}", Resource.Admin_ViewOrder_ItemNum, ord.Number,
                                                      ord.OrderCustomer.LastName, ord.OrderCustomer.FirstName)
                                      : string.Format("{0}{1}", Resource.Admin_ViewOrder_ItemNum, ord.Number);

                lblGroupDiscount.Text = ord.GroupDiscountString;
                chkCopyAddress.Checked = true;

                ddlOrdertype.SelectedValue = ord.OrderSourceId.ToString();

                if (ord.OrderCustomer != null)
                {
                    LoadCustomer(ord.OrderCustomer.CustomerID, ord.OrderCustomer);
                    lCustomerIP.Text = ord.OrderCustomer.CustomerIP;
                }

                OrderNumber = ord.Number;
                OrderCode = ord.Code.ToString();
                lOrderDate.Text = ord.OrderDate.ToString("dd.MM.yyy");
                txtOrderTime.Text = ord.OrderDate.ToString("HH:mm");
                //lNumber.Text = ord.Number;
                lblOrderStatus.Text = string.Format("({0})", ord.OrderStatus.StatusName);
                if (ord.OrderCurrency != null)
                    orderItems.SetCurrency(ord.OrderCurrency.CurrencyCode, ord.OrderCurrency.CurrencyValue, ord.OrderCurrency.CurrencyNumCode, ord.OrderCurrency.CurrencySymbol, ord.OrderCurrency.IsCodeBefore);
                orderItems.OrderDiscount = ord.OrderDiscount;
                orderItems.OrderDiscountValue = ord.OrderDiscountValue;
                orderItems.GroupDiscount = ord.GroupDiscount;
                orderItems.CouponCode = ord.Coupon != null ? ord.Coupon.Code : null;
                hforderShipName.Value = ord.ArchivedShippingName;

                if (Settings1C.Enabled && ord.UseIn1C)
                {
                    chkUseIn1C.Checked = true;
                }

                ckbManagerConfirm.Checked = ord.ManagerConfirmed;

                // Billing ------------------------

                //var billingCustomerContact = new CustomerContact();
                //if (ord.BillingContact != null)
                //{
                //    billingCustomerContact = new CustomerContact
                //    {
                //        Name = ord.BillingContact.Name,
                //        Address = ord.BillingContact.Address,
                //        City = ord.BillingContact.City,
                //        Country = ord.BillingContact.Country,
                //        Region = ord.BillingContact.Zone,
                //        Zip = ord.BillingContact.Zip,
                //        CustomerGuid = ord.OrderCustomer.CustomerID,
                //        CustomField1 = ord.BillingContact.CustomField1,
                //        CustomField2 = ord.BillingContact.CustomField2,
                //        CustomField3 = ord.BillingContact.CustomField3
                //    };
                //}

                //LoadBilling(billingCustomerContact);
                //if (ord.Certificate != null)
                //{
                //    lCertificateCode.Text = ord.Certificate.Code;
                //    pnlCertificateCode.Visible = !string.IsNullOrEmpty(ord.Certificate.Code);
                //}

                //if (ord.OrderCustomer != null)
                //{
                //    hfBillingID.Value = (CustomerService.GetContactId(billingCustomerContact) ?? "New");
                //}
                //else
                //{
                //    hfBillingID.Value = "New";
                //}

                // Shipping ----------------------------------
                //TODO: deal with countries and contacts
                var shippingCustomerContact = new CustomerContact();
                if (ord.OrderCustomer != null)
                {
                    shippingCustomerContact = new CustomerContact
                    {
                        Name = ord.OrderCustomer.FirstName,
                        Street = ord.OrderCustomer.Street,
                        City = ord.OrderCustomer.City,
                        Country = ord.OrderCustomer.Country,
                        Region = ord.OrderCustomer.Region,
                        Zip = ord.OrderCustomer.Zip,
                        CustomerGuid = ord.OrderCustomer == null ? Guid.Empty : ord.OrderCustomer.CustomerID,
                    };
                }

                txtShippingMethod.Text = ord.ArchivedShippingName;
                if (ord.ShippingMethodName ==  "Ёлектронной почтой")
                {
                    hfOrderShippingId.Value = "Email";
                }
                else
                {
                    hfOrderShippingId.Value = ord.ShippingMethodId.ToString();
                }
                hiddenIdOrder.Value = ord.OrderID.ToString();
                //if (ord.OrderCurrency != null)
                //{
                txtShippingPrice.Text = ord.ShippingCost.ToString("F2"); //(ord.ShippingCost / ord.OrderCurrency.CurrencyValue).ToString("F2");
                txtPaymentPrice.Text = ord.PaymentCost.ToString("F2");   //(ord.PaymentCost / ord.OrderCurrency.CurrencyValue).ToString("F2");
                //}

                if (ord.OrderPickPoint != null)
                {
                    // TODO: Add SelectShippingOptionEx
                    //ShippingRates.SelectShippingOptionEx = new ShippingOptionEx()
                    //{
                    //    PickpointId = ord.OrderPickPoint.PickPointId,
                    //    PickpointAddress = ord.OrderPickPoint.PickPointAddress,
                    //    AdditionalData = ord.OrderPickPoint.AdditionalData
                    //};

                    ltPickPointID.Text = ord.OrderPickPoint.PickPointId;
                    ltPickPointAddress.Text = ord.OrderPickPoint.PickPointAddress;

                    if (!string.IsNullOrEmpty(ord.OrderPickPoint.AdditionalData))
                    {
                        hfPickpointAdditional.Value = ord.OrderPickPoint.AdditionalData;
                    }
                }
                else
                {
                    ltPickPointID.Text = "";
                    ltPickPointAddress.Text = "";
                }

                LoadShipping(shippingCustomerContact);

                if (ord.OrderCustomer != null)
                {
                    hfShippingID.Value = (CustomerService.GetContactId(shippingCustomerContact) ?? "New");
                }
                else
                {
                    hfShippingID.Value = "New";
                }

                List<PaymentMethod> listPayments = PaymentService.GetAllPaymentMethods(true).ToList();
                //PaymentMethod cashMethod = new CashOnDelivery(null);

                //if (ord.PaymentMethodId == cashMethod.PaymentMethodID)
                //    listPayments.Add(cashMethod);

                //PaymentMethod pickPointMethod = new PickPoint();

                //if (ord.PaymentMethodId == pickPointMethod.PaymentMethodID)
                //    listPayments.Add(pickPointMethod);

                ddlPaymentMethod.DataSource = listPayments;
                ddlPaymentMethod.DataBind();

                // TODO сделать textbox
                ddlPaymentMethod.SelectedValue = ord.PaymentMethodId.ToString();

                //NOTE: ”зкое место. проверить отображение в различных ситуаци€х
                if (ord.Payed)
                {
                    ddlPaymentMethod.Visible = false;
                    txtPaymentMethod.Text = ord.PaymentMethodName;
                    txtPaymentMethod.Visible = true;
                }
                else
                {
                    ddlPaymentMethod.Visible = true;
                    if (ord.PaymentMethod != null)
                        txtPaymentMethod.Visible = false;
                }
                if (ord.PaymentMethod == null)
                {
                    ddlPaymentMethod.Items.Insert(0, new ListItem(Resource.Admin_NotSet, "0"));
                }

                lblUserComment.Text = string.IsNullOrEmpty(ord.CustomerComment)
                                          ? Resource.Admin_OrderSearch_NoComment
                                          : ord.CustomerComment;

                lblCustomData.Text = ord.CustomData.IsNotEmpty() ? ord.CustomData : Resource.Admin_OrderSearch_NoComment;

                txtAdminOrderComment.Text = string.Format("{0}", ord.AdminOrderComment);
                txtStatusComment.Text = string.Format("{0}", ord.StatusComment);

                paymentDetails.Visible = false;
                btnPrintPaymentDetails.Visible = false;

                if (ord.PaymentMethod != null &&
                    (ord.PaymentMethod is SberBank || ord.PaymentMethod is Bill || ord.PaymentMethod is BillUa ||
                      ord.PaymentMethod is Check || ord.PaymentMethod is Qiwi))
                {
                    if (ord.PaymentMethod is SberBank)
                    {
                        LocalizeClient_OrderConfirmation_OrganizationName.Text = Resource.Admin_EditOrder_CustomerName;
                    }
                    printPaymentDetails.Visible = true;
                    btnPrintPaymentDetails.Visible = true;
                    btnPrintPaymentDetails.Value = ord.PaymentMethod.ButtonText;

                    btnPrintPaymentDetails.Attributes.Add("onclick",
                        string.Format(
                            "javascript:open_printable_version(\'../paymentreceipt/{0}?ordernumber={1}{2}",
                            ord.PaymentMethod.PaymentKey, ord.Number,
                            !(ord.PaymentMethod is Check)
                                ? (string.Format(
                                    "&bill_CompanyName=\' + escape(document.getElementById(\'{0}\').value) + \'&bill_INN=\' + escape(document.getElementById(\'{1}\').value));",
                                    txtCompanyName.ClientID, txtINN.ClientID))
                                : "\');"));

                    if (ord.PaymentMethod is Bill || ord.PaymentMethod is SberBank)
                    {
                        paymentDetails.Visible = true;
                        btnPrintPaymentDetails.Visible = true;
                        if (ord.PaymentDetails != null)
                        {
                            txtCompanyName.Text = ord.PaymentDetails.CompanyName;
                            txtINN.Text = ord.PaymentDetails.INN;
                        }
                    }
                    else if (ord.PaymentMethod is BillUa)
                    {
                        paymentDetails.Visible = true;
                    }
                    else if (ord.PaymentMethod is Qiwi)
                    {
                        printPaymentDetails.Visible = true;
                        qiwiPanel.Visible = true;
                        if (ord.PaymentDetails != null)
                        {
                            txtPhoneQiwi.Text = ord.PaymentDetails.Phone;
                        }
                    }
                }

                if (ord.OrderCertificates == null || ord.OrderCertificates.Count == 0)
                {
                    orderItems.OrderItems = (List<OrderItem>)ord.OrderItems;
                }
                else
                {
                    orderCertificates.Certificates = ord.OrderCertificates;
                    orderCertificates.OrderCurrency = ord.OrderCurrency;
                    orderItems.Visible = false;
                }

                LoadTotal();


                if (ord.PaymentDate != null)
                {
                    pnlOderContent.Enabled = false;
                    lOrderContent.Visible = true;
                }

                pnEmpty.Visible = false;
                pnOrder.Visible = true;
            }
            else
            {
                Response.Redirect("OrderSearch.aspx");
                pnEmpty.Visible = true;
                lblNotFound.Text = @"Not found";
            }


            if (BonusSystem.IsActive)
            {
                if (ord != null)
                {
                    Card bonusCard = null;

                    if (ord.BonusCardNumber != null)
                        bonusCard = BonusSystemService.GetCard(ord.BonusCardNumber);

                    if (bonusCard == null && ord.OrderCustomer != null)
                    {
                        var customer = CustomerService.GetCustomer(ord.OrderCustomer.CustomerID);
                        if (customer != null)
                            bonusCard = BonusSystemService.GetCard(customer.Id);
                    }
                    
                    if (bonusCard != null)
                    {
                        bonusPurchaise.Visible = true;
                        useBonuses.Visible = true;
                        notBonusCard.Visible = false;
                        //bonusPurchaise.Visible = false;
                        //useBonuses.Visible = false;

                        bonusCardBlock.Visible = true;
                        lblBonusCardNumber.Text = bonusCard.CardNumber.ToString();

                        var purchase = BonusSystemService.GetPurchase(bonusCard.CardNumber, ord.Number, ord.OrderID);
                        if (purchase != null)
                            lblBonusCardAmount.Text = purchase.NewBonusAmount.ToString();

                        if (ord.Payed)
                        {
                            chkMakePurchaise.Enabled = false;
                            chkUseBonuses.Enabled = false;
                        }
                    }
                    else
                    {
                        //bonusPurchaise.Visible = false;
                    }
                }
            }

            UpdatePanel1.Update();

            return ord;
        }

        private void LoadCustomer(Guid customerId, OrderCustomer orderCustomer)
        {
            hfContactID.Value = customerId.ToString();
            var customer = CustomerService.GetCustomer(customerId);

            if (customer == null)
            {
                modalPopupCreateUser.Show();
            }

            if (orderCustomer != null)
            {
                txtOrderLastName.Text = orderCustomer.LastName;
                txtOrderFirstName.Text = orderCustomer.FirstName;
                txtOrderEmail.Text = orderCustomer.Email;
                txtOrderMobilePhone.Text = orderCustomer.Phone;
                txtStandardPhone.Text = orderCustomer.StandardPhone.ToString();
            }
            else if (customer != null)
            {
                txtOrderLastName.Text = customer.LastName;
                txtOrderFirstName.Text = customer.FirstName;
                txtOrderEmail.Text = customer.EMail;
                txtOrderMobilePhone.Text = customer.Phone;
                txtStandardPhone.Text = customer.StandardPhone.ToString();
            }

            if (customer != null && customer.RegistredUser)
            {
                hlCustomer.Text = string.Format("{0} {1} - {2} - {3}", customer.FirstName, customer.LastName, customer.EMail, customer.Phone);
                hlCustomer.NavigateUrl = "ViewCustomer.aspx?CustomerID=" + customerId;
                hlCustomer.Visible = true;

                lblGroupDiscount.Text = customer.CustomerGroup != null ? customer.CustomerGroup.GroupName : string.Empty;
                lblChosingCustomer.Visible = false;
            }
            else if (orderCustomer != null)
            {
                lblCustomer.Text = string.Format("{0} {1} - {2} - {3}", orderCustomer.FirstName, orderCustomer.LastName, orderCustomer.Email, orderCustomer.Phone);
                lblCustomer.Visible = true;
                lblChosingCustomer.Visible = false;
                lblChosingCustomer.Visible = false;
            }

            LoadContacts(customerId, false);
        }


        protected void SqlDataSource1_Init(object sender, EventArgs e)
        {
            SqlDataSource1.ConnectionString = Connection.GetConnectionString();
        }

        public string RenderDivHeader()
        {
            string divHeader;
            if (Request.Browser.Browser == "IE")
            {
                var c = new CultureInfo("en-us");
                divHeader = double.Parse(Request.Browser.Version, c.NumberFormat) < 7
                                ? "<div class=\'mtree_ie6\'>"
                                : "<div class=\'mtree_ie\'>";
            }
            else
            {
                divHeader = "<div class=\'mtree\'>";
            }
            return divHeader;
        }

        public string RenderDivBottom()
        {
            return "</div>";
        }

        protected void FillView(CustomerContact contact)
        {
            if (contact != null)
            {
                LoadBilling(contact);
                LoadShipping(contact);
            }
            else
            {
                CleanBilling();
                CleanShipping();
            }
        }

        private void LoadBilling(CustomerContact contact)
        {
            hfBillingID.Value = contact.ContactId.ToString();
            txtBillingAddress.Text = HttpUtility.HtmlDecode(contact.Street);
            txtBillingCity.Text = HttpUtility.HtmlDecode(contact.City);
            txtBillingName.Text = HttpUtility.HtmlDecode(contact.Name);
            txtBillingZip.Text = HttpUtility.HtmlDecode(contact.Zip);
            
            ddlBillingCountry.DataBind();
            if (!string.IsNullOrEmpty(contact.Country))
            {
                ListItem temp = ddlBillingCountry.Items.FindByText(contact.Country);
                if (temp != null)
                {
                    ddlBillingCountry.SelectedValue = temp.Value;
                }
                else
                {
                    ddlBillingCountry.Items.Add(new ListItem(contact.Country, "0"));
                    ddlBillingCountry.SelectedValue = "0";
                }
            }
            else if (ddlBillingCountry.Items.FindByValue(SettingsMain.SellerCountryId.ToString()) != null)
            {
                ddlBillingCountry.SelectedValue = SettingsMain.SellerCountryId.ToString();
            }

            txtBillingZone.Text = HttpUtility.HtmlDecode(contact.Region);
            SetEnabled();
        }

        private void SetEnabled()
        {
            txtBillingAddress.Enabled = !chkCopyAddress.Checked;
            txtBillingCity.Enabled = !chkCopyAddress.Checked;
            txtBillingName.Enabled = !chkCopyAddress.Checked;
            txtBillingZip.Enabled = !chkCopyAddress.Checked;
            txtBillingZone.Enabled = !chkCopyAddress.Checked;
            ddlBillingCountry.Enabled = !chkCopyAddress.Checked;
            txtBillingCustomField1.Enabled = !chkCopyAddress.Checked;
            txtBillingCustomField2.Enabled = !chkCopyAddress.Checked;
            txtBillingCustomField3.Enabled = !chkCopyAddress.Checked;
        }

        private void CleanBilling()
        {
            hfBillingID.Value = string.Empty;
            txtBillingAddress.Text = string.Empty;
            txtBillingCity.Text = string.Empty;
            txtBillingName.Text = string.Empty;
            txtBillingZip.Text = string.Empty;

            txtBillingCustomField1.Text = string.Empty;
            txtBillingCustomField2.Text = string.Empty;
            txtBillingCustomField3.Text = string.Empty;

            if (ddlBillingCountry.Items.FindByValue(SettingsMain.SellerCountryId.ToString()) != null)
                ddlBillingCountry.SelectedValue = SettingsMain.SellerCountryId.ToString();//CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString();
        }

        private void LoadShipping(CustomerContact contact)
        {
            hfShippingID.Value = contact.ContactId.ToString();
            
            var address = contact.Street ?? "";

            if (!string.IsNullOrEmpty(contact.House))
                address += " " + LocalizationService.GetResource("Core.Orders.OrderContact.House") + " " + contact.House;

            if (!string.IsNullOrEmpty(contact.Apartment))
                address += ", " + LocalizationService.GetResource("Core.Orders.OrderContact.Apartment") + " " + contact.Apartment;

            if (!string.IsNullOrEmpty(contact.Structure))
                address += ", " + LocalizationService.GetResource("Core.Orders.OrderContact.Structure") + " " + contact.Structure;

            if (!string.IsNullOrEmpty(contact.Entrance))
                address += ", " + LocalizationService.GetResource("Core.Orders.OrderContact.Entrance") + " " + contact.Entrance;

            if (!string.IsNullOrEmpty(contact.Floor))
                address += ", " + LocalizationService.GetResource("Core.Orders.OrderContact.Floor") + " " + contact.Floor;

            txtShippingAddress.Text = HttpUtility.HtmlDecode(address);
            txtShippingCity.Text = HttpUtility.HtmlDecode(contact.City);
            txtShippingName.Text = HttpUtility.HtmlDecode(contact.Name);
            txtShippingZip.Text = HttpUtility.HtmlDecode(contact.Zip);
            

            ddlShippingCountry.DataBind();
            if (!string.IsNullOrEmpty(contact.Country))
            {
                ListItem item = ddlShippingCountry.Items.FindByText(contact.Country);
                if (item != null)
                {
                    ddlShippingCountry.SelectedValue = item.Value;
                }
                else
                {
                    ddlShippingCountry.Items.Add(new ListItem(contact.Country, "0"));
                    ddlShippingCountry.SelectedValue = "0";
                }
            }
            else if (ddlShippingCountry.Items.FindByValue(SettingsMain.SellerCountryId.ToString()) != null)
            {
                ddlShippingCountry.SelectedValue = SettingsMain.SellerCountryId.ToString();
            }
            txtShippingZone.Text = HttpUtility.HtmlDecode(contact.Region);

            var addressFull = string.Empty;
            addressFull += contact.Country + ",";
            addressFull += contact.Region + ",";
            addressFull += contact.City + ",";
            addressFull += address;

            lnkMap.NavigateUrl = "http://maps.yandex.ru/?text=" + HttpUtility.UrlEncode(addressFull);
        }

        private void CleanShipping()
        {
            hfShippingID.Value = string.Empty;
            txtShippingAddress.Text = string.Empty;
            txtShippingCity.Text = string.Empty;
            txtShippingName.Text = string.Empty;
            txtShippingZip.Text = string.Empty;
            if (ddlShippingCountry.Items.FindByValue(SettingsMain.SellerCountryId.ToString()) != null)
                ddlShippingCountry.SelectedValue = SettingsMain.SellerCountryId.ToString();//CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString();

            txtShippingZone.Text = string.Empty;

            txtShippingCustomField1.Text = string.Empty;
            txtShippingCustomField2.Text = string.Empty;
            txtShippingCustomField3.Text = string.Empty;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(hfContactID.Value))
            //{
            //    msgErr(Resource.Admin_ViewOrder_NoUserError);
            //    return;
            //}
            try
            {
                if (!string.IsNullOrEmpty(hfContactID.Value))
                {
                    new Guid(hfContactID.Value);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                msgErr(Resource.Admin_ViewOrder_NoUserError);
                return;
            }

            try
            {
                Decimal.Parse(txtShippingPrice.Text);
            }
            catch (Exception)
            {
                //if error we don't save
                msgErr(Resource.Admin_OrderSearch_ErrorParseShipping);
                return;
            }

            if (string.IsNullOrEmpty(ddlPaymentMethod.SelectedValue))
            {
                msgErr(Resource.Admin_OrderSearch_SelectPaymentMethod);
                return;
            }

            int shipId = 0;
            if (!string.IsNullOrWhiteSpace(hfOrderShippingId.Value))
            {
                if(hfOrderShippingId.Value != "Email")
                {
                    shipId = Convert.ToInt32(hfOrderShippingId.Value);
                    var shippingMethod = ShippingMethodService.GetShippingMethod(shipId);
                    if (shippingMethod == null && !AddingNewOrder)
                    {
                        msgErr(Resource.Admin_OrderSearch_SelectShippingMethod);
                        return;
                    }
                }
            }
            else
            {
                msgErr(Resource.Admin_OrderSearch_SelectShippingMethod);
                return;
            }
            var orders = OrderService.GetOrder(hiddenIdOrder.Value.TryParseInt());
            if (orderItems.OrderItems.Count == 0 && (orders != null && orders.OrderCertificates.Count == 0))
            {
				msgErr(Resource.Admin_EditOrder_NoOrderItems);
				return;
            }

            if (ddlPaymentMethod.SelectedValue == "0")
            {
                msgErr(Resource.Admin_EditOrder_SelectPayment);
                return;
            }

            if (!string.IsNullOrEmpty(txtStandardPhone.Text) && !Regex.IsMatch(txtStandardPhone.Text, "^\\d+$"))
            {
                msgErr(Resource.Admin_EditOrder_WrongData);

                txtStandardPhone.CssClass = "phoneDest niceTextBox_faild";
                return;
            }
            else
            {
                txtStandardPhone.CssClass = "phoneDest";
            }


            // -- Order starting here
            bool shippingRefresh;
            Order order = BuildOrder(out shippingRefresh, shipId);
            if (order == null)
            {
                msgErr("Order ID invalid");
                return;
            }
            if (AddingNewOrder)
            {
                CreateOrder(order);
            }
            else
            {
                SaveOrder(order, shippingRefresh);
            }

            if (AddingNewOrder)
            {
                Response.Redirect("ViewOrder.aspx?OrderID=" + order.OrderID);
            }
            else
            {
                LoadOrder();
            }
        }

        private void CreateOrder(Order order)
        {
            var customer = new Customer();
            if (string.IsNullOrEmpty(hfContactID.Value))
            {
                var customerGroup = CustomerGroupService.GetCustomerGroupList();
                var customerId = Guid.NewGuid();
                while (CustomerService.ExistsCustomer(customerId))
                    customerId = Guid.NewGuid();
                customer = new Customer
                {
                    Id = customerId,
                    FirstName = txtOrderFirstName.Text,
                    LastName = txtOrderLastName.Text,
                    EMail = txtOrderEmail.Text,
                    Phone = txtOrderMobilePhone.Text,
                    StandardPhone = txtStandardPhone.Text.TryParseLong(true),
                    CustomerGroupId = customerGroup.Count > 0 ? customerGroup.First().CustomerGroupId : 0
                };
                CustomerService.InsertNewCustomer(customer);
                // ? CustomerService.InsertNewCustomer customer.Id ????? ????????? ?? Id ??????? ?????????
                customer.Id = customerId;
            }
            else
            {
                customer = CustomerService.GetCustomer(new Guid(hfContactID.Value));
            }

            if (txtStandardPhone.Text.IsNotEmpty() && !Regex.IsMatch(txtStandardPhone.Text, "^\\d+$"))
            {
                txtStandardPhone.CssClass = "phoneDest niceTextBox_faild";
                return;
            }
            else
            {
                txtStandardPhone.CssClass = "phoneDest";
            }

            //проверка на null
            if (order.OrderCustomer == null)
                order.OrderCustomer = new OrderCustomer
                {
                    CustomerID = customer.Id,
                    CustomerIP = Request.UserHostAddress,
                    FirstName = txtOrderFirstName.Text,
                    LastName = txtOrderLastName.Text,
                    Email = txtOrderEmail.Text,
                    Phone = txtOrderMobilePhone.Text,
                    StandardPhone = txtStandardPhone.Text.TryParseLong(true),
                };
            

            order.GroupName = customer.CustomerGroup != null ? customer.CustomerGroup.GroupName : string.Empty;

            order.OrderStatusId = OrderStatusService.DefaultOrderStatus;
            order.OrderDate = DateTime.Now;
            order.AffiliateID = 0; // For crash protection
            
            order.IsFromAdminArea = true;

            order.OrderID = OrderService.AddOrder(order, new OrderChangedBy(CustomerContext.CurrentCustomer));

            if (order.OrderID == 0)
            {
                msgErr(Resource.Admin_ViewOrder_CreateError);
                return;
            }

            OrderStatusService.ChangeOrderStatus(order.OrderID, OrderStatusService.DefaultOrderStatus, LocalizationService.GetResource("Core.OrderStatus.Created"));

            OrderID = order.OrderID;
            //SaveOrderCart(order.OrderID);
            

            float prodTotal = orderItems.OrderItems.Sum(oi => oi.Price * oi.Amount);
            float totalDiscount = orderItems.OrderDiscount > 0 ? orderItems.OrderDiscount * prodTotal / 100 : 0;
            totalDiscount += orderItems.OrderDiscountValue;

            var orderTable = OrderService.GenerateOrderItemsHtml(orderItems.OrderItems, CurrencyService.CurrentCurrency,
                                                                 prodTotal, order.OrderDiscount, order.OrderDiscountValue, 
                                                                 order.Coupon, order.Certificate, totalDiscount, order.ShippingCost,
                                                                 order.PaymentCost, order.TaxCost, order.BonusCost, 0);

            var orderMailTemplate = new NewOrderMailTemplate(order.Number,
                                                             order.OrderCustomer.Email,
                                                             BuildCustomerContacts(order.OrderCustomer),
                                                             order.ArchivedShippingName +
                                                                (order.OrderPickPoint != null && !String.IsNullOrWhiteSpace(order.OrderPickPoint.PickPointAddress)
                                                                    ? string.Format(" ({0})", order.OrderPickPoint.PickPointAddress)
                                                                    : string.Empty),
                                                             order.PaymentMethodName,
                                                             orderTable, order.OrderCurrency.CurrencyCode,
                                                             order.Sum.ToString(), order.CustomerComment,
                                                             OrderService.GetBillingLinkHash(order),
                                                             order.OrderCustomer.FirstName,
                                                             order.OrderCustomer.LastName);
            orderMailTemplate.BuildMail();

            if (!string.IsNullOrEmpty(customer.EMail))
                SendMail.SendMailNow(customer.Id, customer.EMail, orderMailTemplate.Subject, orderMailTemplate.Body, true);
            
            SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForOrders, orderMailTemplate.Subject, orderMailTemplate.Body, true);

            if (BonusSystem.IsActive && (chkMakePurchaise.Checked || chkUseBonuses.Checked) && order.BonusCardNumber.HasValue)
            {
                var purchase = BonusSystemService.GetPurchase(order.BonusCardNumber, order.Number, order.OrderID);
                var sumPrice = BonusSystem.BonusType == EBonusType.ByProductsCostWithShipping
                        ? prodTotal - totalDiscount + order.ShippingCost
                        : prodTotal - totalDiscount;

                if (purchase == null)
                {
                    BonusSystemService.MakeBonusPurchase(order.BonusCardNumber.Value, (decimal)prodTotal, (decimal)sumPrice, order);
                }
                else
                {
                    throw new Exception(string.Format("purchase {0} is exist for card {1}", purchase.OrderId, purchase.CardId));
                }
            }
        }

        private static string BuildCustomerContacts(OrderCustomer customer)
        {
            var sb = new StringBuilder();

            sb.AppendFormat(Resource.Client_Registration_Name + " {0}<br/>", customer.FirstName);
            sb.AppendFormat(Resource.Client_Registration_Surname + " {0}<br/>", customer.LastName);
            sb.AppendFormat(Resource.Client_Registration_Country + " {0}<br/>", customer.Country);
            sb.AppendFormat(Resource.Client_Registration_State + " {0}<br/>", customer.Region);
            sb.AppendFormat(Resource.Client_Registration_City + " {0}<br/>", customer.City);
            sb.AppendFormat(Resource.Client_Registration_Zip + " {0}<br/>", customer.Zip);
            sb.AppendFormat(Resource.Client_Registration_Address + ": {0}<br/>", string.IsNullOrEmpty(customer.GetCustomerAddress())
                                                                                              ? Resource.Client_OrderConfirmation_NotDefined
                                                                                              : customer.GetCustomerAddress());
            return sb.ToString();
        }


        private void SaveOrder(Order order, bool shippingRefresh)
        {
			var changedBy = new OrderChangedBy(CustomerContext.CurrentCustomer);
			
            order.OrderID = Convert.ToInt32(OrderID);
            order.Number = OrderNumber;
            
            // -- Save main info
            OrderService.UpdateOrderMain(order, changedBy: changedBy);
            OrderService.RefreshTotal(order);

            if (order.OrderPickPoint != null && order.OrderPickPoint.PickPointId == "delete")
            {
                OrderService.DeleteOrderPickPoint(order.OrderID);
            }
            else if (order.OrderPickPoint != null)
            {
                OrderService.AddUpdateOrderPickPoint(order.OrderID, order.OrderPickPoint);
            }
            else
            {
                OrderService.DeleteOrderPickPoint(order.OrderID);
            }

            OrderID = order.OrderID;
            

            // -- Order currency
            OrderService.UpdateOrderCurrency(order.OrderID, order.OrderCurrency.CurrencyCode, order.OrderCurrency.CurrencyValue, changedBy);

            OrderService.UpdateOrderCustomer(order.OrderCustomer, changedBy);
            
            OrderService.UpdatePaymentDetails(order.OrderID, order.PaymentDetails, changedBy);

            shippingRefresh |= orderItems.OrderItems.AggregateHash() != order.OrderItems.AggregateHash();

            SaveOrderCart(order.OrderID, order.OrderItems, order.OrderStatus, changedBy);


            if (BonusSystem.IsActive && (chkMakePurchaise.Checked || chkUseBonuses.Checked) && order.BonusCardNumber.HasValue)
            {
                var purchase = BonusSystemService.GetPurchase(order.BonusCardNumber, order.Number, order.OrderID);
                float prodTotal = orderItems.OrderItems.Sum(oi => oi.Price * oi.Amount);
                float totalDiscount = orderItems.OrderDiscount > 0 ? orderItems.OrderDiscount * prodTotal / 100 : 0;

                if(chkUseBonuses.Checked)
                {
                    var bonusCard = BonusSystemService.GetCard(order.BonusCardNumber.Value);
                    if (bonusCard != null && bonusCard.BonusAmount > 0)
                    {
                        totalDiscount += BonusSystemService.GetBonusCost(prodTotal + order.ShippingCost, prodTotal, (float)bonusCard.BonusAmount);                        
                    }                    
                }

                var sumPrice = BonusSystem.BonusType == EBonusType.ByProductsCostWithShipping
                    ? prodTotal - totalDiscount + order.ShippingCost
                    : prodTotal - totalDiscount;

                if (purchase == null)
                {
                    BonusSystemService.MakeBonusPurchase(order.BonusCardNumber.Value, (decimal)prodTotal, (decimal)sumPrice, order);
                }
                else if(purchase.PurchaseAmount != (decimal)sumPrice)
                {
                    BonusSystemService.UpdatePurchase(order.BonusCardNumber.Value, (decimal)prodTotal, (decimal)sumPrice, order);
                }
            }

            if (shippingRefresh)
                modalRecheckShipping.Show();
        }


        private Order BuildOrder(out bool shippingRefresh, int shippingMethodId)
        {
            Order order = AddingNewOrder ? new Order() : OrderService.GetOrder(OrderID);
            order.PaymentMethodId = Convert.ToInt32(ddlPaymentMethod.SelectedValue);
            order.ShippingMethodId = shippingMethodId;
            order.AdminOrderComment = txtAdminOrderComment.Text;
            order.StatusComment = txtStatusComment.Text;
            order.ShippingCost = PriceService.RoundPrice(txtShippingPrice.Text.TryParseFloat(), orderItems.Currency, orderItems.Currency.Rate); //  * orderItems.CurrencyValue

            var shipping = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shipping != null)
                order.ShippingTaxType = shipping.TaxType;

            order.UseIn1C = Settings1C.Enabled && chkUseIn1C.Checked;
            order.ManagerConfirmed = ckbManagerConfirm.Checked;

            var currency = CurrencyService.GetCurrencyByIso3(orderItems.CurrencyCode);

            order.OrderCurrency = new OrderCurrency
            {
                CurrencyCode = orderItems.CurrencyCode,
                CurrencyValue = orderItems.CurrencyValue,
                CurrencyNumCode = orderItems.CurrencyNumCode,
                CurrencySymbol = orderItems.CurrencySymbol,
                RoundNumbers = currency != null ? currency.RoundNumbers : 0,
                EnablePriceRounding = currency.EnablePriceRounding
            };

            foreach (var orderItem in orderItems.OrderItems)
            {
                if (!order.OrderItems.Contains(orderItem))
                    order.OrderItems.Add(orderItem);
            }

            var payment = PaymentService.GetPaymentMethod(order.PaymentMethodId);
            if (payment != null)
            {
                txtPaymentPrice.Text = (payment.ExtrachargeType == ExtrachargeType.Fixed
                    ? payment.Extracharge
                    : payment.Extracharge / 100 *
                      (orderItems.OrderItems.Sum(item => item.Price * item.Amount) -
                       (orderItems.OrderItems.Sum(item => item.Price * item.Amount) / 100 * orderItems.OrderDiscount)
                       - order.BonusCost + order.ShippingCost
                       + order.Taxes.Where(tax => !tax.ShowInPrice).Sum(tax => tax.Sum))).ToString("F2");
                order.PaymentCost = payment.ExtrachargeType == ExtrachargeType.Percent
                    ? PriceService.RoundPrice(txtPaymentPrice.Text.TryParseFloat(), orderItems.Currency, orderItems.Currency.Rate)
                    : PriceService.RoundPrice(txtPaymentPrice.Text.TryParseFloat(), order.OrderCurrency, CurrencyService.CurrentCurrency.Rate);
            }
            else
            {
                order.PaymentCost = PriceService.RoundPrice(txtPaymentPrice.Text.TryParseFloat(), orderItems.Currency, orderItems.Currency.Rate);
            }

            if (order.PaymentDetails == null)
                order.PaymentDetails = new PaymentDetails();

            order.PaymentDetails.CompanyName = txtCompanyName.Text;
            order.PaymentDetails.INN = txtINN.Text;
            order.PaymentDetails.Phone = txtPhoneQiwi.Text;

            order.ArchivedShippingName = txtShippingMethod.Text;

            if (!string.IsNullOrEmpty(ltPickPointID.Text) || !string.IsNullOrEmpty(ltPickPointAddress.Text) || !string.IsNullOrEmpty(hfPickpointAdditional.Value))
            {
                order.OrderPickPoint = new OrderPickPoint
                {
                    PickPointId = ltPickPointID.Text,
                    PickPointAddress = ltPickPointAddress.Text.IsNotEmpty() ? ltPickPointAddress.Text : txtShippingAddress.Text
                };

                if (!string.IsNullOrEmpty(hfPickpointAdditional.Value))
                {
                    order.OrderPickPoint.AdditionalData = hfPickpointAdditional.Value;
                }
            }
            else
            {
                order.OrderPickPoint = new OrderPickPoint { PickPointId = "delete" };
            }
            
            order.OrderDiscount = orderItems.OrderDiscount;
            order.OrderDiscountValue = orderItems.OrderDiscountValue;

            if (order.OrderCustomer == null)
                order.OrderCustomer = new OrderCustomer();

            var orderCustomer = order.OrderCustomer.DeepClone();

            order.OrderCustomer.CustomerID = string.IsNullOrEmpty(hfContactID.Value) ? Guid.NewGuid() : new Guid(hfContactID.Value);
            order.OrderCustomer.FirstName = txtOrderFirstName.Text;
            order.OrderCustomer.LastName = txtOrderLastName.Text;
            order.OrderCustomer.Email = txtOrderEmail.Text;
            order.OrderCustomer.Phone = txtOrderMobilePhone.Text;
            order.OrderCustomer.StandardPhone = txtStandardPhone.Text.TryParseLong(true);

            order.OrderCustomer.Country = ddlShippingCountry.SelectedItem.Text;
            order.OrderCustomer.Region = txtShippingZone.Text;
            order.OrderCustomer.City = txtShippingCity.Text;
            order.OrderCustomer.Street = txtShippingAddress.Text;
            order.OrderCustomer.Zip = txtShippingZip.Text;
            order.OrderCustomer.CustomField1 = txtShippingCustomField1.Text;
            order.OrderCustomer.CustomField2 = txtShippingCustomField2.Text;
            order.OrderCustomer.CustomField3 = txtShippingCustomField3.Text;
            
            shippingRefresh = !AddingNewOrder && !ContactChanged(orderCustomer, order.OrderCustomer);
            
            order.OrderDate = GetDate();
            

            orderItems.SetCustomerDiscount(order.OrderCustomer.CustomerID);

            order.OrderSourceId = Convert.ToInt32(ddlOrdertype.SelectedValue);

            Card bonusCard = null;
            if (BonusSystem.IsActive)
            {
                if (order.BonusCardNumber != null)
                    bonusCard = BonusSystemService.GetCard(order.BonusCardNumber);

                if (bonusCard == null)
                {
                    var customer = CustomerService.GetCustomer(order.OrderCustomer.CustomerID);
                    if (customer != null)
                        bonusCard = BonusSystemService.GetCard(customer.Id);
                }

                if (bonusCard != null && chkUseBonuses.Checked && bonusCard.BonusAmount > 0)
                {
                    var totalPrice = orderItems.OrderItems.Sum(oi => oi.Price * oi.Amount);
                    var discount = orderItems.OrderDiscount > 0 ? orderItems.OrderDiscount * totalPrice / 100 : 0;
                    discount += orderItems.OrderDiscountValue;

                    order.BonusCost =
                        BonusSystemService.GetBonusCost(totalPrice - discount + order.ShippingCost, totalPrice - discount, (float)bonusCard.BonusAmount);
                    order.BonusCardNumber = bonusCard.CardNumber;
                }
                else if (bonusCard != null)
                {
                    order.BonusCardNumber = bonusCard.CardNumber;
                }
            }

            return order;
        }

        private bool ContactChanged(OrderCustomer shippingCustomer, OrderCustomer orderCustomer)
        {
            if (shippingCustomer == null || orderCustomer == null)
                return true;

            return shippingCustomer.Country == orderCustomer.Country
                   && shippingCustomer.City == orderCustomer.City
                   && shippingCustomer.Zip == orderCustomer.Zip;
        }

        protected void agv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectContact")
            {
                string[] values = ((string)e.CommandArgument).Split('^');
                SelectCustomer(values[0].TryParseGuid(), values[1], true);
            }
        }

        protected void SelectCustomer(Guid customerID, string customerEmail, bool fillView)
        {
            hfContactID.Value = customerID.ToString();
            lblChosingCustomer.Text = customerEmail;
            LoadContacts(customerID, fillView);
        }

        private void LoadContacts(Guid customerID, bool fillView)
        {
            List<CustomerContact> contacts = CustomerService.GetCustomerContacts(customerID);
            if (fillView)
            {
                FillView(contacts.Count > 0 ? contacts.First() : null);
            }

            if (contacts.Count == 0)
            {
                ErrMes.Text = string.Empty;
                ErrMes.Visible = true;
            }

            CustomerContacts.Items.Clear();
            var liNew = new ListItem
            {
                Value = "New",
                Text = Resource.Admin_OrderSearch_NewAddress
            };

            CustomerContacts.Items.Add(liNew);
            const string format = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>{0}</b>&nbsp;{1}<br />";
            foreach (var customerRow in contacts)
            {
                var liText = new StringBuilder();
                liText.AppendFormat(
                    "&nbsp;<b>{0}:</b>&nbsp;{1}<br />",
                    Resource.Admin_ViewCustomer_ContactPerson, customerRow.Name);
                liText.AppendFormat(format,
                                    Resource.Admin_ViewCustomer_ContactCountry, customerRow.Country);
                liText.AppendFormat(format,
                                    Resource.Admin_ViewCustomer_ContactCity, customerRow.City);

                if (!string.IsNullOrEmpty(customerRow.Region.Trim()))
                {
                    liText.AppendFormat(format,
                                        Resource.Admin_ViewCustomer_ContactZone, customerRow.Region);
                }

                if (!string.IsNullOrEmpty(customerRow.Zip.Trim()))
                {
                    liText.AppendFormat(format,
                                        Resource.Admin_ViewCustomer_ContactZip, customerRow.Zip);
                }
                liText.AppendFormat(format,
                                    Resource.Admin_ViewCustomer_ContactAddress, customerRow.Street);

                var li = new ListItem { Text = liText.ToString(), Value = customerRow.ContactId.ToString() };
                CustomerContacts.Items.Add(li);
            }

            var customer = CustomerService.GetCustomer(customerID);
            if (customer != null)
            {
                var group = CustomerGroupService.GetCustomerGroup(customer.CustomerGroupId);
                // јпдейтим им€ группы и скидку группы
                Order order = OrderService.GetOrder(OrderID);
                if (order != null && group != null && (order.GroupName != group.GroupName || order.GroupDiscount != group.GroupDiscount))
                {
                    order.GroupName = group.GroupName;
                    order.GroupDiscount = group.GroupDiscount;
                    OrderService.UpdateOrderMain(order);
                }
                orderItems.SetCustomerDiscount(customerID);
            }
            UpdatePanel4.Update();
        }

        private void msgErr_createUser(string messageText)
        {
            pnlMsgErr.Visible = true;
            Message.Text = "<br/>" + messageText;
        }

        private void msgErr(string messageText)
        {
            pnlMsgErr.Visible = true;
            MsgErr.Text = "<br/>" + messageText;
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            if (ValidateCustomer())
            {
                var customerId = CustomerService.InsertNewCustomer(new Customer
                {
                    Id = CustomerId,
                    Password = txtPassword.Text,
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Phone = txtPhone.Text,
                    StandardPhone = StringHelper.ConvertToStandardPhone(txtPhone.Text),
                    SubscribedForNews = chkSubscribed4News.Checked,
                    EMail = txtEmail.Text.Trim(),
                    CustomerRole = (Role)SQLDataHelper.GetInt(ddlCustomerRole.SelectedValue),
                    CustomerGroupId = SQLDataHelper.GetInt(ddlCustomerGroup.SelectedValue)
                });
                if (!customerId.Equals(Guid.Empty))
                {
                    ClearCustomer();
                    var customer = CustomerService.GetCustomer(customerId);
                    //SelectCustomer(customer.Id, customer.EMail, true);
                    LoadCustomer(customer.Id, new OrderCustomer
                    {
                        CustomerID = customerId,
                        Email = customer.EMail,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Phone = customer.Phone,
                        StandardPhone = customer.StandardPhone,
                        OrderID = OrderID
                    });

                    modalPopupCreateUser.Hide();
                    UpdatePanel1.Update();

                    // Bind user grid
                    hfBillingID.Value = "New";
                    hfShippingID.Value = "New";
                }
                else
                {
                    msgErr_createUser(Resource.Admin_ViewOrder_UserCreateError);
                    //bad thing happens. notify user about this
                }
            }
            else
            {
                msgErr_createUser(Resource.Admin_ViewOrder_PwdConfirmError);
            }
        }

        private void ClearCustomer()
        {
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtPasswordConfirm.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtPhone.Text = string.Empty;
            chkSubscribed4News.Checked = false;
        }

        private bool ValidateCustomer()
        {
            bool boolIsValidPast = true;

            ulUserRegistarionValidation.InnerHtml = "";

            // ------------------------------------------------------

            string email = txtEmail.Text.Trim();
            if ((!string.IsNullOrEmpty(email)) && ValidationHelper.IsValidEmail(email) && (!CustomerService.ExistsEmail(email)))
            {
                txtEmail.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtEmail.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            if (!string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text.Length > 3)
            {
                txtPassword.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtPassword.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            if (string.IsNullOrEmpty(txtPasswordConfirm.Text) == false)
            {
                txtPasswordConfirm.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtPasswordConfirm.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            if ((string.IsNullOrEmpty(txtPasswordConfirm.Text) == false) &&
                (string.IsNullOrEmpty(txtPassword.Text) == false) && (txtPassword.Text == txtPasswordConfirm.Text))
            {
                txtPassword.CssClass = "OrderConfirmation_ValidTextBox";
                txtPasswordConfirm.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtPassword.CssClass = "OrderConfirmation_InvalidTextBox";
                txtPasswordConfirm.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            if (string.IsNullOrEmpty(txtFirstName.Text) == false)
            {
                txtFirstName.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtFirstName.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            if (string.IsNullOrEmpty(txtLastName.Text) == false)
            {
                txtLastName.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtLastName.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            // ------------------------------------------------------

            if (!boolIsValidPast)
            {
                ulUserRegistarionValidation.Visible = true;
                ulUserRegistarionValidation.InnerHtml += string.Format("<li>{0}</li>", Resource.Client_OrderConfirmation_EnterEmptyField);
            }
            else
                ulUserRegistarionValidation.Visible = false;
            return boolIsValidPast;
        }

        protected void btnSelectAddress_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CustomerContacts.SelectedValue))
            {
                switch (hfTypeBindAddress.Value)
                {
                    case "billing":
                        chkCopyAddress.Checked = false;
                        if (CustomerContacts.SelectedValue != "New")
                        {
                            LoadBilling(CustomerService.GetCustomerContact(CustomerContacts.SelectedValue));
                        }
                        else
                        {
                            CleanBilling();
                            hfBillingID.Value = "New";
                        }
                        break;
                    case "shipping":
                        if (CustomerContacts.SelectedValue != "New")
                        {
                            LoadShipping(CustomerService.GetCustomerContact(CustomerContacts.SelectedValue));
                            if (chkCopyAddress.Checked)
                            {
                                LoadBilling(CustomerService.GetCustomerContact(CustomerContacts.SelectedValue));
                            }
                        }
                        else
                        {
                            CleanShipping();
                            hfShippingID.Value = "New";
                        }
                        break;
                }
                UpdatePanel1.Update();
            }
        }

        //protected void SaveOrderCart(int orderId)
        //{
        //    OrderService.AddUpdateOrderItems(orderItems.OrderItems, orderId, new OrderChangedBy(CustomerContext.CurrentCustomer));
        //}

        protected void SaveOrderCart(int orderId, List<OrderItem> oldItems, OrderStatus status, OrderChangedBy changedBy)
        {
            OrderService.AddUpdateOrderItems(orderItems.OrderItems, oldItems, orderId, changedBy);

            if (status != null && status.Command == OrderStatusCommand.Increment)
            {
                OrderService.IncrementProductsCountAccordingOrder(orderId);
            }
            else if (status != null && status.Command == OrderStatusCommand.Decrement)
            {
                OrderService.DecrementProductsCountAccordingOrder(orderId);
            }
        }

        protected void orderItems_Updated(object sender, EventArgs args)
        {
            LoadTotal();
            //txtShippingPrice.Text = (txtShippingPrice.Text.TryParseFloat() / (orderItems.CurrencyValue / orderItems.OldCurrencyValue)).ToString("F2");
        }

        private DateTime GetDate()
        {
            DateTime d;
            if (DateTime.TryParse(lOrderDate.Text, out d))
            {
                var hours = txtOrderTime.Text.Split(":").FirstOrDefault().TryParseInt();
                var minutes = txtOrderTime.Text.Split(":").LastOrDefault().TryParseInt();
                if (hours < 0 || hours > 23)
                {
                    hours = 0;
                }

                if (minutes < 0 || minutes > 59)
                {
                    minutes = 0;
                }
                return new DateTime(d.Year, d.Month, d.Day, hours, minutes, 0);
            }

            return DateTime.Now;
        }

        protected void btnSelectShipping_Click(object sender, EventArgs e)
        {
            LoadShippingMethods();
            ShippingRates.LoadMethods();
			
            if (ShippingRates.SelectedItem == null)
                return;
			
            float shipPrice = 0;

            //var ord = OrderService.GetOrder(OrderID);
            //if (ord == null || ord.PaymentMethod == null || ord.PaymentMethod.Type != PaymentType.CashOnDelivery || ShippingRates.SelectedItem.Ext == null)
            //{
            //    shipPrice = ShippingRates.SelectedItem.Rate;
            //}
            //else
            //{
            //    shipPrice = ShippingRates.SelectedItem.Ext.PriceCash;
            //}
            shipPrice = ShippingRates.SelectedItem.Rate;

            hfOrderShippingId.Value = ShippingRates.SelectedItem.MethodId.ToString();
            txtShippingPrice.Text = shipPrice.ToString("F2"); //(shipPrice / orderItems.CurrencyValue).ToString("F2");
            txtShippingMethod.Text = ShippingRates.SelectedItem.NameRate ?? ShippingRates.SelectedItem.Name;

            if (ShippingRates.SelectShippingOptionEx != null)
            {
                ltPickPointID.Text = ShippingRates.SelectShippingOptionEx.PickpointId;
                ltPickPointAddress.Text = ShippingRates.SelectShippingOptionEx.PickpointAddress;
                hfPickpointAdditional.Value = ShippingRates.SelectShippingOptionEx.AdditionalData;
            }
            else
            {
                ltPickPointID.Text = string.Empty;
                ltPickPointAddress.Text = string.Empty;
                hfPickpointAdditional.Value = string.Empty;
            }

            if ((ShippingRates.SelectedItem is CheckoutOption) && (ltPickPointID.Text.IsNullOrEmpty() &&
                ltPickPointAddress.Text.IsNullOrEmpty() && hfPickpointAdditional.Value.IsNullOrEmpty()))
            {
                var pickPoint = ((CheckoutOption)ShippingRates.SelectedItem).GetOrderPickPoint();
                ltPickPointID.Text = pickPoint.PickPointId;
                ltPickPointAddress.Text = pickPoint.PickPointAddress;
                hfPickpointAdditional.Value = pickPoint.AdditionalData;
            }

            LoadTotal();
            modalShipping.Hide();
        }

        protected void lbChangeShipping_Click(object sender, EventArgs e)
        {
			if (string.IsNullOrEmpty(txtShippingCity.Text) || string.IsNullOrEmpty(txtShippingZone.Text))
            {
                msgErr(Resource.Admin_EditOrder_NotSelectShippingItem);
                return;
            }
            LoadShippingMethods();
            ShippingRates.LoadMethods();
            modalShipping.Show();
        }

        private void LoadShippingMethods()
        {
            ShippingRates.Country = ddlShippingCountry.SelectedItem.Text;
            ShippingRates.Region = txtShippingZone.Text;
            ShippingRates.City = txtShippingCity.Text;
            ShippingRates.Zip = txtShippingZip.Text;
            ShippingRates.Currency = orderItems.Currency;
			ShippingRates.SelectedPaymentId = ddlPaymentMethod.SelectedValue.TryParseInt();
            ShippingRates.OrderItems = orderItems.OrderItems;
            float prodTotal = orderItems.OrderItems.Sum(oi => oi.Price * oi.Amount);
            ShippingRates.TotalDiscount = orderItems.OrderDiscount > 0 ? orderItems.OrderDiscount * prodTotal / 100 : 0;
            //var shoppingCart = new ShoppingCart();
            //shoppingCart.AddRange(
            //    from item in orderItems.OrderItems
            //    where item.ProductID != null && item.ProductID != 0
            //    select new ShoppingCartItem
            //    {
            //        OfferId = ProductService.GetProduct((int)item.ProductID).Offers.First().OfferId,
            //        Amount = item.Amount,
            //        Price = item.Price
            //    });

            //ShippingRates.ShoppingCart = shoppingCart;
        }


        protected void chkUseBonuses_CheckedChanged(object sender, EventArgs e)
        {
            LoadTotal();
        }
    }
}