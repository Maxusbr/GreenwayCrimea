using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using AdvantShop.Catalog;
using AdvantShop.FilePath;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Colors
{
    public class ColorsFilterModel : BaseFilterModel
    {
        public string ColorName { get; set; }
    }

    
    public class ColorModel : IValidatableObject
    {
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }

        public string PhotoName { private get; set; }

        public string ColorIcon
        {
            get
            {
                return string.IsNullOrEmpty(PhotoName)
                    ? string.Empty
                    : FoldersHelper.GetImageColorPath(ColorImageType.Details, PhotoName, false);
            }
        }

        public int SortOrder { get; set; }
        public int ProductsCount { get; set; }

        public bool CanBeDeleted
        {
            get { return !ColorService.IsColorUsed(ColorId); }
        }

        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ColorName))
            {
                yield return new ValidationResult("Название обязательно", new[] { "Name" });
            }

            if (!string.IsNullOrEmpty(ColorCode) && !IsValidColorCode(ColorCode))
            {
                yield return new ValidationResult("Невалидный код цвета", new[] { "Url" });
            }
        }

        private static bool IsValidColorCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return false;

            //remove '#'
            code = code.Replace("#", "");

            if (code.Length != 6)
                return false;

            uint tmp;
            if (!uint.TryParse(code, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out tmp))
                return false;
            // ffffff - 16777215
            if (tmp > 16777215)
                return false;

            return true;
        }
    }
}
