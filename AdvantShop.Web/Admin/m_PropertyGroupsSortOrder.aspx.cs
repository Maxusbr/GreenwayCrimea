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
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Resources;

namespace Admin
{
    public partial class m_PropertyGroupsSortOrder : AdvantShopAdminPage
    {
        private SqlPaging _paging;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_m_CategorySortOrder_Title));

            divSave.Visible = false;
            grid.ResetToDefaultValueOnRowEditCancel = false;

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                {
                    TableName = "Catalog.PropertyGroup",
                    ItemsPerPage = SQLDataHelper.GetInt(ddRowsPerPage.SelectedValue)
                };

                _paging.AddFieldsRange(new List<Field>
                {
                    new Field { Name = "PropertyGroupId as ID", IsDistinct = true },
                    new Field { Name = "GroupName" },
                    new Field { Name = "GroupSortOrder", Sorting = SortDirection.Ascending }
                });

                grid.ChangeHeaderImageUrl("arrowGroupSortOrder", "images/arrowup.gif");

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

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            grid.DataSource = _paging.PageItems;
            grid.DataBind();
            pageNumberer.PageCount = _paging.PageCount;
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"GroupName", "arrowGroupName"},
                    {"GroupSortOrder", "arrowGroupSortOrder"}
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
                    PropertyGroupService.UpdateSortOrder(grid.UpdatedRow["ID"].TryParseInt(), grid.UpdatedRow["GroupSortOrder"].TryParseInt());
                }
            }
            divSave.Visible = true;
        }

        protected void linkGO_Click(object sender, EventArgs e)
        {
            var pagen = txtPageNum.Text.TryParseInt(-1);

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