using System;
using System.Collections.Generic;

namespace AdvantShop.App.Landing.Domain
{
    public class Lp
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Template { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class LpTemplate
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public List<string> Blocks { get; set; }
    }
}
