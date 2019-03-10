namespace AdvantShop.Web.Admin.Models.Brands
{
    public class UploadBrandPictureResult
    {
        public bool Result { get; set; }
        public string Error { get; set; }
        public string Picture { get; set; }
        public int PictureId { get; set; }
        public string PictureName { get; set; }
    }
}
