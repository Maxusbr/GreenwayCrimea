//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class m_CategorySortOrder : AdvantShopAdminPage
    {
        private SqlPaging _paging;

        private static void LoadChildCategories(TreeNode node)
        {
            foreach (Category c in CategoryService.GetChildCategoriesByCategoryId(SQLDataHelper.GetInt(node.Value), false))
            {
                var newNode = new TreeNode
                    {
                        Text = c.Name + @" (" + c.ProductsCount + @")",
                        Value = c.CategoryId.ToString()
                    };
                if (c.HasChild)
                {
                    newNode.Expanded = false;
                    newNode.PopulateOnDemand = true;
                }
                else
                {
                    newNode.Expanded = true;
                    newNode.PopulateOnDemand = false;
                }
                node.ChildNodes.Add(newNode);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_m_CategorySortOrder_Title));
        
            divSave.Visible = false;
            grid.ResetToDefaultValueOnRowEditCancel = false;

            if (!IsPostBack)
            {
                var root = new TreeNode { Text = Resource.Admin_m_CategorySortOrder_Root, Value = @"0", Selected = true };
                tree.Nodes.Add(root);
                LoadChildCategories(tree.Nodes[0]);
            }

            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "Catalog.Category" };

                var f = new Field { Name = "CategoryID", IsDistinct = true };
                var ifilter = new NotEqualFieldFilter { Value = "0", ParamName = "@id" };
                f.Filter = ifilter;
                _paging.AddField(f);


                f = new Field { Name = "Name" };
                _paging.AddField(f);

                f = new Field { Name = "SortOrder", Sorting = SortDirection.Ascending };
                _paging.AddField(f);

                f = new Field { Name = "ParentCategory" };
                var pfilter = new EqualFieldFilter { Value = hfParent.Value, ParamName = "@CategoryID" };
                f.Filter = pfilter;
                _paging.AddField(f);
                grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");

                _paging.ItemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue);

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);

                _paging.ItemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue);


                if (_paging == null)
                {
                    throw (new Exception("Paging lost"));
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            CommonHelper.RegCloseScript(this, string.Empty);
        }

        protected void tree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            LoadChildCategories(e.Node);
        }

        protected void upParent_PreRender(object sender, EventArgs e)
        {
            lParent.Text = tree.SelectedNode.Text;
        }

        protected void btnOkParent_Click(object sender, EventArgs e)
        {
            hfParent.Value = tree.SelectedValue;
            lParent.Text = tree.SelectedNode.Text;
            mpeTree.Hide();

            var pfilter = new EqualFieldFilter { Value = hfParent.Value, ParamName = "@CategoryID" };
            //var pfilter = new InChildCategoriesFieldFilter { CategoryId = hfParent.Value, ParamName = "@CategoryID" };
            _paging.Fields["ParentCategory"].Filter = pfilter;
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {
                CategoryService.UpdateCategorySortOrder(SQLDataHelper.GetString(grid.UpdatedRow["Name"]),
                                                        SQLDataHelper.GetInt(grid.UpdatedRow["SortOrder"]),
                                                        SQLDataHelper.GetInt(grid.UpdatedRow["CategoryID"]));
            }

            grid.DataSource = _paging.PageItems;
            grid.DataBind();
            pageNumberer.PageCount = _paging.PageCount;
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"CategoryID", "arrowID"},
                    {"Name", "arrowName"},
                    {"SortOrder", "arrowSortOrder"}
                };

            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";


            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[csf.Name], (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
            }
            else
            {
                csf.Sorting = null;
                grid.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

                nsf.Sorting = SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
        }

        protected void SaveAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= grid.Rows.Count - 1; i++)
            {
                grid.UpdateRow(grid.Rows[i].RowIndex, false);
                if (grid.UpdatedRow != null)
                {
                    try
                    {
                        SQLDataAccess.ExecuteNonQuery("Update Catalog.Category set name=@name, SortOrder=@SortOrder where CategoryID = @CategoryID",
                                                      CommandType.Text,
                                                      new SqlParameter("@name", grid.UpdatedRow["Name"]),
                                                      new SqlParameter("@SortOrder", grid.UpdatedRow["SortOrder"]),
                                                      new SqlParameter("@CategoryID", grid.UpdatedRow["CategoryID"])
                            );
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }
            }
            divSave.Visible = true;
        }

        protected void linkGO_Click(object sender, EventArgs e)
        {
            int pagen;
            try
            {
                pagen = int.Parse(txtPageNum.Text);
            }
            catch (Exception)
            {
                pagen = -1;
            }
            if (pagen >= 1 && pagen <= _paging.PageCount)
            {
                pageNumberer.CurrentPageIndex = pagen;
                _paging.CurrentPageIndex = pagen;
            }
        }


        protected void ddRowsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageNumberer.CurrentPageIndex = 1;
        }
    }
}