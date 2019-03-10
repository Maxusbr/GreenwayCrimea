namespace AdvantShop.Web.Admin.Models.News
{
    public class UploadNewsPictureResult
    {
        public bool Result { get; set; }
        public string Error { get; set; }
        public string Picture { get; set; }
        public int PictureId { get; set; }
        public string FileName { get; set; }
    }
}
