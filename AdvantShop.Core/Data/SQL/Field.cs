//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace AdvantShop.Core.SQL
{
    [Serializable]
    public class Field
    {
        public Field()
        {
        }
        public Field(string name)
        {
            Name = name;
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                SelectExpression = value;
                if (value.Contains(" as ") || value.Contains(" AS "))
                {
                    var strings = value.Split(new[] { " as ", " AS " }, StringSplitOptions.RemoveEmptyEntries);
                    _name = strings.Last();
                    FilterExpression = strings.First();
                }
                else
                {
                    _name = value;
                    FilterExpression = value;
                }
            } 
        }

        public string SelectExpression { get; set; }
        public string FilterExpression { get; set; }
        public FieldFilter Filter{ get; set; }
        public SortDirection? Sorting{ get; set; }
        public bool NotInQuery{ get; set; }
        public bool IsDistinct{ get; set; }
    }
}