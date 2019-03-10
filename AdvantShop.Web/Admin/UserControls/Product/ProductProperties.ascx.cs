using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using Newtonsoft.Json;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Debug = AdvantShop.Diagnostics.Debug;

namespace Admin.UserControls.Products
{
    public partial class ProductProperties : UserControl
    {
        private List<PropertyValue> _properties;
        protected List<PropertyValue> Properties
        {
            get { return _properties ?? (_properties = PropertyService.GetPropertyValuesByProductId(ProductId)); }
        }

        public int ProductId { set; get; }
        public int CategoryId { get; set; }

        protected string RenderingProperties;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillcboPropertyName();
            }

            cboPropertyName.CssClass = RadioButtonExistProperty.Checked ? string.Empty : "disabled";
            txtNewPropertyName.CssClass = RadioButtonExistProperty.Checked ? "disabled" : string.Empty;
            RadioButtonNewPropertyValue.Checked = RadioButtonAddNewProperty.Checked || RadioButtonNewPropertyValue.Checked;
            cboPropertyValue.Enabled = !RadioButtonAddNewProperty.Checked;
            RadioButtonExistPropertyValue.Enabled = !RadioButtonAddNewProperty.Checked;
            RadioButtonExistPropertyValue.Checked = RadioButtonExistPropertyValue.Checked && !RadioButtonAddNewProperty.Checked;

            cboPropertyValue.CssClass = RadioButtonExistPropertyValue.Checked ? string.Empty : "disabled";
            txtNewPropertyValue.CssClass = RadioButtonExistPropertyValue.Checked ? "disabled" : string.Empty;

