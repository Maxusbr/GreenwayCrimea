using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Localization;

namespace AdvantShop.Web.Admin.Models.News
{
    public partial class NewsModel : IValidatableObject
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public bool ShowOnMainPage { get; set; }
        public bool Enabled { get; set; }
        public int? NewsCategoryId { get; set; }
        public string NewsCategory { get; set; }
        
        public DateTime AddingDate { get; set; }
        public string AddingDateFormatted { get { return Culture.ConvertDateWithoutSeconds(AddingDate); } }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                yield return new ValidationResult("Укажите название", new[] { "Title" });
            }
        }
    }
}
