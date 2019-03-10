using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.SEO;
using AdvantShop.Catalog;
using System.Web.Mvc;

namespace AdvantShop.Web.Admin.Models.News
{
    public class AddEditNewsModel : IValidatableObject
    {
        public int NewsId { get; set; }
        public int NewsCategoryId { get; set; }
        public List<SelectListItem> NewsCategory { get; set; }
        public string Title { get; set; }
        public string TextToPublication { get; set; }
        public string TextToEmail { get; set; }
        public string TextAnnotation { get; set; }
        public bool ShowOnMainPage { get; set; }
        public bool Enabled { get; set; }
        public DateTime AddingDate { get; set; }
        public string AddingDates { get; set; }
        public string UrlPath { get; set; }
        private string PhotoName { get; set; }
        public string PhotoSrc { get; set; }
        public string FileName { get; set; }
        public int PhotoId { get; set; }

        private NewsPhoto _picture;
        public NewsPhoto Picture
        {
            get
            {
                if (_picture != null)
                    return _picture;
                if (!string.IsNullOrEmpty(PhotoName))
                {
                    _picture = new NewsPhoto() { PhotoName = PhotoName };
                    return _picture;
                }
                return (_picture = PhotoService.GetPhotoByObjId<NewsPhoto>(NewsId, PhotoType.News));
            }
            set
            {
                _picture = value;
            }
        }
        public MetaType MetaType
        {
            get { return MetaType.News; }
        }
        private MetaInfo _meta;
        public MetaInfo Meta
        {
            get
            {
                return _meta ??
                       (_meta =
                        MetaInfoService.GetMetaInfo(NewsId, MetaType) ??
                        MetaInfoService.GetDefaultMetaInfo(MetaType, Title));
            }
            set
            {
                _meta = value;
            }
        }

        public bool IsEditMode { get; set; }
        public bool DefaultMeta { get; set; }
        public string SeoTitle { get; set; }
        public string SeoH1 { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                yield return new ValidationResult("Укажите название", new[] { "Title" });
            }
            if (NewsCategoryId == 0)
            {
                yield return new ValidationResult("Укажите категорию новости", new[] { "NewsCategoryId" });
            }
            if (string.IsNullOrWhiteSpace(TextToPublication))
            {
                yield return new ValidationResult("Укажите текст новости", new[] { "TextToPublication" });
            }
            if (string.IsNullOrWhiteSpace(TextAnnotation))
            {
                yield return new ValidationResult("Укажите аннотацию", new[] { "TextAnnotation" });
            }
            if (string.IsNullOrWhiteSpace(UrlPath))
            {
                yield return new ValidationResult("Укажите cиноним для URL запроса", new[] { "UrlPath" });
            }
        }
    }
}
