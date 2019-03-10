using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules;
using AdvantShop.Module.ProductSets.Domain;
using System.Web.UI.WebControls;
using Color = System.Drawing.Color;

namespace AdvantShop.Module.ProductSets
{
    public partial class Admin_AdminProductSets : ProductAdminTabContent
    {
        protected int ProductId
        {
            get
            {
                int id;
                int.TryParse(Request["productid"], out id);
                return id;
            }
        }

        private void Msg(string text, bool isSuccess = true)
        {
            lblMessage.Text = text;
            lblMessage.ForeColor = isSuccess ? Color.Blue : Color.Red;
            lblMessage.Visible = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            lblHead.Text = ProductSetsSettings.Title;
        }
        
        protected void Page_PreRender(object sender, EventArgs e)
        {
            rProductsInSet.DataSource = ProductSetsService.GetLinkedOffers(ProductId);
            rProductsInSet.DataBind();
        }

        protected void btnAddOfferByArtNo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOfferArtNo.Text))
            {
                Msg((string)GetLocalResourceObject("EnterProductArtNo"), false);
                return;
            }

            var offer = OfferService.GetOffer(txtOfferArtNo.Text);
            if (offer != null)
            {
                ProductSetsService.AddProductLink(ProductId, offer.OfferId);
            }
            else
            {
                Msg(string.Format((string)GetLocalResourceObject("NotFoundProductByArtNo"), txtOfferArtNo.Text), false);
            }
            txtOfferArtNo.Text = string.Empty;
        }

        protected void rProductsInSet_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteOfferFromSet")
            {
                ProductSetsService.DeleteProductLink(ProductId, ModulesRepository.ConvertTo<int>(e.CommandArgument));
            }
        }

        protected string CheckOffer(Offer offer)
        {
            var errors = new List<string>();
            if (!offer.Product.Enabled)
                errors.Add((string)GetLocalResourceObject("CheckOffer_NotActive"));
            if (!offer.Product.CategoryEnabled)
                errors.Add((string)GetLocalResourceObject("CheckOffer_NotActiveCategory"));
            if (offer.Amount <= 0)
                errors.Add((string)GetLocalResourceObject("CheckOffer_NotAvailable"));
            if (offer.RoundedPrice <= 0)
                errors.Add((string)GetLocalResourceObject("CheckOffer_NoPrice"));
            if (CustomOptionsService.DoesProductHaveRequiredCustomOptions(offer.ProductId))
                errors.Add((string)GetLocalResourceObject("CheckOffer_HasRequiredCustomOptions"));
            return errors.Any()
                ? string.Format("<br/><span style=\"color: #ee0000; padding-left: 12px;\">{0}: {1}</span>",
                    GetLocalResourceObject("CheckOffer_Summary"),
                    errors.Aggregate((current, next) => current + ", " + next)) 
                : string.Empty;
        }
    }
}