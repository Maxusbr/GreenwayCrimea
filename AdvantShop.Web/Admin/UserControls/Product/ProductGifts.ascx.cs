using System;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Helpers;

namespace Admin.UserControls.Products
{
    public partial class ProductGifts : System.Web.UI.UserControl
    {
        public int ProductID { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtError.Visible = false;
            if (!IsPostBack)
            {
                popTree.ExceptId = ProductID;
                popTree.UpdateTree(OfferService.GetProductGifts(ProductID).Select(o => o.OfferId));
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            rptProductGifts.DataSource = OfferService.GetProductGifts(ProductID);
            rptProductGifts.DataBind();
        }

        protected void popTree_Selected(object sender, PopupTreeView.TreeNodeSelectedArgs args)
        {
            foreach (var offerId in args.SelectedValues)
            {
                OfferService.AddProductGift(ProductID, SQLDataHelper.GetInt(offerId));
                ProductService.PreCalcProductParams(ProductID);
            }
            popTree.UpdateTree(OfferService.GetProductGifts(ProductID).Select(o => o.OfferId));
        }

        protected void lbAddProductGift_Click(object sender, EventArgs e)
        {
            popTree.Show();
        }

        protected void lbAddProductGiftByArtNo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOfferArtNo.Text))
            {
                txtError.Text = Resources.Resource.Admin_Product_EnterProductArtNo;
                txtError.Visible = true;
                return;
            }

            var offer = OfferService.GetOffer(txtOfferArtNo.Text);
            if (offer != null)
            {
                OfferService.AddProductGift(ProductID, offer.OfferId);
                ProductService.PreCalcProductParams(ProductID);
            }
            else
            {
                txtError.Text = Resources.Resource.Admin_Product_NotFoundProductByArtNo + txtOfferArtNo.Text;
                txtError.Visible = true;
            }
            txtOfferArtNo.Text = string.Empty;

        }

        protected void rptProductGifts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteGift")
            {
                OfferService.DeleteProductGift(ProductID, SQLDataHelper.GetInt(e.CommandArgument));
                ProductService.PreCalcProductParams(ProductID);
            }
            popTree.UpdateTree(OfferService.GetProductGifts(ProductID).Select(o => o.OfferId));
        }
    }
}