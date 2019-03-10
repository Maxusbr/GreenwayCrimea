using System;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Helpers;
using AdvantShop.News;

namespace Admin.UserControls.News
{
    public partial class NewsProducts : System.Web.UI.UserControl
    {
        public int NewsId { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtError.Visible = false;
            if (!IsPostBack)
            {
                popTree.UpdateTree(NewsService.GetAllNewsProducts(NewsId).Select(rp => rp.ProductId));
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            rNewsProducts.DataSource = NewsService.GetAllNewsProducts(NewsId);
            rNewsProducts.DataBind();
        }

        protected void popTree_Selected(object sender, PopupTreeView.TreeNodeSelectedArgs args)
        {
            foreach (var altId in args.SelectedValues)
            {
                NewsService.AddNewsProduct(NewsId, SQLDataHelper.GetInt(altId));
            }
            popTree.UpdateTree(NewsService.GetAllNewsProducts(NewsId).Select(rp => rp.ProductId));
        }

        protected void lbAddRelatedProduct_Click(object sender, EventArgs e)
        {
            popTree.Show();
        }

        protected void lbAddProductByArtNo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductArtNo.Text))
            {
                txtError.Text = Resources.Resource.Admin_Product_EnterProductArtNo;
                txtError.Visible = true;
                return;
            }

            var productId = ProductService.GetProductId(txtProductArtNo.Text);
            if (productId != 0)
            {
                NewsService.AddNewsProduct(NewsId, productId);
            }
            else
            {
                txtError.Text = Resources.Resource.Admin_Product_NotFoundProductByArtNo + txtProductArtNo.Text;
                txtError.Visible = true;
            }
            txtProductArtNo.Text = string.Empty;
        }

        protected void rNewsProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteNewsProduct")
            {
                NewsService.DeleteNewsProduct(NewsId, SQLDataHelper.GetInt(e.CommandArgument));
            }
            popTree.UpdateTree(NewsService.GetAllNewsProducts(NewsId).Select(rp => rp.ProductId));
        }
    }
}