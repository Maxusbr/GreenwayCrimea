namespace AdvantShop.Web.Admin.Models.Categories
{
    public class UploadCategoryPictureResult
    {
        public bool Result { get; set; }
        public string Error { get; set; }
        public string Picture { get; set; }
        public int PictureId { get; set; }
    }
}
