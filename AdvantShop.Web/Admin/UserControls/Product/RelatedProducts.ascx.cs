using System;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Helpers;

namespace Admin.UserControls.Products
{
    public partial class RelatedProducts : System.Web.UI.UserControl
    {
        public int ProductID { set; get; }
        public int RelatedType { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtError.Visible = false;
            if (!IsPostBack)
            {
                popTree.ExceptId = ProductID;
                popTree.UpdateTree(ProductService.GetRelatedProducts(ProductID, (RelatedType)RelatedType).Select(rp => rp.ProductId));
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            rRelatedProducts.DataSource = ProductService.GetRelatedProducts(ProductID, (RelatedType)RelatedType);
            rRelatedProducts.DataBind();
        }

        protected void popTree_Selected(object sender, PopupTreeView.TreeNodeSelectedArgs args)
        {
            foreach (var altId in args.SelectedValues)
            {
                ProductService.AddRelatedProduct(ProductID, SQLDataHelper.GetInt(altId), (RelatedType)RelatedType);
            }
            popTree.UpdateTree(ProductService.GetRelatedProducts(ProductID, (RelatedType)RelatedType).Select(rp => rp.ProductId));
        }

        protected void lbAddRelatedProduct_Click(object sender, EventArgs e)
        {
            popTree.Show();
        }

        protected void lbAddRelatedProductByArtNo_Click(object sender, EventArgs e)
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
                ProductService.AddRelatedProduct(ProductID, productId, (RelatedType)RelatedType);
            }
            else
            {
                txtError.Text = Resources.Resource.Admin_Product_NotFoundProductByArtNo + txtProductArtNo.Text;
                txtError.Visible = true;
            }
            txtProductArtNo.Text = string.Empty;

        }

        protected void rRelatedProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRelatedProduct")
            {
                ProductService.DeleteRelatedProduct(ProductID, SQLDataHelper.GetInt(e.CommandArgument), (RelatedType)RelatedType);
            }
            popTree.UpdateTree(ProductService.GetRelatedProducts(ProductID, (RelatedType)RelatedType).Select(rp => rp.ProductId));
        }
    }
}