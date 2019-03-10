using System;
using System.Linq;
using System.Collections;
using AdvantShop.Shipping;
using System.Text;
using System.Web.UI;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Module.BuyMore.Domain;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Advantshop.Modules.UserControls.BuyMore
{
    public partial class BuyMoreModuleAddEdit : Page
    {
        protected int Id;
        private BuyMoreProductModel _action;
        private BuyMoreProductModel _oldAction;


        protected void Page_Load(object sender, EventArgs e)
        {
            Id = Request["Id"].TryParseInt();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                LoadOffers();
                return;
            }

            if (Id != 0)
            {
                LoadAction();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Id != 0)
                SaveAction();
            else
                CreateAction();

            if (!lblMessage.Visible)
            {
                var jScript = new StringBuilder();
                jScript.Append("<script type=\'text/javascript\' language=\'javascript\'> ");
                jScript.Append("window.opener.location.reload(true); ");
                jScript.Append("self.close();");
                jScript.Append("</script>");
                Type csType = this.GetType();
                ClientScriptManager clScriptMng = this.ClientScript;
                clScriptMng.RegisterClientScriptBlock(csType, "Close_window", jScript.ToString());
            }
        }

        protected void lvProducts_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (string.Equals(e.CommandName, "DeleteOffer") && ViewState["offersIds"] != null)
            {
                ViewState["offersIds"] = ((List<int>)ViewState["offersIds"]).Where(item => item != Convert.ToInt32(e.CommandArgument)).ToList<int>();
                LoadOffers();
            }
        }

        protected void btnAddOffer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProductArtNo.Text))
                return;

            var offer = OfferService.GetOffer(txtProductArtNo.Text);
            if (offer == null)
            {
                //message
                return;
            }

            var list = (List<int>)ViewState["offersIds"];
            if (list != null && !list.Any(item => item == offer.OfferId))
            {
                list.Add(offer.OfferId);
                ViewState["offersIds"] = list;
            }
            else if(list== null)
            {
                ViewState["offersIds"] = new List<int>() { offer.OfferId };
            }

            txtProductArtNo.Text = string.Empty;

            LoadOffers();
        }

        #region Private methods

        private void LoadAction()
        {
            _action = BuyMoreService.Get(Id);

            if (_action != null)
            {
                txtOrderPriceFrom.Text = _action.OrderPriceFrom.ToString();
                cbFreeShipping.Checked = _action.FreeShipping;

                ViewState["offersIds"] = _action.GiftOffersIdsList;
                LoadOffers();
            }
        }

        private void LoadOffers()
        {
            if (ViewState["offersIds"] != null)
            {
                var offers = new List<Offer>();
                foreach (var offerId in (List<int>)ViewState["offersIds"])
                {
                    var offer = OfferService.GetOffer(offerId);
                    if (offer != null)
                    {
                        offers.Add(offer);
                    }
                }

                lvProducts.DataSource = offers;
                lvProducts.DataBind();

                return;
            }
        }


        private void SaveAction()
        {
            if (!IsValidData())
                return;

            _action = BuyMoreService.Get(Id);
            _oldAction = BuyMoreService.Get(Id);

            var giftOffers = string.Empty;
            foreach (var offerArtNo in (List<int>)ViewState["offersIds"])
            {
                var offer = OfferService.GetOffer(offerArtNo);
                if (offer != null)
                {
                    giftOffers += offer.OfferId + ";";
                }
            }

            _action.OrderPriceFrom = txtOrderPriceFrom.Text.TryParseFloat();
            _action.FreeShipping = cbFreeShipping.Checked;
            _action.GiftOffersIds = giftOffers;

            BuyMoreService.Update(_action);

            if (_oldAction.GiftOffersIds != _action.GiftOffersIds && !string.IsNullOrEmpty(_oldAction.GiftOffersIds))
            {
                BuyMoreService.DeleteGiftFromShoppingCart(string.Join(",", _oldAction.GiftOffersIdsList));
            }
        }

        private void CreateAction()
        {
            if (!IsValidData())
                return;

            var giftOffers = string.Empty;

            if (ViewState["offersIds"] != null)
            {
                foreach (var offerArtNo in (List<int>) ViewState["offersIds"])
                {
                    var offer = OfferService.GetOffer(offerArtNo);
                    if (offer != null)
                    {
                        giftOffers += offer.OfferId + ";";
                    }
                }
            }

            var action = new BuyMoreProductModel
            {
                OrderPriceFrom = txtOrderPriceFrom.Text.TryParseFloat(),
                GiftOffersIds = giftOffers,
                FreeShipping = cbFreeShipping.Checked
            };

            BuyMoreService.Add(action);
        }

        private bool IsValidData()
        {
            bool valid = true;

            lblMessage.Visible = false;
            float price;

            if (txtOrderPriceFrom.Text.IsNullOrEmpty() || !float.TryParse(txtOrderPriceFrom.Text, out price))
            {
                MsgErr("Неверное число.");
                valid = false;
            }

            foreach (var offerArtNo in txtProductArtNo.Text.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (offerArtNo.IsNotEmpty())
                {
                    var offer = OfferService.GetOffer(offerArtNo);
                    if (offer == null)
                    {
                        MsgErr("Артикул " + offerArtNo + " оффера не найден.");
                        valid = false;
                    }
                }
            }

            if (txtOrderPriceFrom.Text.IsNullOrEmpty() && !cbFreeShipping.Checked)
            {
                MsgErr("Предложение не может быть пустым - должна быть бесплатная доставка или подарок.");
                valid = false;
            }

            return valid;
        }

        private void MsgErr(string msg)
        {
            lblMessage.Visible = true;
            lblMessage.Text = msg;
        }

        #endregion
    }
}