//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdvantShop.Core.Controls
{
    [ParseChildren(true)]
    public class EnumDataSource : Control, IDataSource
    {
        public EnumDataSource()
        {
            _ExceptValues = new List<int>();
        }
        private Type _enumType;
        public Type EnumType { get { return _enumType; } }
        public string EnumTypeName
        {
            get { return _enumType.Name; }
            set
            {
                Type type = Type.GetType(value, true, true);
                if (type == null)
                    throw new ArgumentException("Invalid type name");
                if (type.BaseType != typeof(Enum))
                    throw new ArgumentException("Type must be an enumeration.");
                _enumType = type;
            }
        }
        public List<int> _ExceptValues { get; private set; }

        [Category("Behavior"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public List<ListItem> ExceptValues 
        { 
            get { return _ExceptValues.Select(val => new ListItem(val.ToString())).ToList(); } 
            set { _ExceptValues = value.Select(item => int.Parse(item.Value)).ToList(); } 
        }

        protected override void AddParsedSubObject(object obj)
        {
            if (obj is ListItem) _ExceptValues.Add(int.Parse(((ListItem)obj).Value));
            else base.AddParsedSubObject(obj);
        }

        public DataSourceView GetView(string viewName)
        {
            return new EnumDataSourceView(this, "default");
        }

        public ICollection GetViewNames()
        {
            return new[] { "default" };
        }

        public event EventHandler DataSourceChanged;

    }
}
