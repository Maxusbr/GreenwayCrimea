using AdvantShop.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Service
{
    public static class Strings
    {
        public static bool TryParseBoolean(this string val)
        {
            try
            {
                if (val.ToLower() == "да")
                {
                    return true;
                }
                if (val.ToLower() == "нет")
                {
                    return false;
                }
                int res;
                var IsDigit = int.TryParse(val, out res);
                return IsDigit ? Convert.ToBoolean(res) : Convert.ToBoolean(val);
            }
            catch (Exception ex)
            {
                var msg = " TryParseBoolean Convert error" + " val is '"+val+"'" + Environment.NewLine;
                LogService.ErrLog(msg);
                return false;
            }
        }

        public static int TryParseInteger(this string val)
        {
            try
            {
                if (string.IsNullOrEmpty(val))
                {
                    return 0;
                }
                if (val.ToLower() == "достаточно")
                {
                    return 1000;
                }
                int res;
                var IsDigit = int.TryParse(val, out res);
                return IsDigit ? Convert.ToInt32(res) : 0;
            }
            catch (Exception ex)
            {
                var msg = " TryParseInteger Convert error" + " val is '" + val + "'" + Environment.NewLine;
                LogService.ErrLog(msg);
                return 0;
            }
        }

        public static string YesNo(this bool val)
        {
            return val ? "да" : "нет";
        }

    }
}
