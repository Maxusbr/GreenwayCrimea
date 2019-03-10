using System.Collections.Generic;
using AdvantShop.Web.Admin.Models.Categories;
using Newtonsoft.Json;

namespace AdvantShop.Web.Admin.Models.Products
{
    public class UploadPhotoResultModel
    {
        [JsonProperty("result")]
        public bool Result { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        public List<UploadCategoryPictureResult> PhotoResults { get; set; }
    }
}
