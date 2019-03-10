using AdvantShop.Catalog;
using AdvantShop.Core.Services.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.SimaLand.Service
{
    public class TagMarkerService
    {
        public static void MTGiftAdd(int productId)
        {
            var tag = TagService.GetByUrl("mt-gift");
            if (tag == null)
            {
                var tpt = new Tag()
                {
                    Name = "+ Подарок",
                    Enabled = true,
                    UrlPath = "mt-gift"
                };
                var tId = TagService.Add(tpt);
                tag = TagService.Get(tId);
            }
            var existTag = TagService.Gets(productId, ETagType.Product).Select(x => x).Where(t => t.UrlPath == "mt-gift");
            if (existTag.Count() == 0)
                TagService.AddMap(productId, tag.Id, ETagType.Product, 0);
        }

        public static void MTGiftRemove(Product product)
        {
            var tags = product.Tags;
            tags = tags.Select(x => x).Where(x => x.UrlPath != "mt-gift").ToList();
            TagService.DeleteMap(product.ProductId, ETagType.Product);
            if (tags != null && tags.Count != 0)
            {
                foreach (var item in tags)
                {
                    var tag = TagService.Get(item.Name);
                    item.Id = tag == null ? TagService.Add(item) : tag.Id;
                    TagService.AddMap(product.ProductId, item.Id, ETagType.Product, 0);
                }
            }
        }
    }
}
