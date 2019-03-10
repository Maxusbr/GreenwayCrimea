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
using AdvantShop;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class StaticBlocks : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;
    
        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_PageParts_StaticBlocks));
            if (!IsPostBack)
            {
                _paging = new SqlPaging
                    {
                        TableName = "[CMS].[StaticBlock]",
                        ItemsPerPage = 50
                    };

                _paging.AddFieldsRange(new List<Field>
                    {
                        new Field {Name = "StaticBlockID as ID",IsDistinct = true},
                        new Field {Name = "[Key]", Sorting = SortDirection.Ascending},
                        new Field {Name = "InnerName"},
                        new Field {Name = "Added"},
                        new Field {Name = "Modified"},
                        new Field{Name = "Enabled"}
                    });

                grid.ChangeHeaderImageUrl("arrowKey", "images/arrowup.gif");

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

            if (Request["key"].IsNotEmpty())
            {
                _paging.Fields["[Key]"].Filter = new CompareFieldFilter
                {
                    Expression = Request["key"],
                    ParamName = "@Key"
                };
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
            if (Request["key"].IsNotEmpty())
                txtKey.Text = Request["key"];

            _paging.Fields["[Key]"].Filter = !string.IsNullOrEmpty(txtKey.Text) ? new CompareFieldFilter { Expression = txtKey.Text, ParamName = "@Key" } : null;
            _paging.Fields["InnerName"].Filter = !string.IsNullOrEmpty(txtTitle.Text) ? new CompareFieldFilter { Expression = txtTitle.Text, ParamName = "@InnerName" } : null;
            //TODO date filters

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
                        StaticBlockService.DeleteBlock(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("StaticBlockID as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        StaticBlockService.DeleteBlock(id);
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
                        StaticBlockService.SetStaticBlockActivity(SQLDataHelper.GetInt(id), true);
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("StaticBlockID as ID");
                    foreach (int id in itemsIds.Where(cId => !_selectionFilter.Values.Contains(cId.ToString(CultureInfo.InvariantCulture))))
                    {
                        StaticBlockService.SetStaticBlockActivity(SQLDataHelper.GetInt(id), true);
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
                        StaticBlockService.SetStaticBlockActivity(SQLDataHelper.GetInt(id), false);
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("StaticBlockID as ID");
                    foreach (int id in itemsIds.Where(cId => !_selectionFilter.Values.Contains(cId.ToString(CultureInfo.InvariantCulture))))
                    {
                        StaticBlockService.SetStaticBlockActivity(SQLDataHelper.GetInt(id), false);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteBlock")
            {
                StaticBlockService.DeleteBlock(SQLDataHelper.GetInt(e.CommandArgument));
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"[Key]", "arrowKey"},
                    {"InnerName", "arrowInnerName"},
                    {"Added", "arrowAdded"},
                    {"Modified", "arrowModified"},
                    {"Enabled", "arrowEnabled"}
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
            if (grid.UpdatedRow != null)
            {
                var part = StaticBlockService.GetPagePart(SQLDataHelper.GetInt(grid.UpdatedRow["ID"]));

                part.InnerName = grid.UpdatedRow["InnerName"];
                part.Key = grid.UpdatedRow["[Key]"];
                part.Enabled = SQLDataHelper.GetBoolean(grid.UpdatedRow["Enabled"]);

                if (part.InnerName.IsNotEmpty() && part.Key.IsNotEmpty())
                    StaticBlockService.UpdatePagePart(part);
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

        protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void btnAddBlock_Click(object sender, EventArgs e)
        {
            Response.Redirect("StaticBlock.aspx");
        }

        protected void sds_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
        }
    }
}