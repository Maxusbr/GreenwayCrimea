using System;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Helpers;
using AdvantShop.Module.Blog.Domain;

namespace AdvantShop.Module.Blog
{
    public partial class BlogProducts : System.Web.UI.UserControl
    {
        public int BlogId { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtError.Visible = false;
            if (!IsPostBack)
            {
                popTree.UpdateTree(BlogService.GetProducts(BlogId).Select(rp => rp.ProductId));
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            rBlogProducts.DataSource = BlogService.GetProducts(BlogId);
            rBlogProducts.DataBind();
        }

        protected void popTree_Selected(object sender, PopupTreeView.TreeNodeSelectedArgs args)
        {
            foreach (var altId in args.SelectedValues)
            {
                BlogService.AddProduct(BlogId, SQLDataHelper.GetInt(altId));
            }
            popTree.UpdateTree(BlogService.GetProducts(BlogId).Select(rp => rp.ProductId));
        }

        protected void lbAddRelatedProduct_Click(object sender, EventArgs e)
        {
            popTree.Show();
        }

        protected void lbAddProductByArtNo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductArtNo.Text))
            {
                txtError.Text = (string)GetLocalResourceObject("Admin_Blogt_EnterProductArtNo");
                txtError.Visible = true;
                return;
            }

            var productId = ProductService.GetProductId(txtProductArtNo.Text);
            if (productId != 0)
            {
                BlogService.AddProduct(BlogId, productId);
            }
            else
            {
                txtError.Text = (string)GetLocalResourceObject("Admin_Blog_NotFoundProductByArtNo") + txtProductArtNo.Text;
                txtError.Visible = true;
            }
            txtProductArtNo.Text = string.Empty;
        }

        protected void rBlogProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteBlogProduct")
            {
                BlogService.DeleteProduct(BlogId, SQLDataHelper.GetInt(e.CommandArgument));
            }
            popTree.UpdateTree(BlogService.GetProducts(BlogId).Select(rp => rp.ProductId));
        }
    }
}