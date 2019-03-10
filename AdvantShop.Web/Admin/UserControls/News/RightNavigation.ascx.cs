using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.News;

namespace Admin.UserControls.News
{
    public partial class RightNavigation : UserControl
    {
        protected SqlPaging Paging;
        public int NewsCategoryId { get; set; }
        public int NewsId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Paging = (SqlPaging)(ViewState["Paging"]);
            }
            else
            {
                Paging = new SqlPaging
                    {
                        TableName = "Settings.News LEFT JOIN Catalog.Photo ON Photo.ObjId = News.NewsId AND Type = @PhotoType",
                        ItemsPerPage = 18
                    };

                Paging.AddFieldsRange(new[]
                    {
                        new Field { Name = "NewsId as ID", IsDistinct = true },
                        new Field { Name = "Title" },
                        new Field { Name = "PhotoName" },
                        new Field { Name = "NewsCategoryID", NotInQuery = true},
                        new Field { Name = "AddingDate", Sorting = SortDirection.Descending }
                    });

                ddlNewsCategory.DataBind();
                ddlNewsCategory.SelectedValue = NewsCategoryId.ToString();

                var pageIndex = Request["pn"].TryParseInt(1);

                Paging.CurrentPageIndex = pageIndex;
                Paging.AddParam(new SqlParam {ParameterName = "@PhotoType", Value = PhotoType.News.ToString()});
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            Paging.Fields["NewsCategoryID"].Filter = new EqualFieldFilter()
            {
                ParamName = "@NewsCategoryID",
                Value = ddlNewsCategory.SelectedValue
            };
            UpdateCategoryContentPanel();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            Paging.CurrentPageIndex = 1;
        }
        protected void ddlCurrentPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Paging.CurrentPageIndex = SQLDataHelper.GetInt(ddlCurrentPage.SelectedValue);
        }

        private void UpdateCategoryContentPanel()
        {
            rptNews.DataSource = Paging.PageItems;
            rptNews.DataBind();

            if (Paging.CurrentPageIndex > 1)
            {
                lbPreviousPage.Enabled = true;
            }
            var pageCount = Paging.PageCount;
            if (pageCount > 1 && Paging.CurrentPageIndex < pageCount)
            {
                lbNextPage.Enabled = true;
            }

            ddlCurrentPage.Items.Clear();

            for (int i = 1; i <= pageCount; i++)
            {
                var itm = new ListItem(i.ToString(), i.ToString());
                if (i == Paging.CurrentPageIndex)
                {
                    itm.Selected = true;
                }
                ddlCurrentPage.Items.Add(itm);
            }
            ViewState["Paging"] = Paging;

        }

        protected void lbNextPage_Click(object sender, EventArgs e)
        {
            Paging.CurrentPageIndex++;
        }

        protected void lbPreviousPage_Click(object sender, EventArgs e)
        {
            Paging.CurrentPageIndex--;
        }

        protected void rptNews_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteNews"))
            {
                NewsService.DeleteNews(SQLDataHelper.GetInt(e.CommandArgument));
                if (Request["newsid"] == e.CommandArgument.ToString())
                {
                    RedirectFromUpdatePanel("NewsAdmin.aspx");
                }
            }
        }

        public void RedirectFromUpdatePanel(string url)
        {
            string redirectUrl = Page.ResolveClientUrl(url);
            string script = "window.location='" + redirectUrl + "';";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "RedirectFromUpdatePanel", script, true);
        }

        protected void sds_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = AdvantShop.Connection.GetConnectionString();
        }
    }
}