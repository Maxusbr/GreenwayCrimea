//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using Resources;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Orders;
using AdvantShop.Shipping;

namespace Admin
{
    public partial class ViewOrder : AdvantShopAdminPage
    {
        protected int OrderId = 0;
        protected string OrderNumber;
        protected string OrderCode;
        protected bool IsPaid;
        protected float CurrencyValue;
        protected string CurrencyCode;

        protected Order order;

        protected bool ShippingTypeIsSdek = false;
        protected bool ShippingTypeIsCheckout = false;
        protected bool ShippingTypeIsYandexDelivery = false;
        protected OrderPickPoint OrderPickPoint;

        private DateTime PrevDate = DateTime.MinValue;
        protected bool ShowGroup = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            blockManagers.Visible = SettingsCheckout.EnableManagersModule
                && (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveCrm));
            if (!int.TryParse(Request["orderid"], out OrderId))
            {
                OrderId = OrderService.GetLastOrderId();

            }
            SetMeta(string.Format("{0} - {1} {2}", SettingsMain.ShopName, Resource.Admin_ViewOrder_ItemNum, OrderId));
            LoadOrder();
        }

        private void LoadOrder()
        {
            order = OrderService.GetOrder(OrderId);
            if (order == null)
                Response.Redirect("OrderSearch.aspx");

            lnkExportToExcel.NavigateUrl = "HttpHandlers/ExportOrderExcel.ashx?OrderID=" + order.OrderID;
            lnkEditOrder.NavigateUrl = "EditOrder.aspx?OrderID=" + order.OrderID;

            OrderNumber = order.Number;
            OrderCode = order.Code.ToString();
            lblOrderId.Text = order.Number;
            lblOrderDate.Text = AdvantShop.Localization.Culture.ConvertDate(order.OrderDate);
            //lblOrderNumber.Text = order.Number;
            
            lnkDelete.Attributes.Add("data-confirm", string.Format(Resource.Admin_OrderSearch_DeleteOrder, HttpUtility.HtmlEncode(order.Number)));
            IsPaid = order.PaymentDate != null && order.PaymentDate != DateTime.MinValue;

            if (order.OrderCurrency != null)
            {
                CurrencyValue = order.OrderCurrency.CurrencyValue;
                CurrencyCode = order.OrderCurrency.CurrencyCode;
            }

            if (order.OrderCustomer != null)
            {
                var customer = CustomerService.GetCustomer(order.OrderCustomer.CustomerID);
                if (customer != null && customer.Id != Guid.Empty)
                {
                    lnkCustomerName.Text = StringHelper.AggregateStrings(" ", order.OrderCustomer.FirstName, order.OrderCustomer.Patronymic, order.OrderCustomer.LastName);
                    lnkCustomerName.NavigateUrl = @"viewcustomer.aspx?customerid=" + order.OrderCustomer.CustomerID;
                    lnkCustomerEmail.Text = order.OrderCustomer.Email;
                    lnkCustomerEmail.NavigateUrl = "mailto:" + order.OrderCustomer.Email;
                }
                else
                {
                    lblCustomerEmail.Text = order.OrderCustomer.Email;
                    lblCustomerName.Text = StringHelper.AggregateStrings(" ", order.OrderCustomer.FirstName, order.OrderCustomer.Patronymic, order.OrderCustomer.LastName);
                }
                lbSendLetterToCustomer.Visible = !string.IsNullOrWhiteSpace(order.OrderCustomer.Email) &&
                                                  ValidationHelper.IsValidEmail(order.OrderCustomer.Email);

                lbSendLetterToCustomer.Text = string.Format("(<a href=\"javascript: void(0)\" class=\"send-letter order-email\" data-orderid='{0}'>послать письмо</a>)", order.OrderID);

                long? standardPhone = StringHelper.ConvertToStandardPhone(order.OrderCustomer.Phone);

                lblCustomerPhone.Text = order.OrderCustomer.Phone;

                if (SettingsTelephony.PhonerLiteActive &&
                    (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveTelephony)) &&
                    standardPhone.HasValue)
                {
                    lblCustomerPhone.Text = string.Format("<a href=\"callto:{0}\">{1}</a>", standardPhone, order.OrderCustomer.Phone);
                }

                lblCustomerPhone.Text += standardPhone.HasValue
                                            ? " " + IPTelephonyOperator.Current.RenderCallButton(standardPhone, ECallButtonType.Big)
                                            : string.Empty;
            }

            if (order.OrderCustomer != null)
            {
                lblBuyerCountry.Text = lblShippingCountry.Text = order.OrderCustomer.Country;
                lblBuyerRegion.Text = lblShippingRegion.Text = order.OrderCustomer.Region;
                lblBuyerCity.Text = lblShippingCity.Text = order.OrderCustomer.City;
                lblBuyerZip.Text = lblShippingZipCode.Text = order.OrderCustomer.Zip;
                lblBuyerAddress.Text = lblShippingAddress.Text = order.OrderCustomer.GetCustomerAddress();

                if (!string.IsNullOrEmpty(order.OrderCustomer.Country) && !string.IsNullOrEmpty(order.OrderCustomer.City) && 
                    !string.IsNullOrEmpty(order.OrderCustomer.Region) && !string.IsNullOrEmpty(order.OrderCustomer.Street))
                {
                    lnkBuyerAddressOnMap.NavigateUrl = lnkShippingAddressOnMap.NavigateUrl =
                            (SettingsCheckout.PrintOrder_MapType == "googlemap"
                                ? "https://maps.google.com/maps?ie=UTF8&z=15&q="
                                : "http://maps.yandex.ru/?text=") +
                            HttpUtility.UrlEncode(StringHelper.AggregateStrings(",", order.OrderCustomer.Country,
                                order.OrderCustomer.Region, order.OrderCustomer.City, order.OrderCustomer.Street + " " + order.OrderCustomer.House));
                }
                else
                {
                    lnkBuyerAddressOnMap.Visible = lnkShippingAddressOnMap.Visible = false;
                }
            }

            lblShippingMethodName.Text = order.ArchivedShippingName + (order.OrderPickPoint != null ? "<br />" + order.OrderPickPoint.PickPointAddress : "");
            lblPaymentMethodName.Text = order.PaymentMethodName;

            var statusesList = OrderStatusService.GetOrderStatuses();
            if (statusesList != null && statusesList.Any(status => status.StatusID == order.OrderStatus.StatusID))
            {
                ddlViewOrderStatus.DataSource = statusesList.OrderBy(item => item.SortOrder);
                ddlViewOrderStatus.DataBind();
                ddlViewOrderStatus.SelectedValue = order.OrderStatus.StatusID.ToString();
            }
            else
            {
                ddlViewOrderStatus.Items.Add(new ListItem(order.OrderStatus.StatusName, order.OrderStatus.StatusID.ToString()));
                ddlViewOrderStatus.SelectedValue = order.OrderStatus.StatusID.ToString();
            }
            ddlViewOrderStatus.Attributes["data-orderid"] = order.OrderID.ToString();

            if (CustomerContext.CurrentCustomer.IsAdmin || CustomerContext.CurrentCustomer.IsModerator || CustomerContext.CurrentCustomer.HasRoleAction(RoleAction.Crm))
            {
                ddlViewOrderManager.Visible = true;
                ddlViewOrderManager.Items.Add(new ListItem("-", ""));
                foreach (var manager in ManagerService.GetManagersList())
                    ddlViewOrderManager.Items.Add(new ListItem(string.Format("{0} {1}", manager.FirstName, manager.LastName), manager.ManagerId.ToString()));

                if (order.ManagerId.HasValue)
                    ddlViewOrderManager.SelectedValue = order.ManagerId.Value.ToString();

                ddlViewOrderManager.Attributes["data-orderid"] = order.OrderID.ToString();
            }
            else
            {
                if (order.ManagerId.HasValue)
                {
                    var manager = order.ManagerId.HasValue ? ManagerService.GetManager((int)order.ManagerId) : null;
                    lblOrderManager.Text = manager != null
                        ? string.Format("{0} {1}", manager.FirstName, manager.LastName)
                        : "-";
                }
            }

            var orderSource = OrderSourceService.GetOrderSource(order.OrderSourceId);
            if (orderSource != null)
            {
                lblOrderType.Text = orderSource.Name;
            }

            pnlOrderNumber.Attributes["style"] = "border-left-color: #" + order.OrderStatus.Color;

            if (order.OrderCertificates == null || order.OrderCertificates.Count == 0)
            {
                lvOrderItems.DataSource = order.OrderItems;
                lvOrderItems.DataBind();
                lvOrderCertificates.Visible = false;
            }
            else
            {
                lvOrderCertificates.DataSource = order.OrderCertificates;
                lvOrderCertificates.DataBind();
                lvOrderItems.Visible = false;
            }

            lblUserComment.Text = string.IsNullOrEmpty(order.CustomerComment)
                                        ? Resource.Admin_OrderSearch_NoComment
                                        : order.CustomerComment;

            txtAdminOrderComment.Text = string.Format("{0}", order.AdminOrderComment);
            txtStatusComment.Text = string.Format("{0}", order.StatusComment);

            txtStatusComment.Attributes["data-orderid"] = order.OrderID.ToString();
            txtAdminOrderComment.Attributes["data-orderid"] = order.OrderID.ToString();

            txtStatusComment.Attributes["data-currentvalue"] = order.StatusComment;
            txtAdminOrderComment.Attributes["data-currentvalue"] = order.AdminOrderComment;

            txtTrackNumber.Text = order.TrackNumber;
            txtTrackNumber.Attributes["data-orderid"] = order.OrderID.ToString();
            txtTrackNumber.Attributes["data-currentvalue"] = order.TrackNumber;

            var shipping = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shipping != null)
            {
                OrderPickPoint = order.OrderPickPoint;

                liMultiship.Visible = shipping.ShippingType == "Multiship";

                ShippingTypeIsSdek = shipping.ShippingType == "Sdek";
                ShippingTypeIsCheckout = lblCheckoutAdressNotice.Visible = shipping.ShippingType == "CheckoutRu";
                ShippingTypeIsYandexDelivery = shipping.ShippingType == "YandexDelivery";
            }

            liSendBillingLink.Visible = aSendBillingLink.Visible =
                !order.Payed && order.OrderCustomer != null && shipping != null && order.PaymentMethod != null;

            if (BonusSystem.IsActive)
            {
                var purchase = BonusSystemService.GetPurchase(order.BonusCardNumber, order.Number, order.OrderID);
                if (purchase != null)
                {
                    bonusCardBlock.Visible = true;
                    lblBonusCardNumber.Text = purchase.Card.CardNumber.ToString();
                    lblBonusCardAmount.Text = purchase.NewBonusAmount.ToString();
                }
            }

            if (Settings1C.Enabled)
            {
                divUseIn1C.Visible = true;
                chkUseIn1C.Checked = order.UseIn1C;
                chkUseIn1C.Attributes["data-orderid"] = order.OrderID.ToString();

                var status1C = OrderStatus1CService.GetStatus1C(order.OrderID);
                if (status1C != null)
                {
                    divStatus1C.Visible = true;
                    lbl1CStatus.Text = status1C.Status1C;
                }
            }
            else
            {
                divUseIn1C.Visible = false;
            }

            if (SettingsCheckout.ManagerConfirmed)
            {
                divManagerConfirm.Visible = true;
                ckbManagerConfirm.Checked = order.ManagerConfirmed;
                ckbManagerConfirm.Attributes["data-orderid"] = order.OrderID.ToString();
            }


			lvOrderStatusHistory.DataSource = OrderStatusService.GetOrderStatusHistory(order.OrderID).OrderByDescending(item => item.Date);
            lvOrderStatusHistory.DataBind();

			var history = OrderHistoryService.GetList(order.OrderID);
            lvOrderHistory.DataSource = history;
            lvOrderHistory.DataBind();

            LoadTotal(order);
        }

        private void LoadTotal(Order order)
        {
            lblShippingPrice.Text = string.Format("+{0}", PriceFormatService.FormatPrice(order.ShippingCost, order.OrderCurrency));

            lvTaxes.DataSource = order.Taxes;
            lvTaxes.DataBind();

            float prodTotal = 0;
            if (order.OrderCertificates != null && order.OrderCertificates.Count > 0)
            {
                prodTotal = order.OrderCertificates.Sum(item => item.Sum);
            }
            else
            {
                prodTotal = order.OrderItems.Sum(oi => oi.Price * oi.Amount);
            }

            float totalDiscount = 0;

            totalDiscount += order.GetOrderDiscountPrice(); //order.OrderDiscount > 0 ? Convert.ToSingle(Math.Round(prodTotal / 100 * order.OrderDiscount, 2)) : 0;

            lblTotalOrderPrice.Text = PriceFormatService.FormatPrice(prodTotal, order.OrderCurrency);

            lblOrderDiscount.Text = string.Format("-{0}", PriceFormatService.FormatPrice(totalDiscount, order.OrderCurrency));
            lblOrderDiscountPercent.Text = order.OrderDiscount + @"%";
            trDiscount.Visible = order.OrderDiscount != 0 || order.OrderDiscountValue != 0;

            lblOrderBonuses.Text = string.Format("-{0}", PriceFormatService.FormatPrice(order.BonusCost, order.OrderCurrency));
            trBonuses.Visible = order.BonusCost != 0;

            liPaymentPrice.Visible = order.PaymentCost != 0;
            lblPaymentPrice.Text = (order.PaymentCost > 0 ? "+" : "") + PriceFormatService.FormatPrice(order.PaymentCost, order.OrderCurrency);

            if (order.Certificate != null)
            {
                trCertificatePrice.Visible = order.Certificate.Price != 0;
                lblCertificatePrice.Text = string.Format("-{0}", PriceFormatService.FormatPrice(order.Certificate.Price, order.OrderCurrency));
                totalDiscount += order.Certificate.Price;
            }

            if (order != null && order.Coupon != null)
            {
                float couponValue;
                trCoupon.Visible = order.Coupon.Value != 0;
                switch (order.Coupon.Type)
                {
                    case CouponType.Fixed:
                        var productsPrice = order.OrderItems.Where(p => p.IsCouponApplied).Sum(p => p.Price * p.Amount);
                        couponValue = productsPrice >= order.Coupon.Value ? order.Coupon.Value : productsPrice;
                        totalDiscount += couponValue;
                        lblCoupon.Text = String.Format("-{0} ({1})", PriceFormatService.FormatPrice(couponValue.RoundPrice(order.OrderCurrency.CurrencyValue, order.OrderCurrency), order.OrderCurrency), order.Coupon.Code);
                        break;
                    case CouponType.Percent:
                        couponValue = order.OrderItems.Where(p => p.IsCouponApplied).Sum(p => order.Coupon.Value * p.Price / 100 * p.Amount);
                        totalDiscount += couponValue;

                        lblCoupon.Text = String.Format("-{0} ({1}%) ({2})", PriceFormatService.FormatPrice(couponValue.RoundPrice(order.OrderCurrency.CurrencyValue, order.OrderCurrency), order.OrderCurrency),
                                                       PriceFormatService.FormatPriceInvariant(order.Coupon.Value),
                                                       order.Coupon.Code);
                        break;
                }
            }
            totalDiscount = totalDiscount.RoundPrice(order.OrderCurrency.CurrencyValue, order.OrderCurrency);

            float sum = prodTotal - totalDiscount - order.BonusCost + order.ShippingCost + order.Taxes.Where(tax => !tax.ShowInPrice).Sum(tax => tax.Sum) + order.PaymentCost;
            lblTotalPrice.Text = PriceFormatService.FormatPrice(sum < 0 ? 0 : sum, order.OrderCurrency);
        }

        protected string RenderSelectedOptions(IList<EvaluatedCustomOptions> evlist)
        {
            if (evlist == null || evlist.Count == 0)
                return "";

            var html = new StringBuilder();
            html.Append("<ul>");

            foreach (EvaluatedCustomOptions ev in evlist)
            {
                html.Append(string.Format("<li>{0}: {1} - {2}</li>",
                    ev.CustomOptionTitle, ev.OptionTitle,
                    ev.OptionPriceType == OptionPriceType.Fixed
                        ? PriceFormatService.FormatPrice(ev.OptionPriceBc, order.OrderCurrency)
                        : ev.OptionPriceBc + "%"));
            }

            html.Append("</ul>");
            return html.ToString();
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            if (int.TryParse(Request["orderid"], out OrderId))
            {
                OrderService.DeleteOrder(OrderId);
                Response.Redirect("ordersearch.aspx");
            }
        }

        protected string RenderPicture(int productId, int? photoId)
        {
            if (photoId != null)
            {
                var photo = PhotoService.GetPhoto((int)photoId);
                if (photo != null)
                {
                    return string.Format("<img src='{0}'/>", FoldersHelper.GetImageProductPath(ProductImageType.XSmall, photo.PhotoName, true));
                }
            }

            var p = ProductService.GetProduct(productId);
            if (p != null && p.Photo.IsNotEmpty())
            {
                return String.Format("<img src='{0}'/>", FoldersHelper.GetImageProductPath(ProductImageType.XSmall, p.Photo, true));
            }

            return string.Format("<img src='{0}' alt=\"\"/>", AdvantShop.Core.UrlRewriter.UrlService.GetAbsoluteLink("images/nophoto_xsmall.jpg"));
        }

        protected string RenderPaidButtons()
        {
            return
                string.Format(
                    "<label><input type=\"radio\" {0} name=\"g-checkout\" value=\"1\" onclick=\"setOrderPaid(1,{2})\"/>" + Resource.Admin_ViewOrder_Paid + "</label>" +
                    "<label><input type=\"radio\" {1} name=\"g-checkout\" value=\"0\" onclick=\"setOrderPaid(0,{2})\"/>" + Resource.Admin_ViewOrder_NotPaid + "</label>",
                    IsPaid ? "checked=\"checked\"" : string.Empty,
                    !IsPaid ? "checked=\"checked\"" : string.Empty,
                    OrderId);
        }

        protected string RenderDate(DateTime date)
        {
            ShowGroup = false;

            if (PrevDate == date)
                return "";

            PrevDate = date;
            ShowGroup = true;

            return AdvantShop.Localization.Culture.ConvertDate(date);
        }
    }
}