//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Scheduler;
using AdvantShop.Module.SupplierOfHappiness.Domain;
using Color = System.Drawing.Color;

namespace Advantshop.Module.SupplierOfHappiness
{
    public partial class SupplierOfHappinessSettings : System.Web.UI.UserControl
    {
        protected static List<ListItem> Categories;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            LoadData();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        protected void btnUpdateCategoriesList_Click(object sender, EventArgs e)
        {
            var log = new SupplierOfHappinessLog();
            log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Нажата кнопка 'Обновление списка категорий'");
            SupplierOfHappinessService.UpdateCategoriesList();
        }

        protected void btnSetDefaultCategoriesList_Click(object sender, EventArgs e)
        {
            var log = new SupplierOfHappinessLog();
            log.Write(DateTime.Now.ToString("[dd.MM.yy HH:mm]") + " Нажата кнопка 'Установить категории по умолчанию'");

            SupplierOfHappinessService.SetDefaultCategories();
        }

        protected void lvCategories_OnDataBinding(object sender, ListViewItemEventArgs e)
        {
            var ddlCategories = (DropDownList)e.Item.FindControl("ddlAdvantshopCategory");
            if (ddlCategories != null)
            {
                ddlCategories.DataSource = Categories;
                ddlCategories.DataBind();

                if (e.Item.DataItem != null)
                {
                    var mappingAdvCategoryId = ((SupplierOfHappinessCategory)e.Item.DataItem).AdvCategoryId ?? -1;


                    ddlCategories.SelectedValue =
                        Categories.Any(item => string.Equals(item.Value, mappingAdvCategoryId.ToString()))
                            ? mappingAdvCategoryId.ToString()
                            : "-1";
                }
            }
        }

