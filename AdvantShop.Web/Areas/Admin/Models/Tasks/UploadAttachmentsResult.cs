namespace AdvantShop.Web.Admin.Models.Attachments
{
    public class UploadAttachmentsResult
    {
        public bool Result { get; set; }
        public string Error { get; set; }
        public AttachmentModel Attachment { get; set; }
    }
}
