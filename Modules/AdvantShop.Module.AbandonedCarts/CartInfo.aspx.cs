using System;
using System.Text;
using System.Web.UI;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Module.AbandonedCarts.Domain;
using AdvantShop.Orders;


namespace Advantshop.Modules.UserControls
{
    public partial class CartInfo : Page
    {
        protected CheckoutAddress ShippingContact;
        protected Customer customer;
        protected ShoppingCart shoppingCart;
        protected ShoppingCartType shoppingCartType;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request["MasterPageEmpty"] == "true")
            {
                MasterPageFile = "~/Admin/MasterPageEmpty.master";
            }
            else
            {
                MasterPageFile = "~/Admin/MasterPageAdmin.master";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var customerId = Request["Id"].TryParseGuid();

            shoppingCartType = Request["ShoppingCartType"].TryParseEnum<ShoppingCartType>();
            if (shoppingCartType == 0)
                shoppingCartType = ShoppingCartType.ShoppingCart;

            customer = CustomerService.GetCustomer(customerId);
            var confirmCart = AbandonedCartsService.GetAbondonedCart(customerId);

            Title = "Временная корзина";

            if (confirmCart != null && confirmCart.CheckoutData != null &&
                confirmCart.CheckoutData.User != null)
            {
                customer = AbandonedCartsService.GetCustomer(confirmCart.CheckoutData.User);
                ShippingContact = confirmCart.CheckoutData.Contact;

                if (confirmCart.CheckoutData.SelectShipping != null)
                {
                    lblShippingMethodName.Text = confirmCart.CheckoutData.SelectShipping.Name;
                }

                if (confirmCart.CheckoutData.SelectPayment != null)
                {
                    lblPaymentMethodName.Text = confirmCart.CheckoutData.SelectPayment.Name;
                }
            }
            else if (customer != null)
            {
                divShipPayments.Visible = false;
                customerInfoNotExist.Visible = true;
            }
            else
            {
                customerInfo.Visible = false;
                divShipPayments.Visible = false;
                customerInfoNotExist.Visible = true;
            }


            if (customer != null)
            {
                if (customer.RegistredUser && customer.Id != Guid.Empty)
                {
                    lnkCustomerName.Text = customer.FirstName + " " + customer.LastName + " " + customer.Patronymic;
                    lnkCustomerName.NavigateUrl = Request["MasterPageEmpty"] == "true" ? "../adminv2/customers/edit/" + customer.Id : @"viewcustomer.aspx?customerid=" + customer.Id;
                    lnkCustomerEmail.Text = customer.EMail;
                    lnkCustomerEmail.NavigateUrl = "mailto:" + customer.EMail;

                    lblCustomerName.Visible = false;
                    lblCustomerEmail.Visible = false;
                }
                else
                {
                    lblCustomerName.Text = customer.FirstName + " " + customer.LastName + " " + customer.Patronymic;
                    lblCustomerEmail.Text = customer.EMail;

                    lnkCustomerName.Visible = false;
                    lnkCustomerEmail.Visible = false;
                }
                lblCustomerPhone.Text = customer.Phone;
            }

            if (ShippingContact != null)
            {
                lblShippingCountry.Text = ShippingContact.Country;
                lblShippingCity.Text = ShippingContact.City;
                lblShippingRegion.Text = ShippingContact.Region;
                lblShippingZipCode.Text = ShippingContact.Zip;
                lblShippingAddress.Text = ShippingContact.Street;
            }

            shoppingCart = ShoppingCartService.GetShoppingCart(shoppingCartType, customerId);
            if (shoppingCart != null)
            {
                lvOrderItems.DataSource = shoppingCart;
                lvOrderItems.DataBind();
            }

            lbCreateLead.Visible = customer != null && shoppingCart != null && shoppingCart.HasItems && 
                (customer.EMail.IsNotEmpty() || customer.FirstName.IsNotEmpty() || customer.Phone.IsNotEmpty());
        }

        protected string RenderPicture(int productId, int? photoId)
        {
            if (photoId != null)
            {
                var photo = PhotoService.GetPhoto((int) photoId);
                if (photo != null)
                    return string.Format("<img src='{0}'/>",
                                         FoldersHelper.GetImageProductPath(ProductImageType.Small, photo.PhotoName, true));
            }

            var p = ProductService.GetProduct(productId);
            if (p != null && p.Photo.IsNotEmpty())
            {
                return String.Format("<img src='{0}'/>",
                                     FoldersHelper.GetImageProductPath(ProductImageType.XSmall, p.Photo, true));
            }

            return string.Format("<img src='{0}' alt=\"\"/>", UrlService.GetAbsoluteLink("images/nophoto_xsmall.jpg"));
        }

        protected string RenderSelectedOptions(string options)
        {
            if (string.IsNullOrWhiteSpace(options))
                return string.Empty;

            var html = new StringBuilder();

            //foreach (EvaluatedCustomOptions ev in evlist)
            //{
            //    html.Append(string.Format("<div>{0}: {1}</div>", ev.CustomOptionTitle, ev.OptionTitle));
            //}

            return html.ToString();
        }
    }
}