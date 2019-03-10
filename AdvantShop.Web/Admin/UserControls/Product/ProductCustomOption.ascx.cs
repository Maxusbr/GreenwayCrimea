using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin.UserControls.Products
{
    public partial class ProductCustomOption : UserControl
    {
        private bool _valid = true;
        private List<CustomOption> _customOptions;

        public int ProductId { set; get; }
        public bool ShowSaveButton { set; get; }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProductId == 0)
                return;

            btnSave.Visible = ShowSaveButton;
            trHead.Visible = trHelp.Visible = !ShowSaveButton;

            if (!IsPostBack)
                _customOptions = CustomOptionsService.GetCustomOptionsByProductId(ProductId);
            else
                _customOptions = (List<CustomOption>)(ViewState["CustomOptions"]);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
                UpdateCustomOptions();
        }

        public bool SaveCustomOption()
        {
            LoadCustomOptions();
            if (_valid)
            {
                CustomOptionsService.SubmitCustomOptionsWithSameProductId(ProductId, _customOptions);
                _customOptions = CustomOptionsService.GetCustomOptionsByProductId(ProductId);
            }

            UpdateCustomOptions();

            return _valid;
        }

        protected void rCustomOptions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (!_valid)
            {
                ((e.Item.FindControl("pInvalidTitle"))).Visible = string.IsNullOrEmpty(((TextBox)(e.Item.FindControl("txtTitle"))).Text);

                int i;
                ((e.Item.FindControl("pInvalidSortOrder"))).Visible = !int.TryParse(((TextBox)(e.Item.FindControl("txtSortOrder"))).Text, out i);
            }

            var itp = ((CustomOption)e.Item.DataItem).InputType;

            ((DropDownList)(e.Item.FindControl("ddlInputType"))).SelectedValue = ((int)itp).ToString(CultureInfo.InvariantCulture);

            if (((CustomOption)e.Item.DataItem).Options == null || ((CustomOption)e.Item.DataItem).Options.Count == 0)
            {
                return;
            }

            var pt = ((CustomOption)e.Item.DataItem).Options[0].PriceType;
            ((DropDownList)(e.Item.FindControl("ddlPriceType"))).SelectedValue = pt.ToString();
        }

        protected void ddlInputType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCustomOptions();
            var itp = (CustomOptionInputType)(((DropDownList)sender).SelectedValue.TryParseInt());

            if (itp == CustomOptionInputType.CheckBox || itp == CustomOptionInputType.TextBoxMultiLine || itp == CustomOptionInputType.TextBoxSingleLine)
            {
                var idx = ((RepeaterItem)(((DropDownList)sender).Parent)).ItemIndex;
                _customOptions[idx].Options.Clear();
                var opt = new OptionItem { OptionId = -1, BasePrice = 0, SortOrder = 10, Title = " " };
                _customOptions[idx].Options.Add(opt);
            }
            UpdateCustomOptions();
        }

        private void LoadCustomOptions()
        {
            var customOptions = new List<CustomOption>();

            foreach (RepeaterItem item in rCustomOptions.Items)
            {
                var customOption = new CustomOption
                {
                    CustomOptionsId = ((HiddenField)(item.FindControl("hfId"))).Value.TryParseInt(),
                    ProductId = ((HiddenField)(item.FindControl("hfProductId"))).Value.TryParseInt()
                };

                if (string.IsNullOrEmpty(((TextBox)(item.FindControl("txtTitle"))).Text))
                {
                    _valid = false;
                }

                int i;
                if (!int.TryParse(((TextBox)(item.FindControl("txtSortOrder"))).Text, out i))
                {
                    _valid = false;
                }

                customOption.Title = ((TextBox)(item.FindControl("txtTitle"))).Text;
                customOption.InputType = (CustomOptionInputType)((((DropDownList)(item.FindControl("ddlInputType"))).SelectedValue.TryParseInt()));
                customOption.IsRequired = ((CheckBox)(item.FindControl("cbIsRequired"))).Checked;
                try
                {
                    customOption.SortOrder = SQLDataHelper.GetInt(((TextBox)(item.FindControl("txtSortOrder"))).Text);
                }
                catch (Exception)
                {
                    customOption.SetFieldToNull(CustomOptionField.SortOrder);
                }

                customOption.Options = new List<OptionItem>();

                if (customOption.InputType == CustomOptionInputType.CheckBox)
                {
                    var opt = new OptionItem { Title = string.Empty };
                    try
                    {
                        opt.BasePrice = SQLDataHelper.GetFloat(((TextBox)(item.FindControl("txtBasePrice"))).Text);
                    }
                    catch (Exception)
                    {
                        opt.SetFieldToNull(OptionField.PriceBc);
                        _valid = false;
                    }

                    opt.PriceType = OptionPriceType.Fixed;
                    if (Enum.IsDefined(typeof(OptionPriceType), ((DropDownList)(item.FindControl("ddlPriceType"))).SelectedValue))
                        opt.PriceType = (OptionPriceType)Enum.Parse(typeof(OptionPriceType), ((DropDownList)(item.FindControl("ddlPriceType"))).SelectedValue, true);

                    customOption.Options.Add(opt);
                }
                else
                {
                    foreach (GridViewRow row in ((GridView)(item.FindControl("grid"))).Rows)
                    {
                        var opt = new OptionItem
                        {
                            OptionId = ((Label)(row.Cells[0].FindControl("lId"))).Text.TryParseInt()
                        };
                        if (string.IsNullOrEmpty(((TextBox)(row.Cells[1].FindControl("txtTitle"))).Text) &&
                            !(customOption.InputType == CustomOptionInputType.CheckBox ||
                              customOption.InputType == CustomOptionInputType.TextBoxMultiLine ||
                              customOption.InputType == CustomOptionInputType.TextBoxSingleLine))
                        {
                            _valid = false;
                        }
                        opt.Title = ((TextBox)(row.Cells[1].FindControl("txtTitle"))).Text;

                        try
                        {
                            opt.BasePrice = SQLDataHelper.GetFloat(((TextBox)(row.Cells[2].FindControl("txtBasePrice"))).Text);
                        }
                        catch (Exception)
                        {
                            opt.SetFieldToNull(OptionField.PriceBc);
                            _valid = false;
                        }

                        opt.PriceType = OptionPriceType.Fixed;
                        if (Enum.IsDefined(typeof(OptionPriceType), ((DropDownList)(row.Cells[3].FindControl("ddlPriceType"))).SelectedValue))
                            opt.PriceType = (OptionPriceType)Enum.Parse(typeof(OptionPriceType), ((DropDownList)(row.Cells[3].FindControl("ddlPriceType"))).SelectedValue, true);
                        try
                        {
                            opt.SortOrder = int.Parse(((TextBox)(row.Cells[4].FindControl("txtSortOrder"))).Text);
                        }
                        catch (Exception)
                        {
                            opt.SetFieldToNull(OptionField.SortOrder);
                            _valid = false;
                        }
                        customOption.Options.Add(opt);
                    }
                }
                customOptions.Add(customOption);
            }

            _customOptions = customOptions;
        }

        protected void rCustomOptions_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            LoadCustomOptions();

            if (e.CommandName.Equals("AddNewOption"))
            {
                var copt = _customOptions[e.Item.ItemIndex];
                var maxSort = copt.Options.Select(x => x.SortOrder).DefaultIfEmpty().Max();
                var opt = new OptionItem { OptionId = -1, BasePrice = 0, SortOrder = maxSort + 10 };
                copt.Options.Add(opt);

            }
            else if (e.CommandName.Equals("DeleteCustomOptions"))
            {
                _customOptions.RemoveAt(e.Item.ItemIndex);
            }

            UpdateCustomOptions();
        }

        protected void btnAddCustomOption_Click(object sender, EventArgs e)
        {
            LoadCustomOptions();
            var maxSort = _customOptions.Select(x => x.SortOrder).DefaultIfEmpty().Max();
            var copt = new CustomOption(true) { CustomOptionsId = -1, ProductId = ProductId, SortOrder = maxSort + 10 };

            var opt = new OptionItem { OptionId = -1, BasePrice = 0, SortOrder = 10 };

            copt.Options = new List<OptionItem> { opt };

            _customOptions.Add(copt);
            UpdateCustomOptions();
        }

        private void UpdateCustomOptions()
        {
            rCustomOptions.DataSource = _customOptions;
            rCustomOptions.DataBind();
            ViewState["CustomOptions"] = _customOptions;
        }

        protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            LoadCustomOptions();
            int id = ((HiddenField)(((GridView)sender).Parent.FindControl("hfId"))).Value.TryParseInt();
            foreach (var copt in _customOptions)
            {
                if (copt.CustomOptionsId != id) continue;
                if (copt.Options.Count > 1 && e.RowIndex >= 0)
                {
                    copt.Options.RemoveAt(e.RowIndex);
                }
            }
            UpdateCustomOptions();
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            if (!_valid)
            {
                if (string.IsNullOrEmpty(((TextBox)(e.Row.Cells[1].FindControl("txtTitle"))).Text))
                {
                    var p = new Panel { CssClass = "validation-advice" };
                    p.Controls.Add(new LiteralControl(Resource.Admin_m_Product_RequiredField));
                    e.Row.Cells[1].Controls.AddAt(0, p);
                }
                int i;
                if (!int.TryParse(((TextBox)(e.Row.Cells[2].FindControl("txtBasePrice"))).Text, out i))
                {
                    var p = new Panel { CssClass = "validation-advice" };
                    p.Controls.Add(new LiteralControl(Resource.Admin_Product_EnterValidNumber));
                    e.Row.Cells[2].Controls.AddAt(0, p);
                }

                if (!int.TryParse(((TextBox)(e.Row.Cells[4].FindControl("txtSortOrder"))).Text, out i))
                {
                    var p = new Panel { CssClass = "validation-advice" };
                    p.Controls.Add(new LiteralControl(Resource.Admin_Product_EnterValidNumber));
                    e.Row.Cells[4].Controls.AddAt(0, p);
                }
            }

            ((DropDownList)(e.Row.Cells[2].FindControl("ddlPriceType"))).SelectedValue =
                ((OptionItem)e.Row.DataItem).PriceType.ToString();
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            SaveCustomOption();
        }
    }
}