namespace AdvantShop.Web.Admin.Models.Attachments
{
    public class AttachmentModel
    {
        public int Id { get; set; }
        public int ObjId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FilePathAdmin { get; set; }
        public string FileSize { get; set; }
    }
}
