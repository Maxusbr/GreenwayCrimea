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
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using AdvantShop.Repository.Currencies;
using Resources;
using AdvantShop.Saas;

namespace Admin
{
    public enum ECurrencyRound
    {
        [Localize("Admin.Currencies.ECurrencyRound.None")]
        None = -10,

        [Localize("Admin.Currencies.ECurrencyRound.UpToSecondPlace")]
        UpToSecondPlace = -2,

        [Localize("Admin.Currencies.ECurrencyRound.UpToFirstPlace")]
        UpToFirstPlace = -1,

        [Localize("Admin.Currencies.ECurrencyRound.UpToWhole")]
        UpToWhole = 0,

        [Localize("Admin.Currencies.ECurrencyRound.UpToTens")]
        UpToTens = 1,

        [Localize("Admin.Currencies.ECurrencyRound.UpToHundreds")]
        UpToHundreds = 2,

        [Localize("Admin.Currencies.ECurrencyRound.UpToThousands")]
        UpToThousands = 3
    }

    public partial class Currencies : AdvantShopAdminPage
    {
        private bool _inverseSelection;
        private SqlPaging _paging;
        private InSetFieldFilter _selectionFilter;

        public Currencies()
        {
            _inverseSelection = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Currencies_Header));

            if (!IsPostBack)
            {
                chkAutoUpdateEnabled.Checked = SettingsMain.EnableAutoUpdateCurrencies;

                _paging = new SqlPaging { TableName = "[Catalog].[Currency]", ItemsPerPage = 10 };

                var f = new Field { Name = "CurrencyID as ID", IsDistinct = true, Filter = _selectionFilter };
                _paging.AddField(f);

                f = new Field { Name = "Name", Sorting = SortDirection.Ascending };
                _paging.AddField(f);

                f = new Field { Name = "Code" };
                _paging.AddField(f);

                f = new Field { Name = "CurrencyValue" };
                _paging.AddField(f);

                f = new Field { Name = "CurrencyISO3" };
                _paging.AddField(f);

                f = new Field { Name = "CurrencyNumIso3" };
                _paging.AddField(f);

                f = new Field { Name = "IsCodeBefore" };
                _paging.AddField(f);

                //f = new Field { Name = "PriceFormat" };
                //_paging.AddField(f);

                f = new Field { Name = "EnablePriceRounding" };
                _paging.AddField(f);

                f = new Field { Name = "RoundNumbers" };
                _paging.AddField(f);

                grid.ChangeHeaderImageUrl("arrowName", "images/arrowup.gif");

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
                    //_InverseSelection = If(ids(0) = -1, True, False)
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
                    else
                    {
                        _selectionFilter = null;
                    }
                }
                _paging.Fields["ID"].Filter = _selectionFilter;
            }
            else
            {
                _paging.Fields["ID"].Filter = _selectionFilter;
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

            //----CurrencyValue filter
            if (!string.IsNullOrEmpty(txtValue.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtValue.Text, ParamName = "@CurrencyValue" };
                _paging.Fields["CurrencyValue"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["CurrencyValue"].Filter = null;
            }


            //---ISO3 filter
            if (!string.IsNullOrEmpty(txtISO3.Text))
            {
                var nfilter = new CompareFieldFilter { Expression = txtISO3.Text, ParamName = "@CurrencyISO3" };
                _paging.Fields["CurrencyISO3"].Filter = nfilter;
            }
            else
            {
                _paging.Fields["CurrencyISO3"].Filter = null;
            }

            //----IsCodeBefore filter
            if (ddBefore.SelectedIndex != 0)
            {
                var beforeFilter = new EqualFieldFilter { ParamName = "@IsCodeBefore" };
                if (ddBefore.SelectedIndex == 1)
                {
                    beforeFilter.Value = "1";
                }
                if (ddBefore.SelectedIndex == 2)
                {
                    beforeFilter.Value = "0";
                }
                _paging.Fields["IsCodeBefore"].Filter = beforeFilter;
            }
            else
            {
                _paging.Fields["IsCodeBefore"].Filter = null;
            }

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
                var currency = CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);

                if (!_inverseSelection)
                {
                    foreach (var id in _selectionFilter.Values)
                    {
                        if (currency != null && currency.CurrencyId != SQLDataHelper.GetInt(id))
                        {
                            CurrencyService.DeleteCurrency(SQLDataHelper.GetInt(id));
                        }
                    }
                }
                else
                {
                    var itemsIds = _paging.ItemsIds<int>("CurrencyID as ID");
                    foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                    {
                        if (currency != null && currency.CurrencyId != id)
                        {
                            CurrencyService.DeleteCurrency(id);
                        }
                    }
                }
            }
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCurrency")
            {
                var currency = CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);
                if (currency.CurrencyId != SQLDataHelper.GetInt(e.CommandArgument))
                {
                    CurrencyService.DeleteCurrency(SQLDataHelper.GetInt(e.CommandArgument));
                }
            }
            if (e.CommandName == "AddCurrency")
            {
                GridViewRow footer = grid.FooterRow;
                float temp;
                float.TryParse(((TextBox)footer.FindControl("txtNewCurrencyValue")).Text, out temp);
                if (
                    temp == 0 || string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewName")).Text)
                    || string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewCode")).Text)
                    || string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewCurrencyISO3")).Text)
                    //|| string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewPriceFormat")).Text)
                    )
                {
                    grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                    return;
                }

                var cur = new Currency
                {
                    Name = ((TextBox)footer.FindControl("txtNewName")).Text,
                    Symbol = ((TextBox)footer.FindControl("txtNewCode")).Text,
                    Rate = temp,
                    Iso3 = ((TextBox)footer.FindControl("txtNewCurrencyISO3")).Text,
                    NumIso3 = ((TextBox)footer.FindControl("txtNewCurrencyNumIso3")).Text.TryParseInt(),
                    IsCodeBefore = ((CheckBox)footer.FindControl("checkNewIsCodeBefore")).Checked
                };

                var tempEnum = ((DropDownList)footer.FindControl("ddlNewPriceRounding")).SelectedValue.TryParseEnum<ECurrencyRound>();
                if (tempEnum == ECurrencyRound.None)
                {
                    cur.EnablePriceRounding = false;
                    cur.RoundNumbers = 1;
                }
                else
                {
                    cur.EnablePriceRounding = true;
                    cur.RoundNumbers = (float)Math.Pow(10, (int)tempEnum);
                }

                CurrencyService.InsertCurrency(cur);
                grid.ShowFooter = false;
            }
            if (e.CommandName == "CancelAdd")
            {
                grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
                grid.ShowFooter = false;
            }
        }

        protected void grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            var arrows = new Dictionary<string, string>
            {
                {"Name", "arrowName"},
                {"Code", "arrowCode"},
                {"CurrencyValue", "arrowCurrencyValue"},
                {"CurrencyISO3", "arrowCurrencyISO3"},
                {"CurrencyNumIso3", "arrowCurrencyNumIso3"},
                {"IsCodeBefore", "arrowIsCodeBefore"},
                {"EnablePriceRounding", "arrowEnablePriceRounding"},
                {"RoundNumbers", "arrowRoundNumbers"},
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
            btnUpdateCurrencies.Visible = CurrencyService.GetAllCurrencies(true).Find(c => c.Iso3 == "RUB" && c.Rate == 1) != null;

            if (grid.UpdatedRow != null && grid.UpdatedRow["CurrencyValue"].TryParseDecimal() > 0)
            {
                var id = SQLDataHelper.GetInt(grid.UpdatedRow["ID"]);
                var upCur = CurrencyService.GetCurrency(id);
                //var roundNumbers = grid.UpdatedRow["RoundNumbers"].TryParseFloat(1);

                upCur.Name = grid.UpdatedRow["Name"];
                upCur.Symbol = grid.UpdatedRow["Code"];
                upCur.Rate = grid.UpdatedRow["CurrencyValue"].TryParseFloat();
                upCur.Iso3 = grid.UpdatedRow["CurrencyISO3"];
                upCur.NumIso3 = grid.UpdatedRow["CurrencyNumIso3"].TryParseInt();
                upCur.IsCodeBefore = SQLDataHelper.GetBoolean(grid.UpdatedRow["IsCodeBefore"]);

                var tempEnum = grid.UpdatedRow["PriceRounding"].TryParseEnum<ECurrencyRound>();
                if (tempEnum == ECurrencyRound.None)
                {
                    upCur.EnablePriceRounding = false;
                    upCur.RoundNumbers = 1;
                }
                else
                {
                    upCur.EnablePriceRounding = true;
                    upCur.RoundNumbers = (float)Math.Pow(10, (int)tempEnum);
                }
                //upCur.EnablePriceRounding = grid.UpdatedRow["EnablePriceRounding"].TryParseBool();
                //upCur.RoundNumbers = roundNumbers <= 0 ? 1 : roundNumbers;

                CurrencyService.UpdateCurrency(upCur);
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

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var ddlPriceRounding = ((DropDownList)e.Row.FindControl("ddlPriceRounding"));

                foreach (var item in Enum.GetValues(typeof(ECurrencyRound)).OfType<ECurrencyRound>().OrderBy(x => (int)x))
                {
                    ddlPriceRounding.Items.Add(new ListItem(item.Localize(), item.ToString()));
                }
                var enablePriceRounding = SQLDataHelper.GetBoolean(((DataRowView)e.Row.DataItem)["EnablePriceRounding"]);
                var roundNumbers = SQLDataHelper.GetFloat(((DataRowView)e.Row.DataItem)["RoundNumbers"]);

                if (!enablePriceRounding)
                {
                    ddlPriceRounding.SelectedValue = ECurrencyRound.None.ToString();
                }
                else
                {
                    var temp = (int)Math.Round(Math.Log10(roundNumbers));
                    var tempEnum = (ECurrencyRound)temp;
                    ddlPriceRounding.SelectedValue = tempEnum.ToString();
                }

                //((DropDownList)e.Row.FindControl("ddlPriceRounding")).SelectedValue =
                //    ((DataRowView)e.Row.DataItem)["Type"].ToString();
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                var ddlPriceRounding = ((DropDownList)e.Row.FindControl("ddlNewPriceRounding"));

                foreach (var item in Enum.GetValues(typeof(ECurrencyRound)).OfType<ECurrencyRound>().OrderBy(x => (int)x))
                {
                    ddlPriceRounding.Items.Add(new ListItem(item.Localize(), item.ToString()));
                }
            }
        }

        protected void btnAddCurrency_Click(object sender, EventArgs e)
        {
            grid.ShowFooter = true;
            grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
            grid.DataBound += grid_DataBound;
        }

        protected void btnUpdateCurrencies_Click(object sender, EventArgs e)
        {
            CurrencyService.UpdateCurrenciesFromCentralBank();
        }

        private void grid_DataBound(object sender, EventArgs e)
        {
            if (grid.ShowFooter)
            {
                grid.FooterRow.FindControl("txtNewName").Focus();
            }
        }

        protected void chkAutoUpdateEnabled_CheckedChanged(object sender, EventArgs e)
        {
            SettingsMain.EnableAutoUpdateCurrencies = chkAutoUpdateEnabled.Checked;
        }
    }
}