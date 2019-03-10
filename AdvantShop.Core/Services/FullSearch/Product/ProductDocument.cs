using AdvantShop.Catalog;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using System.Collections.Generic;
using System.Linq;

namespace AdvantShop.Core.Services.FullSearch
{
    public class ProductDocument : BaseDocument
    {
        private string _artNo;
        [SearchField]
        public string ArtNo
        {
            get { return _artNo; }
            set
            {
                _artNo = value;
                AddParameterToDocumentNoStoreAnalyzed(_artNo, boost: HighBoost);
            }
        }

        private string _name;
        [SearchField]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                AddParameterToDocumentNoStoreAnalyzed(_name, boost: HighBoost);
            }
        }

        private IEnumerable<string> _offerArtNo;
        [SearchField]
        public IEnumerable<string> OfferArtNo
        {
            get { return _offerArtNo; }
            set
            {
                _offerArtNo = value;
                foreach (var item in _offerArtNo)
                {
                    AddParameterToDocumentNoStoreAnalyzed(item);
                }
            }
        }

        private IEnumerable<string> _tags;
        [SearchField]
        public IEnumerable<string> Tags
        {
            get { return _tags; }
            set
            {
                _tags = value;
                foreach (var item in _tags)
                {
                    AddParameterToDocumentNoStoreAnalyzed(item);
                }
            }
        }

        private string _desc;
        [SearchField]
        public string Desc
        {
            get { return _desc; }
            set
            {
                _desc = value;
                if (!string.IsNullOrWhiteSpace(_desc))
                {
                    _desc = _desc.Replace("<\br>", " ").Replace("<br />", " ");
                    _desc = StringHelper.RemoveHTML(_desc);
                }
                //if (string.IsNullOrWhiteSpace(_desc))
                //    _desc = StringHelper.RemoveHTML(_desc);
                AddParameterToDocumentNoStoreAnalyzed(_desc, boost: LowBoost);
            }
        }

        private bool _enabled;
        public bool Enabled
        {

            get { return _enabled; }
            set
            {
                _enabled = value;
                AddParameterToDocumentNoStoreAnalyzed(_enabled);
            }
        }


        //[SearchField("OfferArtNo", "Artno", "RegSuffix")]
        public static explicit operator ProductDocument(Product model)
        {
            var pDocument = new ProductDocument()
            {
                Id = model.ProductId,
                ArtNo = StringHelper.ReplaceCirilikSymbol(model.ArtNo),
                Name = StringHelper.ReplaceCirilikSymbol(model.Name),
                Desc = StringHelper.ReplaceCirilikSymbol(model.Description),
                OfferArtNo = (model.Offers ?? new List<Offer>()).Select(l => StringHelper.ReplaceCirilikSymbol(l.ArtNo)),
                Tags = (model.Tags != null && (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveTags))
                    ? model.Tags
                    : new List<Tag>()).Select(l => StringHelper.ReplaceCirilikSymbol(l.Name)),
                Enabled = model.Enabled && model.CategoryEnabled
            };

            if ((model.Offers ?? new List<Offer>()).Any(offer => offer.Amount > 0))
            {
                pDocument.Boost(HighBoost);
            }
            else
            {
                if (!model.AllowPreOrder)
                {
                    pDocument.Boost(LowBoost);
                }
            }
            return pDocument;
        }
    }
}
