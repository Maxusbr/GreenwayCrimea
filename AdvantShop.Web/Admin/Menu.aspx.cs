//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class Menu : AdvantShopAdminPage
    {
        private int MenuId
        {
            get { return Request["menuid"].TryParseInt(); }
        }
        protected EMenuType MenuType
        {
            get { return Request["type"].TryParseEnum<EMenuType>(); }
        }

        private InSetFieldFilter _selectionFilter;
        private bool _inverseSelection;
        private SqlPaging _paging;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, MenuType.Localize()));

            var menuitem = MenuService.GetMenuItemById(MenuId);
            pnlLinks.Visible = menuitem != null;
            lblHead.Text = menuitem != null
                ? string.Format("{0} - {1}", MenuType.Localize(), menuitem.MenuItemName)
                : MenuType.Localize();

            btnAdd.OnClientClick = "open_window('m_Menu.aspx?ParentId=" + MenuId + "&type=" + MenuType + "', 750, 700);return false;";
            hlEditMenuItem.NavigateUrl = "javascript:open_window(\'m_Menu.aspx?MenuID=" + MenuId + "\', 750, 700)";
            hlDeleteMenuItem.Attributes["data-confirm"] = string.Format(Resource.Admin_MasterPageAdminCatalog_MenuConfirmation, menuitem != null ? menuitem.MenuItemName : string.Empty);

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                    {
                        TableName = "[CMS].[Menu]",
                        ItemsPerPage = 10
                    };

                _paging.AddFieldsRange(new List<Field>
                    {
                        new Field {Name = "MenuItemID as ID", IsDistinct = true},
                        new Field {Name = "MenuItemParentID" },
                        new Field {Name = "MenuItemName"},
                        new Field {Name = "Enabled"},
                        new Field {Name = "Blank"},
                        new Field {Name = "SortOrder", Sorting = SortDirection.Ascending},
                        new Field {Name = "MenuType", Filter = new EqualFieldFilter {ParamName = "@MenuType", Value = ((int)MenuType).ToString()}}
                    });

                _paging.Fields["MenuItemParentID"].Filter = MenuId != 0
                    ? new EqualFieldFilter {ParamName = "@MenuItemParentID", Value = MenuId.ToString()}
                    : (FieldFilter)new NullFieldFilter {ParamName = "@MenuItemParentID", Null = true};

                grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");

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
                    string[] arrids = strIds.Split(' ');

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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {
                int sortOrder = 0;
                if (Int32.TryParse(grid.UpdatedRow["SortOrder"], out sortOrder))
                {
                    var mItem = MenuService.GetMenuItemById(SQLDataHelper.GetInt(grid.UpdatedRow["ID"]));
                    mItem.MenuItemName = grid.UpdatedRow["MenuItemName"];
                    mItem.Blank = SQLDataHelper.GetBoolean(grid.UpdatedRow["Blank"]);
                    mItem.SortOrder = sortOrder;
                    mItem.Enabled = SQLDataHelper.GetBoolean(grid.UpdatedRow["Enabled"]);
                    MenuService.UpdateMenuItem(mItem);
                }
            }

            DataTable data = _paging.PageItems;
            while (data.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
            {
                _paging.CurrentPageIndex--;
                data = _paging.PageItems;
            }

            var clmn = new DataColumn("IsSelected", typeof(bool)) { DefaultValue = _inverseSelection };
            data.Columns.Add(clmn);
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
            lblFound.Text = _paging.TotalRowsCount.ToString();
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
                        foreach (var item in MenuService.GetAllChildIdByParent(SQLDataHelper.GetInt(id), MenuType))
                            MenuService.DeleteMenuItemById(item);
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("MenuItemID as ID");
                    foreach (var mItemId in itemsIds.Where(mId => !_selectionFilter.Values.Contains(mId.ToString())))
                    {
                        foreach (var id in MenuService.GetAllChildIdByParent(mItemId, MenuType))
                        {
                            MenuService.DeleteMenuItemById(id);
                        }
                    }
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (ddSelect.SelectedIndex != 0)
            {
                if (ddSelect.SelectedIndex == 2)
                {
                    if (_selectionFilter != null)
                    {
                        _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                    }
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            _paging.Fields["MenuItemName"].Filter = !string.IsNullOrEmpty(txtNameFilter.Text)
                ? new CompareFieldFilter {Expression = txtNameFilter.Text, ParamName = "@MenuItemName"}
                : null;
            _paging.Fields["Blank"].Filter = ddlBlank.SelectedValue != "any"
                ? new EqualFieldFilter {Value = ddlBlank.SelectedValue, ParamName = "@Blank"}
                : null;
            _paging.Fields["Enabled"].Filter = ddlEnabled.SelectedValue != "any"
                ? new EqualFieldFilter {Value = ddlEnabled.SelectedValue, ParamName = "@enabled" } : null;
            
            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            grid.ChangeHeaderImageUrl(null, null);
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                foreach (var id in MenuService.GetAllChildIdByParent(SQLDataHelper.GetInt(e.CommandArgument), MenuType))
                {
                    MenuService.DeleteMenuItemById(id);
                }
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"MenuItemName", "arrowMenuName"},
                    {"Blank", "arrowBlank"},
                    {"Enabled", "arrowEnabled"},
                    {"SortOrder","arrowSortOrder"},
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

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }

        protected void hlDeleteMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var id in MenuService.GetAllChildIdByParent(MenuId, MenuType))
            {
                MenuService.DeleteMenuItemById(id);
            }
            Response.Redirect("Menu.aspx?type=" + MenuType);
        }
    }
}