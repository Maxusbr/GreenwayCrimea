using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.LastOrder.Models
{
    public class FNPModel
    {
        public int ProductId { get; set; }
        public DateTime FakeDateTime { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
    }
}
