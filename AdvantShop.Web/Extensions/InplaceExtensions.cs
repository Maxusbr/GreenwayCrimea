using System;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.SEO;
using AdvantShop.FilePath;

namespace AdvantShop.Extensions
{
    public static class InplaceExtensions
    {

        const string richSimple = "{editorSimple: true}";
        const string richTpl = "data-inplace-rich=\"{4}\" contenteditable=\"true\" data-inplace-url=\"{3}\" data-inplace-params=\"{{id: {0}, type: '{1}', field: '{2}'}}\" placeholder=\"{5}\"";

        // Static page
        public static HtmlString InplaceStaticPage(this HtmlHelper helper, int staticPageId, StaticPageInplaceField field)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Cms))
                return new HtmlString("");

            return new HtmlString(String.Format(richTpl, staticPageId, InplaceType.StaticPage, field, "inplaceeditor/staticpage", string.Empty, "Нажмите сюда, чтобы добавить описание"));
        }

        // Static block
        public static HtmlString InplaceStaticBlock(this HtmlHelper helper, int staticBlockId)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Cms))
                return new HtmlString("");

            return new HtmlString(String.Format(richTpl, staticBlockId, InplaceType.StaticBlock, StaticBlockInplaceField.Content, "inplaceeditor/staticblock", string.Empty, "Нажмите сюда, чтобы добавить описание"));
        }

        // News
        public static HtmlString InplaceNews(this HtmlHelper helper, int newsId, NewsInplaceField field)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Cms))
                return new HtmlString("");

            return new HtmlString(String.Format(richTpl, newsId, InplaceType.NewsItem, field, "inplaceeditor/news", string.Empty, "Нажмите сюда, чтобы добавить описание"));
        }

        // Tags

        public static HtmlString InplaceTag(this HtmlHelper helper, int TagId, TagInplaceField field)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");

            return new HtmlString(String.Format(richTpl, TagId, InplaceType.Category, field, "inplaceeditor/tag", string.Empty, "Нажмите сюда, чтобы добавить описание"));
        }

        // Category

        public static HtmlString InplaceCategory(this HtmlHelper helper, int categoryId, CategoryInplaceField field)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");

            return new HtmlString(String.Format(richTpl, categoryId, InplaceType.Category, field, "inplaceeditor/category", string.Empty, "Нажмите сюда, чтобы добавить описание"));
        }
        
        /// <summary>
        /// reviewId принимает либо int id отзыва, либо имя параметра в модели ангуляра типа string
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="reviewId"></param>
        /// <param name="field"></param>
        /// <param name="editorSimple"></param>
        /// <returns></returns>
        public static HtmlString InplaceReview(this HtmlHelper helper, string reviewId, ReviewInplaceField field, bool editorSimple = false)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");

            return new HtmlString(String.Format(richTpl, reviewId, InplaceType.Review, field, "inplaceeditor/review", editorSimple ? richSimple : string.Empty, "Нажмите сюда, чтобы добавить описание"));
        }


        // Product
        public static HtmlString InplaceProduct(this HtmlHelper helper, int productId, ProductInplaceField field, bool editorSimple = false)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");

            return new HtmlString(String.Format(richTpl, productId, InplaceType.Product, field, "inplaceeditor/product", editorSimple ? richSimple : string.Empty, "Нажмите сюда, чтобы добавить описание"));
        }

        public static HtmlString InplaceProductUnit(int productId, ProductInplaceField field)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");

            return new HtmlString(String.Format(richTpl, productId, InplaceType.Product, field, "inplaceeditor/product", richSimple, LocalizationService.GetResource("Inplace.InplaceProductUnit.Unit").ToLower()));
        }

        // Offer
        public static HtmlString InplaceOfferPrice(this HtmlHelper helper)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");


            return
                new HtmlString(String.Format("data-inplace-price data-inplace-url=\"inplaceeditor/offer\" data-inplace-params=\"{{'type':'{0}'}}\"",
                    InplaceType.Offer));
        }

        public static HtmlString InplaceOfferAmount(int offerId)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");

            return
                new HtmlString(String.Format(richTpl, offerId, InplaceType.Offer, OfferInplaceField.Amount, "inplaceeditor/offer", richSimple, "Нажмите сюда, чтобы добавить описание"));
        }

        public static HtmlString InplaceOfferArtNo(this HtmlHelper helper, string ngOfferId)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");

            return
                new HtmlString(String.Format(richTpl, ngOfferId, InplaceType.Offer, OfferInplaceField.ArtNo, "inplaceeditor/offer", richSimple, "Нажмите сюда, чтобы добавить описание"));
        }

        // Brand
        public static HtmlString InplaceBrand(this HtmlHelper helper, int brandId, BrandInplaceField field)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");

            return
                new HtmlString(String.Format(richTpl, brandId, InplaceType.Brand, field, "inplaceeditor/brand", string.Empty, "Нажмите сюда, чтобы добавить описание"));
        }

        // Property
        public static HtmlString InplaceProperty(this HtmlHelper helper, int propertyValueId, int propertyId, int productId, PropertyInplaceField field, string selectorBlock = "")
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");

            return
                new HtmlString(String.Format(
                        "data-inplace-autocomplete=\"{{propertyId: {1}, productId: {2}}}\" data-inplace-url=\"inplaceeditor/property\" data-inplace-params=\"{{'id':{0}, propertyId: {1}, productId: {2}, 'type':'{3}', 'field':'{4}'}}\" data-inplace-autocomplete-selector-block=\"{5}\"",
                        propertyValueId,
                        propertyId,
                        productId,
                        InplaceType.Property,
                        field,
                        selectorBlock));
        }

        // Meta
        public static HtmlString InplaceMeta(this HtmlHelper helper, int id, MetaType metaType)
        {
            if ((metaType == MetaType.Brand || metaType == MetaType.Category || metaType == MetaType.Product || metaType == MetaType.Tag)
                && !InplaceEditorService.CanUseInplace(RoleAction.Catalog))
            {
                return new HtmlString("");
            }
            else if ((metaType == MetaType.News || metaType == MetaType.NewsCategory || metaType == MetaType.StaticPage)
                && !InplaceEditorService.CanUseInplace(RoleAction.Cms))
            {
                return new HtmlString("");
            }

            return
                new HtmlString(String.Format(
                        "data-inplace-modal data-inplace-url=\"inplaceeditor/meta\" data-inplace-params=\"{{'id':{0}, 'type':'{1}', metaType: '{2}'}}\"",
                        id, InplaceType.Meta, metaType));
        }

        // Image
        public static string InplaceImageLogo()
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Settings))
                return string.Empty;

            return String.Format(
                        "data-inplace-image data-ngf-drop=\"\" accept=\"image/*\" data-ngf-accept=\"'image/*'\" data-ngf-change=\"inplaceImage.fileDrop($files, $event, $rejectedFiles)\"  " +
                        "data-inplace-url=\"inplaceeditor/image\" data-inplace-params=\"{{field: '{0}'}}\" {1}",
                        ImageInplaceField.Logo, InplaceImageButtonsLogo(addVisible: false));
        }

        public static HtmlString InplaceImageBrand(this HtmlHelper helper, int brandId)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");

            return
                new HtmlString(String.Format(
                        "data-inplace-image data-ngf-drop=\"\" accept=\"image/*\" data-ngf-accept=\"'image/*'\" data-ngf-change=\"inplaceImage.fileDrop($files, $event, $rejectedFiles)\" " +
                        "data-inplace-url=\"inplaceeditor/image\" data-inplace-params=\"{{'id':{0}, 'type':'{1}', 'field':'{2}'}}\" {3}",
                        brandId,
                        InplaceType.Image,
                        ImageInplaceField.Brand,
                        InplaceImageButtons(false)));
        }

        public static HtmlString InplaceImageNews(this HtmlHelper helper, int newsId)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Cms))
                return new HtmlString("");

            return
                new HtmlString(String.Format(
                        "data-inplace-image data-ngf-drop=\"\" accept=\"image/*\" data-ngf-accept=\"'image/*'\" data-ngf-change=\"inplaceImage.fileDrop($files, $event, $rejectedFiles)\" " +
                        "data-inplace-url=\"inplaceeditor/image\" data-inplace-params=\"{{'id':{0}, 'type':'{1}', 'field':'{2}'}}\" {3}",
                        newsId,
                        InplaceType.Image,
                        ImageInplaceField.News,
                        InplaceImageButtons(false)));
        }

        public static HtmlString InplaceImageCarousel(this HtmlHelper helper, int slideId, bool specialSlideForInplace)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Cms))
                return new HtmlString("");

            return
                new HtmlString(String.Format(
                        "data-inplace-image data-ngf-drop=\"\" accept=\"image/*\" data-ngf-accept=\"'image/*'\" data-ngf-change=\"inplaceImage.fileDrop($files, $event, $rejectedFiles)\" " +
                        "data-inplace-url=\"inplaceeditor/image\" data-inplace-params=\"{{'id':{0}, 'type':'{1}', 'field':'{2}'}}\" {3}",
                        slideId,
                        InplaceType.Image,
                        ImageInplaceField.Carousel,
                        InplaceImageButtons(true, !specialSlideForInplace, !specialSlideForInplace, specialSlideForInplace)));
        }

        public static HtmlString InplaceImageCategory(this HtmlHelper helper, int categoryId, ImageInplaceField field)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");

            return
                new HtmlString(String.Format(
                        "data-inplace-image data-ngf-drop=\"\" accept=\"image/*\" data-ngf-accept=\"'image/*'\" data-ngf-change=\"inplaceImage.fileDrop($files, $event, $rejectedFiles)\" " +
                        "data-inplace-url=\"inplaceeditor/image\" data-inplace-params=\"{{'id':{0}, 'type':'{1}', 'field':'{2}'}}\" {3}",
                        categoryId,
                        InplaceType.Image,
                        field.ToString(),
                        InplaceImageButtons(false)));
        }

        /// <summary>
        /// reviewId принимает либо int id отзыва, либо имя параметра в модели ангуляра типа string
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ngModelReviewId"></param>
        /// <returns></returns>
        public static HtmlString InplaceImageReview(this HtmlHelper helper, string reviewId)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");

            return
                new HtmlString(String.Format(
                        "data-inplace-image data-ngf-drop=\"\" accept=\"image/*\" data-ngf-accept=\"'image/*'\" data-ngf-change=\"inplaceImage.fileDrop($files, $event, $rejectedFiles)\" " +
                        "data-inplace-url=\"inplaceeditor/image\" data-inplace-params=\"{{'id':{0}, 'type':'{1}', 'field':'{2}'}}\" {3}",
                        reviewId,
                        InplaceType.Image,
                        ImageInplaceField.Review.ToString(),
                        InplaceImageButtons(false)));
        }

        public static HtmlString InplaceImageProduct(this HtmlHelper helper, int photoId, int productId, ProductImageType productImageType)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Catalog))
                return new HtmlString("");

            return new HtmlString(String.Format(
                    "data-inplace-image data-ngf-drop=\"\" accept=\"image/*\" data-ngf-accept=\"'image/*'\" data-ngf-change=\"inplaceImage.fileDrop($files, $event, $rejectedFiles)\" " +
                    "data-inplace-url=\"inplaceeditor/image\" data-inplace-params=\"{{'id':{0}, 'type':'{1}', 'field':'{2}', objId:{3}, additionalData: '{4}'}}\" {5}",
                    photoId,
                    InplaceType.Image,
                    ImageInplaceField.Product,
                    productId,
                    productImageType.ToString(),
                    InplaceImageButtons()));
        }

        private static string InplaceImageButtons(bool addVisible = true, bool updateVisible = true, bool deleteVisible = true, bool permanentVisible = false)
        {
            return string.Format("data-inplace-image-buttons-visible=\"{{'add': {0}, 'update': {1}, 'delete' : {2}, 'permanentVisible': {3}}}\"",
                addVisible.ToString().ToLower(),
                updateVisible.ToString().ToLower(),
                deleteVisible.ToString().ToLower(),
                permanentVisible.ToString().ToLower());
        }

        private static string InplaceImageButtonsLogo(bool addVisible = true, bool updateVisible = true, bool deleteVisible = true, bool permanentVisible = false, bool editLogo = true)
        {
            return string.Format("data-inplace-image-buttons-visible=\"{{'add': {0}, 'update': {1}, 'delete' : {2}, 'permanentVisible': {3}, 'editLogo': {4}}}\"",
                addVisible.ToString().ToLower(),
                updateVisible.ToString().ToLower(),
                deleteVisible.ToString().ToLower(),
                permanentVisible.ToString().ToLower(),
                editLogo.ToString().ToLower());
        }

        // Modules
        public static HtmlString InplaceModule(this HtmlHelper helper, int id, string entityType, string field)
        {
            if (!InplaceEditorService.CanUseInplace(RoleAction.Modules))
                return new HtmlString("");

            return new HtmlString(String.Format(richTpl, id, entityType, field, "inplaceeditor/module", string.Empty, "Нажмите сюда, чтобы добавить описание"));
        }
    }
}