using AdvantShop.Core.Common.Extensions;
using System;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public enum EFieldComparerType
    {
        None = 0,
        Equal = 1,
        Range = 2,
        Flag = 3,
        Contains = 4
    }

    public abstract class FieldComparer
    {
        public virtual EFieldComparerType Type
        {
            get { return EFieldComparerType.None; }
        }

        public int? FieldObjId { get; set; }

        public int? ValueObjId { get; set; }

        public virtual bool Check(string val)
        {
            return false;
        }

        public virtual bool Check(int val)
        {
            return false;
        }

        public virtual bool Check(float val)
        {
            return false;
        }

        public virtual bool Check(bool val)
        {
            return false;
        }

        public virtual bool Check(DateTime val)
        {
            return false;
        }
    }

    public class FieldEqualityComparer : FieldComparer
    {
        public override EFieldComparerType Type
        {
            get { return EFieldComparerType.Equal; }
        }

        public string Value { get; set; }

        public override bool Check(string val)
        {
            return Value.Equals(val, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Check(int val)
        {
            var intVal = Value.TryParseInt(true); // if Value is not int - return false
            return intVal == val;
        }

        public override bool Check(float val)
        {
            var floatVal = Value.TryParseFloat(true); // if Value is not float - return false
            return floatVal == val;
        }

        public override bool Check(bool val)
        {
            var boolVal = Value.TryParseBool(true); // if Value is not bool - return false
            return boolVal == val;
        }

        public override bool Check(DateTime val)
        {
            var dateTimeVal = Value.TryParseDateTime(true); // if Value is not DateTime - return false
            return dateTimeVal == val;
        }
    }

    public class FieldRangeComparer : FieldComparer
    {
        public override EFieldComparerType Type
        {
            get { return EFieldComparerType.Range; }
        }

        public float? From { get; set; }
        public float? To { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string DateFromString
        {
            get { return DateFrom.HasValue ? DateFrom.Value.ToString("dd.MM.yyyy") : string.Empty; }
        }
        public string DateToString
        {
            get { return DateTo.HasValue ? DateTo.Value.ToString("dd.MM.yyyy") : string.Empty; }
        }

        public override bool Check(int val)
        {
            return (!From.HasValue || From.Value < val) && (!To.HasValue || To.Value >= val);
        }

        public override bool Check(float val)
        {
            return (!From.HasValue || From.Value < val) && (!To.HasValue || To.Value >= val);
        }

        public override bool Check(DateTime val)
        {
            return (!DateFrom.HasValue || DateFrom.Value.Date < val.Date) && (!DateTo.HasValue || DateTo.Value.Date >= val.Date);
        }
    }

    public class FieldFlagComparer : FieldComparer
    {
        public override EFieldComparerType Type
        {
            get { return EFieldComparerType.Flag; }
        }

        public bool Flag { get; set; }

        public override bool Check(string val)
        {
            return Flag == val.TryParseBool(true);
        }

        public override bool Check(bool val)
        {
            return Flag == val;
        }
    }

    public class FieldContainsComparer : FieldComparer
    {
        public override EFieldComparerType Type
        {
            get { return EFieldComparerType.Contains; }
        }

        public string Value { get; set; }

        public override bool Check(string val)
        {
            return val.IsNotEmpty() && val.Contains(Value, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
