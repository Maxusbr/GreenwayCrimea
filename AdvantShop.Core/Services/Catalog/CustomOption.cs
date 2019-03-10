//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using Newtonsoft.Json;

namespace AdvantShop.Catalog
{

    public enum CustomOptionInputType
    {
        [Localize("Core.CustomOption.CustomOptionInputType.DropDownList")]
        DropDownList = 0,

        [Localize("Core.CustomOption.CustomOptionInputType.RadioButton")]
        RadioButton = 1,

        [Localize("Core.CustomOption.CustomOptionInputType.CheckBox")]
        CheckBox = 2,

        [Localize("Core.CustomOption.CustomOptionInputType.TextBoxSingleLine")]
        TextBoxSingleLine = 3,

        [Localize("Core.CustomOption.CustomOptionInputType.TextBoxMultiLine")]
        TextBoxMultiLine = 4
    }

    public enum CustomOptionField
    {
        Title = 1,
        SortOrder = 2
    }

    [Serializable]
    public class CustomOption : IDentable
    {
        private int _nullFields;
        public int CustomOptionsId { get; set; }
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public CustomOptionInputType InputType { get; set; }

        [JsonIgnore]
        public int SortOrder { get; set; }

        public int ProductId { get; set; }

        private List<OptionItem> _options;

        public List<OptionItem> Options 
        { 
            get { return _options ?? (_options = CustomOptionsService.GetCustomOptionItems(CustomOptionsId)); }
            set { _options = value; }
        }

        public CustomOption()
        {
        }

        public CustomOption(bool nullFields)
        {
            if (nullFields)
            {
                _nullFields = (int)CustomOptionField.Title | (int)CustomOptionField.SortOrder;
            }
            else
            {
                _nullFields = 0;
            }
        }

        public void SetFieldToNull(CustomOptionField field)
        {
            _nullFields = _nullFields | (int)field;
        }

        public bool IsNull(CustomOptionField field)
        {
            return (_nullFields & (int)field) > 0;
        }

        public int ID
        {
            get { return CustomOptionsId; }
        }

        public OptionItem SelectedOptions { get; set; }
    }
}