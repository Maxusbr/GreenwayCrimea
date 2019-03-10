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
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.SEO;

namespace Admin
{
    public partial class LogError404 : AdvantShopAdminPage
    {
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;
        private bool _inverseSelection;

        public LogError404()
        {
            _inverseSelection = false;
        }

        private void MsgErr(string message)
        {
            pnlErrors.InnerHtml += string.Format("<div>{0}</div>", message);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resources.Resource.Admin_LogError404_Head));
            pnlErrors.InnerHtml = string.Empty;

            if (!IsPostBack)
            {
                chbEnabled301Redirect.Checked = SettingsSEO.Enabled301Redirects;

                _paging = new SqlPaging
                {
                    TableName = "Settings.Error404 LEFT JOIN Settings.Redirect ON Error404.Url = Redirect.RedirectFrom",
                    ItemsPerPage = 20
                };

                _paging.AddFieldsRange(new List<Field>
                {
                    new Field("Error404.Id as ID") {IsDistinct = false, Filter = _selectionFilter, Sorting = SortDirection.Descending},
                    new Field("Url"),
                    new Field("UrlReferer"),
                    new Field("UserAgent"),
                    new Field("IpAddress"),
                    new Field("RedirectTo")
                });

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

                    var ids = new string[arrids.Length];
                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        int t = int.Parse(arrids[idx]);
                        if (t != -1)
                        {
                            ids[idx] = t.ToString();
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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            //-----Selection filter
            if (ddSelect.SelectedValue != "any")
            {
                if (ddSelect.SelectedValue == "0")
                {
                    if (_selectionFilter != null)
                        _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                    else
                        _selectionFilter = null;
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = null;
            }

            _paging.Fields["Url"].Filter = txtUrl.Text.IsNotEmpty()
                ? new CompareFieldFilter { ParamName = "@Url", Expression = txtUrl.Text }
                : null;

            _paging.Fields["UrlReferer"].Filter = txtUrlReferer.Text.IsNotEmpty()
                ? new CompareFieldFilter { ParamName = "@UrlReferer", Expression = txtUrlReferer.Text }
                : null;

            _paging.Fields["UserAgent"].Filter = txtUserAgent.Text.IsNotEmpty()
                ? new CompareFieldFilter { ParamName = "@UserAgent", Expression = txtUserAgent.Text }
                : null;

            _paging.Fields["IpAddress"].Filter = txtIpAddress.Text.IsNotEmpty()
                ? new CompareFieldFilter { ParamName = "@IpAddress", Expression = txtIpAddress.Text }
                : null;


            _paging.Fields["RedirectTo"].Filter = ddlHasRedirect.SelectedValue != "any"
                ? new NullFieldFilter { ParamName = "@RedirectTo", Null = ddlHasRedirect.SelectedValue == "0" }
                : null;

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
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
            var pagen = txtPageNum.Text.TryParseInt(-1);
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
                        Error404Service.DeleteError404(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("Error404.Id as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        Error404Service.DeleteError404(id);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteError404")
            {
                Error404Service.DeleteError404(SQLDataHelper.GetInt(e.CommandArgument));
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"ID", "arrowID"},
                    {"Url", "arrowUrl"},
                    {"UrlReferer", "arrowUrlReferer"},
                    {"UserAgent", "arrowUserAgent"},
                    {"IpAddress", "arrowIpAddress"}
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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grid.UpdatedRow != null)
            {
                var redirectFrom = grid.UpdatedRow["Url"].Trim().ToLower();
                var redirectTo = grid.UpdatedRow["RedirectTo"].Trim().ToLower();                

                if (redirectTo.IsNullOrEmpty())
                {
                    MsgErr(Resources.Resource.Admin_LogError404_EnterRedirectTo);
                    return;
                }

                if (RedirectSeoService.GetRedirectsSeoByRedirectFrom(redirectFrom) != null)
                {
                    MsgErr(Resources.Resource.Admin_LogError404_RedirectExist);
                    return;
                }

                var redirectSeo = new RedirectSeo
                {
                    RedirectFrom = redirectFrom,
                    RedirectTo = redirectTo,
                    ProductArtNo = string.Empty
                };

                RedirectSeoService.AddRedirectSeo(redirectSeo);
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
                    if (Array.Exists(_selectionFilter.Values, c => c == data.Rows[intIndex]["ID"].ToString()))
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

        protected void chbEnabled301Redirect_CheckedChanged(object sender, EventArgs e)
        {
            SettingsSEO.Enabled301Redirects = chbEnabled301Redirect.Checked;
        }
    }
}