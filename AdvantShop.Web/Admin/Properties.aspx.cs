//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using Resources;

namespace Admin
{
    public partial class Properties : AdvantShopAdminPage
    {
        #region Fields

        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        private int _groupId;
        protected int GroupId
        {
            get { return _groupId != 0 ? _groupId : (_groupId = Request["groupId"].TryParseInt()); }
        }
        
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Properties_ListPropreties));

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                    {
                        TableName = "[Catalog].[Property]",
                        ItemsPerPage = 20,
                        CurrentPageIndex = 1
                    };

                _paging.AddFieldsRange(new[]
                    {
                        new Field {Name = "Property.PropertyID as ID", IsDistinct = true},
                        new Field {Name = "Name"},
                        new Field {Name = "UseInFilter"},
                        new Field {Name = "UseInDetails"},
                        new Field {Name = "UseInBrief"},
                        new Field {Name = "ISNULL(GroupId, 0) as GroupId"},
                        new Field {Name = "SortOrder", Sorting = SortDirection.Ascending},
                        new Field {Name = "(Select Count(ProductID) from Catalog.ProductPropertyValue Where [PropertyValueID] in (SELECT [PropertyValueID]  FROM [Catalog].[PropertyValue] WHERE [PropertyID] = [Property].PropertyID)) as ProductsCount"},
                    });

                _paging.Fields["GroupId"].Filter = GroupId != -1
                    ? new EqualFieldFilter {ParamName = "@GroupId", Value = GroupId.ToString()}
                    : null;
                ddlPropertyGroup.Visible = GroupId == -1;

                grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");
                pageNumberer.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);
                _paging.ItemsPerPage = Convert.ToInt32(ddRowsPerPage.SelectedValue);

                if (_paging == null)
                {
                    throw (new Exception("Paging lost"));
                }

                string strIds = Request.Form["SelectedIds"];

                if (!string.IsNullOrEmpty(strIds))
                {
                    strIds = strIds.Trim();
                    var arrids = strIds.Split(' ');

                    var ids = new string[arrids.Length];
                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        int t = int.Parse(arrids[idx]);
                        if (t != -1)
                        {
                            ids[idx] = t.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            _selectionFilter.IncludeValues = false;
                            _inverseSelection = true;
                        }
                    }
                    _selectionFilter.Values = ids;
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {
                var property = PropertyService.GetPropertyById(grid.UpdatedRow["ID"].TryParseInt());
                if (property != null)
                {
                    property.Name = grid.UpdatedRow["Name"];
                    property.UseInFilter = Convert.ToBoolean(grid.UpdatedRow["UseInFilter"]);
                    property.UseInDetails = Convert.ToBoolean(grid.UpdatedRow["UseInDetails"]);
                    property.UseInBrief = Convert.ToBoolean(grid.UpdatedRow["UseInBrief"]);
                    property.SortOrder = grid.UpdatedRow["SortOrder"].TryParseInt();
                    
                    int groupId = grid.UpdatedRow["GroupId"].TryParseInt();
                    property.GroupId = groupId == 0 ? (int?)null : groupId;

                    PropertyService.UpdateProperty(property);
                }
            }

            LoadGroups();
            btnAddProperty.OnClientClick = "open_window('m_Property.aspx" + (GroupId > 0 ? "?groupId=" + GroupId : string.Empty) + "', 750, 640); return false;";
            

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
            lblFound.Text = _paging.TotalRowsCount.ToString(CultureInfo.InvariantCulture);
        }

        #region Group methods

        private void LoadGroups()
        {
            var groups = PropertyGroupService.GetList();
            groups.Insert(0, new PropertyGroup() { Name = Resource.Admin_Properties_UnlinkedProperties, PropertyGroupId = 0 });

            ddlAllGroups.DataSource = groups;
            ddlAllGroups.DataBind();

            groups.Insert(0, new PropertyGroup() { Name = Resource.Admin_Properties_AllProperties, PropertyGroupId = -1 });

            lvGroups.DataSource = groups;
            lvGroups.DataBind();
        }

        protected void lvGroups_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (string.Equals("DeleteGroup", e.CommandName))
            {
                PropertyGroupService.Delete(Convert.ToInt32(e.CommandArgument));
            }
        }

        protected void btnChangeGroup_Click(object sender, EventArgs e)
        {
            int? groupId = hfgroupId.Value.TryParseInt() != 0
                                ? hfgroupId.Value.TryParseInt()
                                : default(int?);

            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        PropertyService.UpdateGroup(Convert.ToInt32(id), groupId);
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("PropertyID as ID");
                    foreach (var id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        PropertyService.UpdateGroup(id, groupId);
                    }
                }

                _selectionFilter = null;
                hfgroupId.Value = "";

                _paging = (SqlPaging)(ViewState["Paging"]);
                _paging.ItemsPerPage = Convert.ToInt32(ddRowsPerPage.SelectedValue);
            }
        }

        #endregion

        #region Property methods

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            //-----Selection filter
            if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "0") != 0)
            {
                if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "2") == 0 && _selectionFilter != null)
                {
                    _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            //----Name filter
            _paging.Fields["Name"].Filter = string.IsNullOrEmpty(txtName.Text)
                                                ? null
                                                : new CompareFieldFilter
                                                {
                                                    Expression = txtName.Text,
                                                    ParamName = "@Name"
                                                };

        
            _paging.Fields["GroupId"].Filter = ddlPropertyGroup.SelectedValue != "Any" || GroupId != -1
                                                       ? new EqualFieldFilter
                                                       {
                                                           ParamName = "@GroupId",
                                                           Value = GroupId == -1 ? ddlPropertyGroup.SelectedValue : GroupId.ToString()
                                                       }
                                                       : null;

        
            _paging.Fields["UseInFilter"].Filter = ddlUseInFilter.SelectedValue != "Any"
                                                       ? new EqualFieldFilter
                                                       {
                                                           ParamName = "@UseInFilter",
                                                           Value = ddlUseInFilter.SelectedValue
                                                       }
                                                       : null;

          
            _paging.Fields["UseInDetails"].Filter = ddlUseInDetails.SelectedValue != "Any"
                                                       ? new EqualFieldFilter
                                                       {
                                                           ParamName = "@UseInDetails",
                                                           Value = ddlUseInDetails.SelectedValue
                                                       }
                                                       : null;

            
            _paging.Fields["UseInBrief"].Filter = ddlUseInBrief.SelectedValue != "Any"
                                                       ? new EqualFieldFilter
                                                       {
                                                           ParamName = "@UseInBrief",
                                                           Value = ddlUseInBrief.SelectedValue
                                                       }
                                                       : null;

           
            _paging.Fields["SortOrder"].Filter = string.IsNullOrEmpty(txtSortOrder.Text)
                                                     ? null
                                                     : new CompareFieldFilter
                                                     {
                                                         ParamName = "@SortOrder",
                                                         Expression = txtSortOrder.Text
                                                     };

            _paging.Fields["ProductsCount"].Filter = string.IsNullOrEmpty(txtProductsCount.Text)
                                                         ? null
                                                         : new EqualFieldFilter
                                                         {
                                                             ParamName = "@ProductsCount",
                                                             Value = txtProductsCount.Text
                                                         };


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
            MassAction(PropertyService.DeleteProperty);
        }

        protected void lbSetInFilter_Click(object sender, EventArgs e)
        {
            MassAction(PropertyService.ShowInFilter, true);
        }

        protected void lbSetNotInFilter_Click(object sender, EventArgs e)
        {
            MassAction(PropertyService.ShowInFilter, false);
        }

        protected void lbUseInDetails_Click(object sender, EventArgs e)
        {
            MassAction(PropertyService.ShowInDetails, true);
        }

        protected void lbNotUseInDetails_Click(object sender, EventArgs e)
        {
            MassAction(PropertyService.ShowInDetails, false);
        }

        protected void lbUseInBrief_Click(object sender, EventArgs e)
        {
            MassAction(PropertyService.ShowInBrief, true);
        }

        protected void lbNotUseInBrief_Click(object sender, EventArgs e)
        {
            MassAction(PropertyService.ShowInBrief, false);
        }

        private void MassAction(Action<int, bool> func, bool param)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        func(Convert.ToInt32(id), param);
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("PropertyID as ID");
                    foreach (var id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        func(Convert.ToInt32(id), param);
                    }
                }
            }
        }

        private void MassAction(Action<int> func)
        {
            if ((_selectionFilter != null) && (_selectionFilter.Values != null))
            {
                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        func(Convert.ToInt32(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("PropertyID as ID");
                    foreach (var id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                    {
                        func(Convert.ToInt32(id));
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProperty")
            {
                PropertyService.DeleteProperty(Convert.ToInt32(e.CommandArgument));
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Name", "arrowName"},
                    {"UseInFilter", "arrowUseInFilter"},
                    {"UseInDetails", "arrowUseInDetails"},
                    {"UseInBrief", "arrowUseInBrief"},
                    {"Expanded", "arrowExpanded"},
                    {"SortOrder", "arrowSortOrder"},
                    {"ProductsCount", "arrowProductsCount"},

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

        protected string RenderProductsCount(int propId)
        {
            return PropertyService.GetProductsCountByProperty(propId).ToString();
        }
        
        #endregion

        protected void sds_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = AdvantShop.Connection.GetConnectionString();
        }

        protected void ddlPropertyGroup_DataBound(object sender, EventArgs e)
        {
            ddlPropertyGroup.Items.Insert(0, new ListItem(Resource.Admin_Properties_UngroupedProperties, "0"));
            ddlPropertyGroup.Items.Insert(0, new ListItem(Resource.Admin_Catalog_Any, "Any"));
        }

        protected void ddlGroupId_DataBound(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Insert(0, new ListItem(Resource.Admin_Properties_UngroupedProperties, "0"));
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                ((DropDownList)e.Row.FindControl("ddlGroupId")).SelectedValue =
                    ((DataRowView)e.Row.DataItem)["GroupId"].ToString();
        }
    }
}