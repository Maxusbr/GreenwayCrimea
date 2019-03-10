using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using AdvantShop.App.Landing.Controllers.Domain;
using AdvantShop.App.Landing.Handlers.Pictures;
using AdvantShop.Catalog;

namespace AdvantShop.App.Landing.Domain.SubBlocks
{
    public class PictureSubBlock : BaseLpSubBlock
    {
        public override dynamic GetSettings(LpBlock currentBlock, Product product, dynamic settings)
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
            else if (settings.src != null)
            {
                var lp = new LpService().Get(currentBlock.LandingId);
                if (lp != null)
                {
                    string imgPath = HostingEnvironment.MapPath("~/Areas/Landing/Templates/" + lp.Template +"/" + settings.src.ToString());
                    if (imgPath != null && File.Exists(imgPath))
                    {
                        var result = new CopyPictureHandler(currentBlock.LandingId, currentBlock.Id, imgPath).Execute();
                        if (result.Result)
                        {
                            settings.src = result.Picture;
                        }
                    }
                }
            }

            return settings;
        }
    }
}
