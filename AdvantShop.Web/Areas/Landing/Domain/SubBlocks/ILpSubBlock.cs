using AdvantShop.Catalog;
using System.Linq;

namespace AdvantShop.App.Landing.Domain.SubBlocks
{
    public interface ILpSubBlock
    {
        string GetContent(Product product);

        dynamic GetSettings(LpBlock currentBlock, Product product, dynamic settings);
    }

    public class BaseLpSubBlock : ILpSubBlock
    {
        public virtual string GetContent(Product product)
        {
            return null;
        }

        public virtual dynamic GetSettings(LpBlock currentBlock, Product product, dynamic settings)
        {
            if (product != null && product.Offers != null)
            {
                var offer = OfferService.GetMainOffer(product.Offers, product.AllowPreOrder);

                var photo = offer != null && offer.Photo != null
                    ? offer.Photo
                    : product.ProductPhotos.OrderByDescending(item => item.Main)
                        .ThenBy(item => item.PhotoSortOrder)
                        .FirstOrDefault(item => item.Main);

                if (settings != null && photo != null)
                {
                    settings.src = photo.ImageSrcMiddle();
                }
            }

            return settings;
        }
    }
}
