//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;

namespace Admin.UserControls
{
    public partial class PopupGridManagers : UserControl
    {
        private SqlPaging _paging;
        public List<int> SelectedManagers;

        private InSetFieldFilter _selectionFilter;

        public bool MultiSelection
        {
            get
            {
                if (ViewState["MultiSelection"] == null)
                {
                    return false;
                }

                return (bool)ViewState["MultiSelection"];
            }

            set
            {
                ViewState["MultiSelection"] = value;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DataTable data = _paging.PageItems;
            while (data.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
            {
                _paging.CurrentPageIndex--;
                data = _paging.PageItems;
            }

            var clmn = new DataColumn("IsSelected", typeof(bool)) { DefaultValue = false };
            data.Columns.Add(clmn);

            if (SelectedManagers != null)
            {
                for (int i = 0; i <= data.Rows.Count - 1; i++)
                {
                    var intIndex = i;
                    if (Array.Exists(SelectedManagers.ToArray(), c => c.ToString() == data.Rows[intIndex]["ManagerId"].ToString()))
                    {
                        data.Rows[i]["IsSelected"] = true;
                    }
                }
            }
            agvManagers.DataSource = data;
            agvManagers.DataBind();
            pageNumberer.PageCount = _paging.PageCount;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var strIds = Request.Form["SelectedIds"];

            string[] ids;

            if (!string.IsNullOrEmpty(strIds))
            {
                strIds = strIds.Trim();
                var arrids = strIds.Split(' ');

                ids = new string[arrids.Length];
                _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                for (int idx = 0; idx <= ids.Length - 1; idx++)
                {
                    var t = arrids[idx];
                    if (t != "-1")
                    {
                        ids[idx] = t;
                    }
                    else
                    {
                        _selectionFilter.IncludeValues = false;
                    }
                }

                _selectionFilter.Values = ids;
            }


            if (!IsPostBack)
            {
                _paging = new SqlPaging
                {
                    TableName = "[Customers].[Customer] INNER JOIN [Customers].[Managers] ON [Managers].[CustomerId] = [Customer].[CustomerId]", ItemsPerPage = 10
                };
                _paging.AddFieldsRange(new List<Field>()
                {
                    new Field { Name = "Managers.ManagerId", IsDistinct = true },
                    new Field { Name = "Email" },
                    new Field { Name = "Firstname", Sorting = SortDirection.Ascending },
                    new Field { Name = "Lastname" },
                    new Field { Name = "Phone" }
                });

                agvManagers.ChangeHeaderImageUrl("arrowFirstname", UrlService.GetAdminAbsoluteLink("images/arrowup.gif"));
                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
                ViewState["Show"] = false;
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);

                if (_paging == null)
                {
                    throw (new Exception("Paging lost"));
                }

                if (!string.IsNullOrEmpty(hfSelectedManager.Value))
                {
                    SelectedManagers = new List<int> { hfSelectedManager.Value.TryParseInt() };
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            //----Firstname filter
            _paging.Fields["Firstname"].Filter = !string.IsNullOrEmpty(txtSearchFirstName.Text) ? new CompareFieldFilter { Expression = txtSearchFirstName.Text, ParamName = "@firstname" } : null;

            //----lastname filter
            _paging.Fields["Lastname"].Filter = !string.IsNullOrEmpty(txtSearchLastname.Text) ? new CompareFieldFilter { Expression = txtSearchLastname.Text, ParamName = "@lastname" } : null;

            //----email filter
            _paging.Fields["Email"].Filter = !string.IsNullOrEmpty(txtSearchEmail.Text) ? new CompareFieldFilter { Expression = txtSearchEmail.Text, ParamName = "@email" } : null;

            //----phone filter
            _paging.Fields["Phone"].Filter = !string.IsNullOrEmpty(txtSearchPhone.Text) ? new CompareFieldFilter { Expression = txtSearchPhone.Text, ParamName = "@phone" } : null;

            ViewState["Show"] = true;
            pageNumberer.CurrentPageIndex = 1;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchFirstName.Text = string.Empty;
            txtSearchLastname.Text = string.Empty;
            txtSearchEmail.Text = string.Empty;
            txtSearchPhone.Text = string.Empty;
            btnFilter_Click(sender, e);
            agvManagers.DataBind();
            ViewState["Show"] = true;
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
            if (pagen < 1 || pagen > _paging.PageCount)
                return;
            pageNumberer.CurrentPageIndex = pagen;
            _paging.CurrentPageIndex = pagen;
            ViewState["Show"] = true;
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
            ViewState["Show"] = true;
        }

        protected void agv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void agvManagers_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Lastname", "arrowLastname"},
                    {"Firstname", "arrowFirstname"},
                    {"Email", "arrowEmail"},
                    {"Phone", "arrowPhone"},
                };

            string urlArrowUp = UrlService.GetAdminAbsoluteLink("images/arrowup.gif");
            string urlArrowDown = UrlService.GetAdminAbsoluteLink("images/arrowdown.gif");
            string urlArrowGray = UrlService.GetAdminAbsoluteLink("images/arrowdownh.gif");

            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                agvManagers.ChangeHeaderImageUrl(arrows[csf.Name],
                                                  (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
            }
            else
            {
                csf.Sorting = null;
                agvManagers.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);
                nsf.Sorting = SortDirection.Ascending;
                agvManagers.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
        }

        protected void btnSaveManager_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfSelectedManager.Value))
            {
                SelectedManagers = new List<int> { hfSelectedManager.Value.TryParseInt() };
            }
        }

        public void CleanSelection()
        {
            hfSelectedManager.Value = "";
        }
    }
}