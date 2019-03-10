using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Service
{
    public class BreakTaskException : Exception
    {
        public BreakTaskException(string message) : base(message)
        { }
    }

    public class CategoryIsNullException : Exception
    {
        public CategoryIsNullException(string message) : base(message)
        { }
    }
}
