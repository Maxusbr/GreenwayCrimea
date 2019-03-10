//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Resources;
using AdvantShop.Core.SQL;

namespace Admin
{
    public partial class StaticPages : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;
        private int _parentPageId = 0;
        private bool _needReloadTree;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_StaticPage_lblSubMain));

            int.TryParse(Request["parentid"], out _parentPageId);
            if (!IsPostBack)
            {
                _paging = new SqlPaging
                    {
                        TableName = "[CMS].[StaticPage]",
                        ItemsPerPage = 10
                    };

                _paging.AddFieldsRange(new List<Field>
                    {
                        new Field {Name = "StaticPageID as ID",IsDistinct = true},
                        new Field {Name = "PageName"},
                        new Field {Name = "Enabled"},
                        new Field {Name = "ParentID"},
                        new Field {Name = "SortOrder",Sorting = SortDirection.Ascending},
                        new Field {Name = "ModifyDate"}
                    });


                if (_parentPageId != 0)
                {
                    var ef = new EqualFieldFilter()
                        {
                            ParamName = "@ParentID",
                            Value = _parentPageId.ToString(CultureInfo.InvariantCulture)
                        };

                    _paging.Fields["ParentID"].Filter = ef;
                }
                else
                {
                    var nf = new NullFieldFilter()
                        {
                            ParamName = "@ParentID",
                            Null = true
                        };

                    _paging.Fields["ParentID"].Filter = nf;
                }



                grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");

                _paging.ItemsPerPage = 10;

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

                string strIds = Request.Form["SelectedIds"];

                if (!string.IsNullOrEmpty(strIds))
                {
                    strIds = strIds.Trim();
                    var arrids = strIds.Split(' ');

                    _selectionFilter = new InSetFieldFilter();
                    if (arrids.Contains("-1"))
                    {
                        _selectionFilter.IncludeValues = false;
                        _inverseSelection = true;
                    }
                    else
                    {
                        _selectionFilter.IncludeValues = true;
                    }
                    _selectionFilter.Values = arrids.Where(id => id != "-1").ToArray();
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            //-----Selection filter
            if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "0") != 0)
            {
                if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "2") == 0)
                {
                    if (_selectionFilter != null)
                    {
                        _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                    }
                    else
                    {
                        _selectionFilter = null;

                    }
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            //----Name filter
            _paging.Fields["PageName"].Filter = !string.IsNullOrEmpty(txtPageName.Text) ? new CompareFieldFilter { Expression = txtPageName.Text, ParamName = "@PageName" } : null;
            _paging.Fields["SortOrder"].Filter = !string.IsNullOrEmpty(txtSortOrder.Text) ? new CompareFieldFilter { Expression = txtSortOrder.Text, ParamName = "@SortOrder" } : null;
            _paging.Fields["Enabled"].Filter = (ddlEnabled.SelectedValue != "any")
                                                   ? new EqualFieldFilter { ParamName = "@Enabled", Value = ddlEnabled.SelectedValue } : null;

            pageNumberer.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            grid.ChangeHeaderImageUrl(null, null);
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
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

        protected void lbDeleteSelected_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        StaticPageService.DeleteStaticPage(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("StaticPageID as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        StaticPageService.DeleteStaticPage(id);
                    }
                }
            }
        }

        protected void lbSetActive_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        StaticPageService.SetStaticPageActivity(SQLDataHelper.GetInt(id), true);
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("StaticPageID as ID");
                    foreach (int id in itemsIds.Where(cId => !_selectionFilter.Values.Contains(cId.ToString(CultureInfo.InvariantCulture))))
                    {
                        StaticPageService.SetStaticPageActivity(SQLDataHelper.GetInt(id), true);
                    }
                }
            }
        }

        protected void lbSetDeactive_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        StaticPageService.SetStaticPageActivity(SQLDataHelper.GetInt(id), false);
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("StaticPageID as ID");
                    foreach (int id in itemsIds.Where(cId => !_selectionFilter.Values.Contains(cId.ToString())))
                    {
                        StaticPageService.SetStaticPageActivity(SQLDataHelper.GetInt(id), false);
                    }
                }
            }
        }

        protected void lbChangeParent_Click(object sender, EventArgs e)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        StaticPageService.ChangeParentPage(SQLDataHelper.GetInt(id), SQLDataHelper.GetInt(ddlParentPages.SelectedValue));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("StaticPageID as ID");
                    foreach (int id in itemsIds.Where(cId => !_selectionFilter.Values.Contains(cId.ToString())))
                    {
                        StaticPageService.ChangeParentPage(SQLDataHelper.GetInt(id), SQLDataHelper.GetInt(ddlParentPages.SelectedValue));
                    }
                }
            }
        }


        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeletePage")
            {
                StaticPageService.DeleteStaticPage(SQLDataHelper.GetInt(e.CommandArgument));
                _needReloadTree = true;
                //Response.Redirect("StaticPages.aspx",true );
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"ID", "arrowStaticPageID"},
                    {"PageName", "arrowPageName"},
                    {"Enabled", "arrowEnabled"},
                    {"SortOrder", "arrowSortOrder"},
                    {"ModifyDate", "arrowModifyDate"}
                };

            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";


            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                grid.ChangeHeaderImageUrl(arrows[csf.Name],
                                          (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (grid.UpdatedRow != null)
                {
                    int sortOrder;
                    if (int.TryParse(grid.UpdatedRow["SortOrder"], out sortOrder))
                    {
                        var page = StaticPageService.GetStaticPage(SQLDataHelper.GetInt(grid.UpdatedRow["ID"]));
                        page.PageName = grid.UpdatedRow["PageName"];
                        page.Enabled = SQLDataHelper.GetBoolean(grid.UpdatedRow["Enabled"]);
                        page.SortOrder = sortOrder;
                        StaticPageService.UpdateStaticPage(page);
                    }
                }

                DataTable data = _paging.PageItems;
                while (data.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
                {
                    _paging.CurrentPageIndex--;
                    data = _paging.PageItems;
                }

                data.Columns.Add(new DataColumn("IsSelected", typeof (bool)) {DefaultValue = _inverseSelection});
                if ((_selectionFilter != null) && (_selectionFilter.Values != null))
                {
                    for (int i = 0; i <= data.Rows.Count - 1; i++)
                    {
                        int intIndex = i;
                        if (Array.Exists(_selectionFilter.Values, c => c == (data.Rows[intIndex]["ID"]).ToString()))
                        {
                            data.Rows[i]["IsSelected"] = !_inverseSelection;
                        }
                    }
                }

                if (data.Rows.Count < 1)
                {
                    goToPage.Visible = false;
                }

                grid.DataSource = data;
                grid.DataBind();

                pageNumberer.PageCount = _paging.PageCount;
                lblFound.Text = _paging.TotalRowsCount.ToString(CultureInfo.InvariantCulture);


                if (!IsPostBack || _needReloadTree)
                {
                    ReloadTree();
                    UpdatePanelTree.Update();
                }
                _needReloadTree = false;
            }
            catch(Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        protected void ReloadTree()
        {
            try
            {
                tree.Nodes.Clear();

                tree.Nodes.Add(new ButtonTreeNodeStaticPage
                    {
                        NavigateUrl = "StaticPages.aspx",
                        Value = "0",
                        Text = Resource.Admin_StaticPage_Root,
                        TreeView = tree,
                        PopulateOnDemand = false,
                        Selected = _parentPageId == 0,
                        ShowButtons = TreeButtonStatus.None
                    });

                foreach (StaticPage page in StaticPageService.GetRootStaticPages())
                {
                    tree.Nodes.Add(new ButtonTreeNodeStaticPage
                        {
                            NavigateUrl = page.HasChildren ? "StaticPages.aspx?ParentID=" + page.StaticPageId : "StaticPage.aspx?PageID=" + page.StaticPageId,
                            Value = page.StaticPageId.ToString(CultureInfo.InvariantCulture),
                            Text = page.PageName,
                            PopulateOnDemand = page.HasChildren,
                            Expanded = !page.HasChildren,
                            Selected = page.StaticPageId == _parentPageId,
                            TreeView = tree

                        });
                }

                var parentPageIDs = StaticPageService.GetParentStaticPages(_parentPageId);
                var nodes = tree.Nodes;

                for (var i = parentPageIDs.Count - 1; i >= 0; i--)
                {
                    var tn = (from TreeNode n in nodes where n.Value == parentPageIDs[i].ToString(CultureInfo.InvariantCulture) select n).SingleOrDefault();
                    if (i == 0)
                    {
                        tn.Select();
                        tn.Expand();
                    }
                    else
                    {
                        tn.Expand();
                    }

                    nodes = tn.ChildNodes;
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        protected void btnAddPage_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request["parentid"]))
                Response.Redirect("StaticPage.aspx?ParentID=" + Request["parentid"]);
            else
                Response.Redirect("StaticPage.aspx");
        }

        protected string RenderSplitter()
        {
            var str = new StringBuilder();
            str.Append("<td class=\'splitter\'  onclick=\'togglePanel();return false;\' >");
            str.Append("<div class=\'leftPanelTop\'></div>");
            switch (Resource.Admin_Catalog_SplitterLang)
            {
                case "rus":
                    str.Append("<div id=\'divHide\' class=\'left_hide_rus\'></div>");
                    str.Append("<div id=\'divShow\' class=\'left_show_rus\'></div>");
                    break;
                case "eng":
                    str.Append("<div id=\'divHide\' class=\'left_hide_en\'></div>");
                    str.Append("<div id=\'divShow\' class=\'left_show_en\'></div>");
                    break;
            }
            str.Append("</td>");
            return str.ToString();
        }

        private void LoadChildStaticPages(TreeNode node)
        {
            foreach (var page in StaticPageService.GetChildStaticPages(SQLDataHelper.GetInt(node.Value), false))
            {
                var newNode = new ButtonTreeNodeStaticPage
                    {
                        Text = page.PageName,
                        MessageToDel =
                            Server.HtmlEncode(string.Format(
                                Resource.Admin_MasterPageAdminCatalog_Confirmation, page.PageName)),
                        Value = page.StaticPageId.ToString(CultureInfo.InvariantCulture),
                        TreeView = tree
                    };
                if (page.HasChildren)
                {
                    newNode.Expanded = false;
                    newNode.PopulateOnDemand = true;
                    newNode.NavigateUrl = "StaticPages.aspx?ParentID=" + page.StaticPageId;
                }
                else
                {
                    newNode.Expanded = true;
                    newNode.PopulateOnDemand = false;
                    newNode.NavigateUrl = "StaticPage.aspx?PageID=" + page.StaticPageId;
                }
                node.ChildNodes.Add(newNode);
            }
        }

        protected void ibtnAddInRoot_Click(object sender, EventArgs e)
        {
            if (_parentPageId != 0)
            {
                Response.Redirect("StaticPage.aspx?ParentID=" + Request["parentid"]);
            }
            else
            {
                Response.Redirect("StaticPage.aspx");
            }
        }

        protected void tree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            LoadChildStaticPages(e.Node);
        }

        protected void tree_TreeNodeCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName.StartsWith("DeleteStaticPage"))
            {
                int statpageId = -1;
                if (e.CommandName.Contains("#"))
                    statpageId = SQLDataHelper.GetInt(e.CommandName.Substring(e.CommandName.IndexOf("#") + 1));
                if (statpageId == -1) return;
                if (statpageId != 0)
                    StaticPageService.DeleteStaticPage(statpageId);
                Response.Redirect(Request.RawUrl);
            }
        }

        protected void ddlParentPagesOnDataBound(object sender, EventArgs e)
        {
            ddlParentPages.Items.Insert(0, new ListItem(Resources.Resource.Admin_StaticPage_Root, "0"));
        }

        protected void sds_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
        }

    }
}