        private void SaveData()
        {
            if (!Validate())
            {
                return;
            }

            // bug fix
            ModulesRepository.ModuleExecuteNonQuery(
                "Update [Settings].[ModuleSettings] Set ModuleName = 'SupplierOfHappiness' Where ModuleName = 'supplierofhappiness'",
                CommandType.Text);

            ModuleSettingsProvider.SetSettingValue("AmountNulling", ckbAmountNulling.Checked, AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ModuleSettingsProvider.SetSettingValue("DeactivateProducts", ckbDeactivateProducts.Checked, AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ModuleSettingsProvider.SetSettingValue("DeactivateProductsNotInStock", ckbDeactivateProductsNotInStock.Checked, AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ModuleSettingsProvider.SetSettingValue("AutoUpdateActiveFull", ckbAutoUpdateActiveFull.Checked, AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            //ModuleSettingsProvider.SetSettingValue("TimePeriodValueFull", txtTimePeriodValueFull.Text, AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ModuleSettingsProvider.SetSettingValue("AutoUpdateActiveQuick", ckbAutoUpdateActiveQuick.Checked, AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ModuleSettingsProvider.SetSettingValue("TimePeriodValueQuick", txtTimePeriodValueQuick.Text, AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ModuleSettingsProvider.SetSettingValue("Uid", txtUid.Text, AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ModuleSettingsProvider.SetSettingValue("ExtraCharge", Convert.ToInt32(txtExtraCharge.Text), AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ModuleSettingsProvider.SetSettingValue("RetailPriceType", ddlRetailPriceType.SelectedValue, AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ModuleSettingsProvider.SetSettingValue("WholePriceType", ddlWholePriceType.SelectedValue, AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ModuleSettingsProvider.SetSettingValue("UpdateDiscount", ddlUpdateDiscount.SelectedValue, AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);

            foreach (var item in lvCategories.Items)
            {
                var ddl = (DropDownList)item.FindControl("ddlAdvantshopCategory");
                var category = (HiddenField)item.FindControl("hfSoHCategory");
                var subCategory = (HiddenField)item.FindControl("hfSohSubCategory");

                if (category == null || subCategory == null || ddl == null)
                {
                    continue;
                }

                var advCategoryId = Convert.ToInt32(ddl.SelectedValue);

                SupplierOfHappinessRepository.UpdateCategory(
                    new SupplierOfHappinessCategory
                    {
                        Category = category.Value,
                        SubCategory = subCategory.Value,
                        AdvCategoryId = advCategoryId == -1 ? null : (int?)advCategoryId
                    });
            }

            TaskManager.TaskManagerInstance().ManagedTask(TaskSettings.Settings);

            lblMessage.Text = (string)GetLocalResourceObject("YandexMarketImportSettings_ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected bool Validate()
        {
            var valid = true;

            var timePeriodValueFull = 0;
            var timePeriodValueQuick = 0;
            var extraCharge = 0f;

            //valid &= !ckbAutoUpdateActiveFull.Checked || int.TryParse(txtTimePeriodValueFull.Text, out timePeriodValueFull);

            valid &= !ckbAutoUpdateActiveQuick.Checked ||
                     int.TryParse(txtTimePeriodValueQuick.Text, out timePeriodValueQuick);

            valid &= float.TryParse(txtExtraCharge.Text, out extraCharge) && extraCharge > -100 && extraCharge <= 1000;

            if (!valid)
            {
                lblMessage.Text = (string)GetLocalResourceObject("YandexMarketImportSettings_WrongData");
                lblMessage.ForeColor = Color.Red;
                lblMessage.Visible = true;
            }

            if ((ckbAutoUpdateActiveFull.Checked && timePeriodValueFull > 23) || (ckbAutoUpdateActiveQuick.Checked && timePeriodValueQuick > 23))
            {
                lblMessage.Text = (string)GetLocalResourceObject("YandexMarketImportSettings_WrongPeriod");
                lblMessage.ForeColor = Color.Red;
                lblMessage.Visible = true;
                valid = false;
            }

            return valid;
        }

        private void LoadData()
        {
            ckbAmountNulling.Checked = ModuleSettingsProvider.GetSettingValue<bool>("AmountNulling", AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ckbDeactivateProducts.Checked = ModuleSettingsProvider.GetSettingValue<bool>("DeactivateProducts", AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ckbDeactivateProductsNotInStock.Checked = ModuleSettingsProvider.GetSettingValue<bool>("DeactivateProductsNotInStock", AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ckbAutoUpdateActiveFull.Checked = ModuleSettingsProvider.GetSettingValue<bool>("AutoUpdateActiveFull", AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            //ddlTimePeriodFull.SelectedValue = ModuleSettingsProvider.GetSettingValue<string>("TimePeriodFull", AdvantShop.Modules.SupplierOfHappiness.ModuleID);
            //txtTimePeriodValueFull.Text = ModuleSettingsProvider.GetSettingValue<string>("TimePeriodValueFull", AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);

            ckbAutoUpdateActiveQuick.Checked = ModuleSettingsProvider.GetSettingValue<bool>("AutoUpdateActiveQuick", AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            //ddlTimePeriodQuick.SelectedValue = ModuleSettingsProvider.GetSettingValue<string>("TimePeriodQuick", AdvantShop.Modules.SupplierOfHappiness.ModuleID);
            txtTimePeriodValueQuick.Text = ModuleSettingsProvider.GetSettingValue<string>("TimePeriodValueQuick", AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);

            ddlRetailPriceType.SelectedValue = ModuleSettingsProvider.GetSettingValue<string>("RetailPriceType", AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);
            ddlWholePriceType.SelectedValue = ModuleSettingsProvider.GetSettingValue<string>("WholePriceType", AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);

            txtUid.Text = ModuleSettingsProvider.GetSettingValue<string>("Uid", AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);

            txtExtraCharge.Text = ModuleSettingsProvider.GetSettingValue<string>("ExtraCharge", AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID) ?? "0";

            ddlUpdateDiscount.SelectedValue = ModuleSettingsProvider.GetSettingValue<string>("UpdateDiscount", AdvantShop.Module.SupplierOfHappiness.SupplierOfHappiness.ModuleID);

            var advantShopCategories = CategoryService.GetCategories();
            Categories = new List<ListItem>() { new ListItem("Не выбрана", "-1") };

            LoadAllCategories(advantShopCategories, Categories, 0, "");

            lvCategories.DataSource = SupplierOfHappinessRepository.GetCategories();
            lvCategories.DataBind();
        }

        private void LoadAllCategories(List<Category> categories, List<ListItem> list, int categoryId, string offset)
        {
            foreach (var category in categories.Where(c => c.ParentCategoryId == categoryId).OrderBy(c => c.SortOrder).ToList())
            {
                list.Add(new ListItem(HttpUtility.HtmlDecode(offset + category.Name), category.CategoryId.ToString()));

                if (categories.Any(c => c.ParentCategoryId == category.CategoryId))
                {
                    LoadAllCategories(categories, list, category.CategoryId, offset + "&nbsp;&nbsp;");
                }
            }
        }
    }
}