            hfpropproductId.Value = ProductId.ToString();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadDataGrid();
        }

        private void LoadDataGrid()
        {
            FillcboPropertyValue();
            _properties = PropertyService.GetPropertyValuesByProductId(ProductId);

            var propertyGroups = PropertyGroupService.GetGroupsByCategory(CategoryId);

            var groups = propertyGroups.Select(g => new UsedPropertyGroupView()
            {
                PropertyGroupId = g.PropertyGroupId,
                GroupName = g.GroupName,
                Properties =
                    propertyGroups.Where(p => p.PropertyGroupId == g.PropertyGroupId).Select(p => new Property
                    {
                        PropertyId = p.PropertyId,
                        Name = p.PropertyName,
                        Type = p.Type
                    }).ToList()
            }).Distinct(new PropertyGroupComparer());

            var propInGroups = new List<int>();
            var result = new StringBuilder();
            var cat = CategoryService.GetCategory(CategoryId);
            var catName = cat != null ? cat.Name : "";
            if (_properties.Any(x => groups.Any(y => y.PropertyGroupId == x.Property.GroupId)))
            {
                result.AppendFormat("<div class='group-header' >{0}</div>", string.Format(Resource.Admin_Properties_GroupedProperties, catName));
            }

            foreach (var group in groups)
            {
                if (group.Properties.Count > 0)
                {
                    result.AppendFormat("<div class='group-header' data-group='{0}'>{1}</div>", group.PropertyGroupId, group.GroupName);

                    foreach (var property in group.Properties)
                    {
                        result.Append(RenderPropertyItem(property, _properties.Where(p => p.PropertyId == property.PropertyId).ToList()));

                        propInGroups.Add(property.PropertyId);
                    }
                }
            }

            var propsWithoutCategory = _properties.Where(p => !propInGroups.Contains(p.PropertyId)).ToList();

            if (propsWithoutCategory.Any())
            {
                result.AppendFormat("<div class='group-header'>{0}</div>", string.Format(Resource.Admin_Properties_UngroupedProperties, catName));
            }

            if (propsWithoutCategory.Count > 0)
            {
                var propIds = new List<int>();
                var groupsNoCat = propsWithoutCategory.Select(x => x.Property.Group).Where(x => x != null).ToList();
                foreach (var g in groupsNoCat)
                {
                    result.AppendFormat("<div class='group-header' data-group='{0}'>{1}</div>", g.PropertyGroupId, g.Name);
                    var tempHaveGroup = propsWithoutCategory.Where(x => x.Property.GroupId == g.PropertyGroupId).ToList();
                    foreach (var propWitoutGroup in tempHaveGroup)
                    {
                        if (propIds.Contains(propWitoutGroup.PropertyId)) continue;
                        result.Append(RenderPropertyItem(propWitoutGroup.Property, _properties.Where(p => p.PropertyId == propWitoutGroup.Property.PropertyId).ToList()));
                        propIds.Add(propWitoutGroup.PropertyId);
                    }
                }
                var tempNoHaveGroup = propsWithoutCategory.Where(x => x.Property.Group == null).ToList();
                foreach (var propWitoutGroup in tempNoHaveGroup)
                {
                    if (propIds.Contains(propWitoutGroup.PropertyId)) continue;
                    result.Append(RenderPropertyItem(propWitoutGroup.Property, _properties.Where(p => p.PropertyId == propWitoutGroup.Property.PropertyId).ToList()));
                    propIds.Add(propWitoutGroup.PropertyId);
                }
            }

            hfpropselected.Value = string.Join(",", _properties.Select(x => x.PropertyValueId));

            RenderingProperties = result.ToString();
        }

        private string RenderPropertyItem(Property property, List<PropertyValue> selectedValues)
        {
            var result = new StringBuilder();

            result.AppendFormat("<div class='property-item'> <div class='property-item-title'>{0}</div> <div class='property-item-values' data-property='{1}'>",
                                    property.Name, property.PropertyId);

            switch (property.Type)
            {
                case (int)PropertyType.Checkbox:
                case (int)PropertyType.Selectbox:
                    foreach (var propertyValue in property.ValuesList.Take(10))
                    {
                        result.AppendFormat(
                            "<div class='propval-item'> <label for='propval_{0}'> <input type='checkbox' name='propval_{3}' id='propval_{0}' {2} value='{0}' /> {1} </div>",
                            propertyValue.PropertyValueId, propertyValue.Value,
                            (selectedValues.Find(p => p.PropertyValueId == propertyValue.PropertyValueId) != null ? "checked" : ""),
                            property.PropertyId);
                    }

                    if (property.ValuesList.Count > 10)
                    {
                        result.AppendFormat("<div class=\"expander-lnk\">{0}</div>", Resource.Admin_Properties_Expande);
                    }

                    result.AppendFormat("<div class='propval-newitem'> <input type='text' class='valtype-text' placeholder='{0}' />  </div>",
                                            Resource.Admin_Product_ProductProperties_EnterOther);
                    break;

                case (int)PropertyType.Range:
                    foreach (var propertyValue in property.ValuesList.Take(10))
                    {
                        result.AppendFormat(
                            "<div class='propval-item'> <label for='propval_{0}'> <input type='checkbox' name='propval_{3}' id='propval_{0}' {2} value='{0}' /> {1} </div>",
                            propertyValue.PropertyValueId, propertyValue.Value,
                            (selectedValues.Find(p => p.PropertyValueId == propertyValue.PropertyValueId) != null ? "checked" : ""),
                            property.PropertyId);
                    }

                    if (property.ValuesList.Count > 10)
                    {
                        result.AppendFormat("<div class=\"expander-lnk\">{0}</div>", Resource.Admin_Properties_Expande);
                    }

                    result.AppendFormat("<div class='propval-newitem'> <input type='text' class='valtype-number' placeholder='{0}' />  </div>",
                                            Resource.Admin_Product_ProductProperties_EnterOther);
                    break;
            }

            result.Append("</div></div>");
            return result.ToString();
        }

        public void SaveProperties()
        {
            try
            {
                var propValues = new List<int>();
                var temp = hfpropresult.Value.Split(',');
                foreach (var elem in temp)
                {
                    int value;
                    if (int.TryParse(elem, out value))
                        propValues.Add(value);
                }

                var currentPropValues = PropertyService.GetPropertyValuesByProductId(ProductId).Select(p => p.PropertyValueId).ToList();

                var newAddedPropValues = hfnewpropresult.Value.Trim();

                if (propValues == currentPropValues && newAddedPropValues.IsNullOrEmpty())
                    return;

                var propValuesToAdd = propValues.Where(v => !currentPropValues.Contains(v));
                var propValuesToDelete = currentPropValues.Where(v => !propValues.Contains(v));

                foreach (var propValueId in propValuesToAdd)
                {
                    PropertyService.AddProductProperyValue(propValueId, ProductId);
                }

                foreach (var propValueId in propValuesToDelete)
                {
                    PropertyService.DeleteProductPropertyValue(ProductId, propValueId);
                }


                var newPropValues = JsonConvert.DeserializeObject<List<PropertyValue>>(hfnewpropresult.Value);
                foreach (var newPropValue in newPropValues)
                {
                    AddNewPropertyValue(newPropValue.PropertyId, newPropValue.Value);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }


        /// <summary>
        /// Ќажали на кнопку добавить, провер€ем добавить новое свойство или выбрать из существующих
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        protected void btnAddNewExistProperty_Click(object sender, EventArgs e)
        {
            txtNewPropertyName.Text = txtNewPropertyName.Text.Trim();
            txtNewPropertyValue.Text = txtNewPropertyValue.Text.Trim();

            try
            {

                if (RadioButtonExistProperty.Checked)
                {
                    if (RadioButtonExistPropertyValue.Checked)
                    {
                        if (!string.IsNullOrEmpty(cboPropertyValue.SelectedValue))
                            AddExistPropertyValue(SQLDataHelper.GetInt(cboPropertyValue.SelectedValue));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txtNewPropertyValue.Text))
                        {
                            AddNewPropertyValue(SQLDataHelper.GetInt(cboPropertyName.SelectedValue), txtNewPropertyValue.Text);
                            //FillcboPropertyValue();
                        }
                        else
                        {
                            lblValueRequired.Visible = true;
                            lblValueRequired.Text = Resource.Admin_m_Product_RequiredField;
                        }
                    }
                }
                else if (RadioButtonAddNewProperty.Checked)
                {
                    if (!string.IsNullOrEmpty(txtNewPropertyName.Text) && !string.IsNullOrEmpty(txtNewPropertyValue.Text))
                    {
                        AddNewPropertyWithValue(txtNewPropertyName.Text, txtNewPropertyValue.Text);
                        FillcboPropertyName();
                    }
                }


            }
            catch  //(Exception ex)
            {
                Msg(Resource.Admin_m_Product_ErrorAddProperty); //ex.Message);
            }


            //FillcboPropertyValue();
            LoadDataGrid();

            txtNewPropertyValue.Text = string.Empty;
            txtNewPropertyName.Text = string.Empty;
        }

        /// <summary>
        ///  ƒобавл€ет продукту свойство с существующим значением
        /// </summary>
        /// <param name="propValId"></param>
        /// <param name="sortOrder"></param>
        /// <remarks></remarks>
        protected void AddExistPropertyValue(int propValId)
        {
            PropertyService.AddProductProperyValue(propValId, ProductId);
        }

        /// <summary>
        ///  ƒобавл€ет продукту новое значение свойства
        /// </summary>
        /// <param name="propVal"></param>
        /// <param name="propId"></param>
        /// <remarks></remarks>
        protected void AddNewPropertyValue(int propId, string propVal)
        {
            var temp = PropertyService.AddPropertyValue(new PropertyValue { PropertyId = propId, Value = propVal });
            PropertyService.AddProductProperyValue(temp, ProductId);
        }

        protected void Msg(string message)
        {
            lblErrorAddProp.Visible = true;
            lblErrorAddProp.Text = message;
        }

        /// <summary>
        /// ƒобавл€ет  продукту новое  несуществующее свойство
        /// </summary>
        /// <remarks></remarks>
        protected void AddNewPropertyWithValue(string propName, string propVal)
        {
            if (ProductId < 1) return;
            try
            {
                var property = new Property
                {
                    Name = propName,
                    UseInFilter = true,
                    UseInDetails = true,
                    Type = 1
                };
                var propertyId = PropertyService.AddProperty(property);
                var propertyValue = new PropertyValue
                {
                    PropertyId = propertyId,
                    Value = propVal,
                };
                var idPropertyValue = PropertyService.AddPropertyValue(propertyValue);
                PropertyService.AddProductProperyValue(idPropertyValue, ProductId);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                Msg("Unable to add property with value");
            }
            cboPropertyName.DataBind();
        }

        protected void cboPropertyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillcboPropertyValue();
        }

        private void FillcboPropertyName()
        {
            cboPropertyName.Items.Clear();
            foreach (Property prop in PropertyService.GetAllProperties())
            {
                var name = prop.Name + (!string.IsNullOrEmpty(prop.Description) ? " (" + prop.Description + ")" : "");
                cboPropertyName.Items.Add(new ListItem(name, prop.PropertyId.ToString(CultureInfo.InvariantCulture)));
            }
            cboPropertyName.DataBind();
            FillcboPropertyValue();
        }

        private void FillcboPropertyValue()
        {
            cboPropertyValue.Items.Clear();
            if (string.IsNullOrEmpty(cboPropertyName.SelectedValue)) return;

            var propList =
                PropertyService.GetValuesByPropertyId(SQLDataHelper.GetInt(cboPropertyName.SelectedValue));

            foreach (var propVal in propList.Except(Properties, (prop, prodProp) => prop.PropertyValueId == prodProp.PropertyValueId).OrderBy(p => p.Value))
            {
                cboPropertyValue.Items.Add(new ListItem(propVal.Value, propVal.PropertyValueId.ToString(CultureInfo.InvariantCulture)));
            }
            cboPropertyValue.DataBind();
        }
    }
}