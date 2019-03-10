//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Admin.Handlers.Statistics;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using Resources;

namespace Admin
{
    public partial class StatisticsFilter : AdvantShopAdminPage
    {
        private SqlPaging _paging;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Request["type"]))
            {
                return;
            }

            var type = Request["type"].ToLower();

            if (type == "abcxyz")
            {
                AbcXyzAnalysis();
            }
            else if (type == "rfm")
            {
                RfmAnalysis();
            }

            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Statistics_Header));
        }

        private void AbcXyzAnalysis()
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrWhiteSpace(Request["from"]) || string.IsNullOrWhiteSpace(Request["to"]) ||
                    string.IsNullOrWhiteSpace(Request["group"]))
                {
                    return;
                }
                
                var from = DateTime.Parse(Request["from"]);
                var to = DateTime.Parse(Request["to"]);
                to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);

                var group = Request["group"].ToUpper();

                lblTitle.Text = string.Format("Товары в группе {0} с {1} по {2}", group, from.ToString("dd.MM.yy"), to.ToString("dd.MM.yy"));
                switch (group)
                {
                    case "AX":
                        lblTitle.Text += Resource.StatisticsFilter_AbcXyzAnalysis_AX;
                        break;
                    case "AY":
                        lblTitle.Text += Resource.StatisticsFilter_AbcXyzAnalysis_AY;
                        break;
                    case "AZ":
                        lblTitle.Text += Resource.StatisticsFilter_AbcXyzAnalysis_AZ;
                        break;

                    case "BX":
                        lblTitle.Text += Resource.StatisticsFilter_AbcXyzAnalysis_BX;
                        break;
                    case "BY":
                        lblTitle.Text += Resource.StatisticsFilter_AbcXyzAnalysis_BY;
                        break;
                    case "BZ":
                        lblTitle.Text += Resource.StatisticsFilter_AbcXyzAnalysis_BZ;
                        break;

                    case "CX":
                        lblTitle.Text += Resource.StatisticsFilter_AbcXyzAnalysis_CX;
                        break;
                    case "CY":
                        lblTitle.Text += Resource.StatisticsFilter_AbcXyzAnalysis_CY;
                        break;
                    case "CZ":
                        lblTitle.Text += Resource.StatisticsFilter_AbcXyzAnalysis_CZ;
                        break;
                }


                var handler = new AbcxyzAnalysisHandler(from, to);
                var data = handler.GetDataItems();

                if (data == null)
                    return;

                var items = data.Where(x => x.Abc == group[0].ToString() && x.Xyz == group[1].ToString()).ToList();
                if (items.Count == 0)
                    return;

                var artNos = items.Select(x => "'" + x.ArtNo.Replace("'",string.Empty) + "'");

                _paging = new SqlPaging
                {
                    ItemsPerPage = 20,
                    TableName = "Catalog.Product " +
                                "left Join Catalog.Currency ON Product.CurrencyID = Currency.CurrencyID " +
                                "Inner Join Catalog.Offer On Product.ProductId = Offer.ProductId and Offer.ArtNo in (" +
                                String.Join(",", artNos) + ") "
                };

                _paging.AddFieldsRange(
                    new List<Field>
                    {
                        new Field {Name = "Offer.ArtNo", IsDistinct = true},
                        new Field {Name = "Product.Name"},
                        new Field {Name = "Price"},
                        new Field {Name = "[Currency].Code"},
                        new Field {Name = "[Currency].CurrencyIso3"},
                        new Field {Name = "[Currency].CurrencyValue"},
                        new Field {Name = "[Currency].IsCodeBefore"},
                    });

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);
                _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
            }

            var dataItems = _paging.PageItems;
            while (dataItems.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
            {
                _paging.CurrentPageIndex--;
                dataItems = _paging.PageItems;
            }

            grid.DataSource = dataItems;
            grid.DataBind();

            pageNumberer.PageCount = _paging.PageCount;

            var itemsCount = _paging.TotalRowsCount;
            lblFound.Text = itemsCount.ToString();
            tbFound.Visible = itemsCount > 0;
        }


        private void RfmAnalysis()
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrWhiteSpace(Request["group"]))
                {
                    return;
                }

                var arr = Request["group"].ToUpper().Split('_');
                if (arr.Length != 4)
                    return;
                
                lblTitle.Text = string.Format("Покупатели в группе R = {0}, {1} = {2}", arr[2], arr[1], arr[3]);

                var handler = new RFMAnalysisHandler();
                var data = handler.GetDataItems();

                if (data == null)
                    return;

                var items = arr[1] == "M"
                    ? data.Where(x => x.R.ToString() == arr[2] && x.M.ToString() == arr[3]).ToList()
                    : data.Where(x => x.R.ToString() == arr[2] && x.F.ToString() == arr[3]).ToList();

                if (items.Count == 0)
                    return;

                var customerIds = items.Select(x => "'" + x.CustomerId + "'");

                _paging = new SqlPaging
                {
                    ItemsPerPage = 20,
                    TableName = "[order].[ordercustomer] Where CustomerId In (" + String.Join(",", customerIds) + ") "
                };

                _paging.AddFieldsRange(
                    new List<Field>
                    {
                        new Field {Name = "CustomerId", IsDistinct = true},
                        new Field {Name = "(Firstname + ' ' + Lastname) as Name"},
                        new Field {Name = "Email "},
                        new Field {Name = "Phone"},
                        new Field {Name = "OrderId"}
                    });

                pageNumberer.CurrentPageIndex = 1;
                _paging.CurrentPageIndex = 1;
                ViewState["Paging"] = _paging;
            }
            else
            {
                _paging = (SqlPaging)(ViewState["Paging"]);
                _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
            }

            var dataItems = _paging.PageItems;
            while (dataItems.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
            {
                _paging.CurrentPageIndex--;
                dataItems = _paging.PageItems;
            }

            gridCustomers.DataSource = dataItems;
            gridCustomers.DataBind();

            pageNumberer.PageCount = _paging.PageCount;

            var itemsCount = _paging.TotalRowsCount;
            lblFound.Text = itemsCount.ToString();
            tbFound.Visible = itemsCount > 0;
        }

        protected void pn_SelectedPageChanged(object sender, EventArgs e)
        {
            
        }
    }
}