//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using Resources;

namespace Admin
{
    public partial class Certificates : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_CertificateAdmin_Header));

            if (!SettingsCheckout.EnableGiftCertificateService)
            {
                lblCertificateAttention.Text = Resource.Admin_CertificateAdmin_NotEnabled;
                lblCertificateAttention.Visible = true;
            }

            if (!IsPostBack)
            {
                _paging = new SqlPaging
                    {
                        TableName = "[Order].[Certificate] INNER JOIN [Order].[Order] ON [Order].[OrderID] = [Certificate].[OrderID] INNER JOIN [Order].[OrderCurrency] ON [OrderCurrency].[OrderID] = [Certificate].[OrderID]",
                        ItemsPerPage = 10
                    };

                _paging.AddFieldsRange(new List<Field>
                    {
                        new Field
                            {
                                Name = "CertificateID as ID",
                                IsDistinct = true
                            },
                        new Field {Name = "[Certificate].CertificateCode as CertificateCode"},
                        new Field {Name = "[Certificate].ApplyOrderNumber as ApplyOrderNumber"},
                        new Field {Name = "[Certificate].OrderId as OrderId"},
                        new Field {Name = "[Certificate].Sum as Sum"},
                        new Field {Name = "[Certificate].Used as Used"},
                        new Field {Name = "[Certificate].Enable as Enable"},
                        new Field {Name = "[Certificate].CreationDate as CreationDate", Sorting = SortDirection.Descending},
                        //OrderCurrency
                        new Field {Name = "[OrderCurrency].CurrencyValue as CurrencyValue"},
                        new Field {Name = "[OrderCurrency].CurrencyCode as CurrencyCode"},
                        new Field {Name = "[OrderCurrency].CurrencySymbol as CurrencySymbol"},
                        new Field {Name = "[OrderCurrency].IsCodeBefore as IsCodeBefore"},
                        //Order
                        new Field {Name = "[Order].PaymentDate as PaymentDate"},
                        //new Field {Name = "[Order].PaymentDate is null as Paid"}
                    });
                
                grid.ChangeHeaderImageUrl("arrowCreationDate", "images/arrowdown.gif");

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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            //-----Selection filter
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

            //----Name filter
            _paging.Fields["CertificateCode"].Filter = !string.IsNullOrEmpty(txtCertificateCode.Text) ? new CompareFieldFilter { Expression = txtCertificateCode.Text, ParamName = "@CertificateCode" } : null;
            _paging.Fields["OrderNumber"].Filter = !string.IsNullOrEmpty(txtOrderNumber.Text) ? new CompareFieldFilter { Expression = txtOrderNumber.Text, ParamName = "@OrderNumber" } : null;
            _paging.Fields["Sum"].Filter = !string.IsNullOrEmpty(txtSumFilter.Text) ? new CompareFieldFilter { Expression = txtSumFilter.Text, ParamName = "@Sum" } : null;
            _paging.Fields["CreationDate"].Filter = !string.IsNullOrEmpty(txtCreationDate.Text) ? new CompareFieldFilter { Expression = txtCreationDate.Text, ParamName = "@CreationDate" } : null;
            //_paging.Fields["Paid"].Filter = (ddlPaidFilter.SelectedValue != "any")
            //    ? new NotEqualFieldFilter { ParamName = "@Paid", Value = ddlPaidFilter.SelectedValue } : null;
            _paging.Fields["Enable"].Filter = (ddlEnabledFilter.SelectedValue != "any")
                                                  ? new EqualFieldFilter { ParamName = "@Enable", Value = ddlEnabledFilter.SelectedValue } : null;

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
                        GiftCertificateService.DeleteCertificateById(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("CertificateID as ID");
                    foreach (var id in itemsIds.Where(certificateId => !_selectionFilter.Values.Contains(certificateId.ToString())))
                    {
                        GiftCertificateService.DeleteCertificateById(id);
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCertificate")
            {
                GiftCertificateService.DeleteCertificateById(SQLDataHelper.GetInt(e.CommandArgument));
            }
            if (e.CommandName == "SendCertificate")
            {
                GiftCertificateService.SendCertificateMails(
                    GiftCertificateService.GetCertificateById(SQLDataHelper.GetInt(e.CommandArgument)));
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"CertificateCode", "arrowCertificateCode"},
                    {"ApplyOrderNumber", "arrowApplyOrderNumber"},
                    {"OrderId", "arrowOrderId"},
                    {"Sum", "arrowSum"},
                    {"PaymentDate", "arrowPaid"},
                    {"Enable", "arrowEnable"},
                    {"CreationDate", "arrowCreationDate"},
                    {"Used", "arrowUsed"}
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
                var certificate =
                    GiftCertificateService.GetCertificateById(SQLDataHelper.GetInt(grid.UpdatedRow["ID"]));

                //if (!certificate.Paid && SQLDataHelper.GetBoolean(grid.UpdatedRow["Paid"]) && !CustomerContext.CurrentCustomer.IsVirtual)
                //{
                //    GiftCertificateService.SendCertificateMails(certificate);
                //}
                OrderService.PayOrder(certificate.OrderId, SQLDataHelper.GetBoolean(grid.UpdatedRow["Paid"]));

                certificate.Enable = SQLDataHelper.GetBoolean(grid.UpdatedRow["Enable"]);
                certificate.Used = SQLDataHelper.GetBoolean(grid.UpdatedRow["Used"]);
                GiftCertificateService.UpdateCertificateById(certificate);
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

        protected void sds_Init(object sender, EventArgs e)
        {
            ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
        }

        protected void btnSettings_Click(object sender, EventArgs e)
        {
            Response.Redirect("CertificatesOptions.aspx");
        }
    }
}