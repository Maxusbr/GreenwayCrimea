//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.UI.WebControls;

using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.ExportImport;
using AdvantShop.Helpers;

using Newtonsoft.Json;
using AdvantShop.Core.Common.Attributes;

namespace Admin
{
    public partial class Resellers : AdvantShopAdminPage
    {
        SqlPaging _paging;
        InSetFieldFilter _selectionFilter;
        bool _inverseSelection;

        public Resellers()
        {
            _inverseSelection = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, "Дропшипперы"));

            if (!IsPostBack)
            {
                _paging = new SqlPaging { TableName = "[Settings].[ExportFeed]", ItemsPerPage = 20 };

                _paging.AddFieldsRange(
                    new List<Field>
                    {
                        new Field {Name = "Id as ID", IsDistinct = true},
                        new Field {Name = "Name", Sorting = SortDirection.Ascending},
                        new Field {Name = "LastExport"},
                        new Field {Name = "Type"},
                        new Field {Name = "Description"},
                    });
                _paging.AddCondition("[Type] = '" + EExportFeedType.Reseller.ToString() + "'");

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
                    int t;
                    _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                    for (int idx = 0; idx <= ids.Length - 1; idx++)
                    {
                        t = int.Parse(arrids[idx]);
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
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtName.Text, ParamName = "@Name" };
                _paging.Fields["Name"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["Name"].Filter = null;
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            btnFilter_Click(sender, e);
            advGridResellers.ChangeHeaderImageUrl(null, null);

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
                        ExportFeedService.DeleteExportFeed(SQLDataHelper.GetInt(id));
                    }
                }
                else
                {
                    foreach (var id in _paging.ItemsIds<string>("ID as ID").Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        ExportFeedService.DeleteExportFeed(SQLDataHelper.GetInt(id));
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteReseller")
            {
                ExportFeedService.DeleteExportFeed(SQLDataHelper.GetInt(e.CommandArgument));
            }

            if (e.CommandName == "AddReseller")
            {
                var footer = advGridResellers.FooterRow;
                if (
                    string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewName")).Text)
                    //|| string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewPurchaseDiscount")).Text)
                    //|| string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewRecommendedPriceMargin")).Text)
                    )
                {
                    advGridResellers.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                    return;
                }

                var resellerCode = Guid.NewGuid();

                var exportFeedId = ExportFeedService.AddExportFeed(new ExportFeed
                {
                    Name = ((TextBox)footer.FindControl("txtNewName")).Text,
                    Type = EExportFeedType.Reseller
                });

                ExportFeedService.InsertCategory(exportFeedId, 0);

                ExportFeedSettingsProvider.SetSettings(exportFeedId, new ExportFeedResellerOptions()
                {
                    ResellerCode = resellerCode.ToString(),
                    PriceMargin = 0,
                    FileName = "app_data/resellers/" + resellerCode,
                    CsvSeparator = ";",
                    CsvColumSeparator = ";",
                    CsvCategorySort = true,
                    CsvPropertySeparator = ":",
                    CsvEnconing = EncodingsEnum.Utf8.StrName(),
                    //ExportNotActiveProducts = true,
                    //ExportNotAmountProducts = true,                    
                    FileExtention = "csv",
                    FieldMapping = new List<ProductFields>(Enum.GetValues(typeof(ProductFields)).OfType<ProductFields>().Where(item => item != ProductFields.None).ToList())
                });

                advGridResellers.ShowFooter = false;
            }
            if (e.CommandName == "CancelAdd")
            {
                advGridResellers.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
                advGridResellers.ShowFooter = false;
            }
        }

        protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
                {
                    {"Name", "arrowName"},
                    //{"PurchaseDiscount", "arrowPurchaseDiscount"},
                    //{"RecommendedPriceMargin", "arrowRecommendedPriceMargin"},
                    //{"ExportNotActiveProducts", "arrowExportNotActiveProducts"},
                    //{"ExportNotAmountProducts", "arrowExportNotAmountProducts"},
                    
                };
            const string urlArrowUp = "images/arrowup.gif";
            const string urlArrowDown = "images/arrowdown.gif";
            const string urlArrowGray = "images/arrowdownh.gif";


            Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
            Field nsf = _paging.Fields[e.SortExpression];

            if (nsf.Name.Equals(csf.Name))
            {
                csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                advGridResellers.ChangeHeaderImageUrl(arrows[csf.Name], (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
            }
            else
            {
                csf.Sorting = null;
                advGridResellers.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

                nsf.Sorting = SortDirection.Ascending;
                advGridResellers.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
            }

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var exportFeedId = 0;
            if (advGridResellers.UpdatedRow != null && Int32.TryParse(advGridResellers.UpdatedRow["ID"], out exportFeedId))
            {
                var exportFeed = ExportFeedService.GetExportFeed(exportFeedId);
                exportFeed.Name = advGridResellers.UpdatedRow["Name"];

                ExportFeedService.UpdateExportFeed(exportFeed);
                var commonSettings = ExportFeedSettingsProvider.GetSettings(exportFeedId);
                var advancedSettings = ExportFeedSettingsProvider.ConvertAdvancedSettings<ExportFeedResellerOptions>(commonSettings.AdvancedSettings);


                commonSettings.PriceMargin = advGridResellers.UpdatedRow["PurchaseDiscount"].TryParseFloat();
                //commonSettings.ExportNotActiveProducts = advGridResellers.UpdatedRow["ExportNotActiveProducts"].TryParseBool();
                //commonSettings.ExportNotAmountProducts = advGridResellers.UpdatedRow["ExportNotAmountProducts"].TryParseBool();
                advancedSettings.RecomendedPriceMargin = advGridResellers.UpdatedRow["RecommendedPriceMargin"].TryParseFloat();

                commonSettings.AdvancedSettings = JsonConvert.SerializeObject(advancedSettings);

                ExportFeedSettingsProvider.SetSettings(exportFeedId, commonSettings);
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

            advGridResellers.DataSource = data;
            advGridResellers.DataBind();


            pageNumberer.PageCount = _paging.PageCount;
            lblFound.Text = _paging.TotalRowsCount.ToString();
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
            {
                var commonSettings = ExportFeedSettingsProvider.GetSettings(Convert.ToInt32(((DataRowView)e.Row.DataItem)["Id"]));
                var advancedSettings = ExportFeedSettingsProvider.ConvertAdvancedSettings<ExportFeedResellerOptions>(commonSettings.AdvancedSettings);

                if (commonSettings == null || string.IsNullOrEmpty(commonSettings.FileName))
                {
                    return;
                }

                ((Label)e.Row.FindControl("lResellerID")).Text = advancedSettings.ResellerCode;
                if (((DataRowView)e.Row.DataItem)["LastExport"] != null)
                {
                    ((Label)e.Row.FindControl("lblFile")).Text = RenderResellerFile(advancedSettings.ResellerCode,
                        commonSettings.FileName, commonSettings.FileExtention, (SQLDataHelper.GetDateTime(((DataRowView)e.Row.DataItem)["LastExport"])).ToShortDateTime());
                }
            }
        }

        protected void btnAddResellers_Click(object sender, EventArgs e)
        {
            advGridResellers.ShowFooter = true;
            advGridResellers.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
            advGridResellers.DataBound += advGridResellers_DataBound;
        }

        void advGridResellers_DataBound(object sender, EventArgs e)
        {
            if (advGridResellers.ShowFooter)
            {
                advGridResellers.FooterRow.FindControl("txtNewName").Focus();
            }
        }

        protected void OnClick(object sender, EventArgs e)
        {
            new Task(ExportFiles).Start();
            Response.Redirect(Request.Url.ToString());
        }

        private void ExportFiles()
        {
            foreach (var exportFeed in ExportFeedService.GetExportFeeds(EExportFeedType.Reseller))
            {                
                var exportFeedSettings = ExportFeedSettingsProvider.GetSettings(exportFeed.Id);
                if (exportFeedSettings == null)
                {
                    continue;
                }

                var type = ReflectionExt.GetTypeByAttributeValue<ExportFeedKeyAttribute>(typeof(BaseExportFeed), atr => atr.Value, exportFeed.Type.ToString());
                var currentExportFeed = (BaseExportFeed)Activator.CreateInstance(type);

                var filePath = exportFeedSettings.FileFullPath;
                var directory = filePath.Substring(0, filePath.LastIndexOf('\\'));

                if (!string.IsNullOrEmpty(directory))
                {
                    FileHelpers.CreateDirectory(directory);
                }

                FileHelpers.DeleteFile(filePath);

                currentExportFeed.Export(exportFeed.Id);

                exportFeed.LastExport = DateTime.Now;
                exportFeed.LastExportFileFullName = exportFeedSettings.FileFullName;// filePath;
                ExportFeedService.UpdateExportFeed(exportFeed);
            }
        }

        protected string RenderResellerFile(string resellerCode, string fileName, string fileExtention, string lastExport)
        {
            var filePath = HostingEnvironment.MapPath("~/" + fileName + "." + fileExtention);
            var tempfilename = HostingEnvironment.MapPath("~/" + fileName + "_temp." + fileExtention);

            if (System.IO.File.Exists(tempfilename))
            {
                return Resources.Resource.Admin_Resellers_Updating;
            }
            else if (System.IO.File.Exists(filePath))
            {
                return string.Format("<a href='{4}/api/resellers/catalog/{0}'>{1}</a> {2}: {3}",
                    resellerCode,
                    Resources.Resource.Admin_Resellers_Downdload,
                    Resources.Resource.Admin_Resellers_Updated,
                    lastExport,
                    SettingsMain.SiteUrl);
            }
            else
            {
                return Resources.Resource.Admin_ExportFeed_NotExports;
            }
        }
    }
}