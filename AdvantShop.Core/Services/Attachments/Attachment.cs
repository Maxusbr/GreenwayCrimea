using System;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Services.Attachments
{
    public abstract class Attachment
    {
        public virtual AttachmentType Type
        {
            get { return AttachmentType.None; }
        }

        public int Id { get; set; }
        public int ObjId { get; set; }
        public string FileName { get; set; }
        public int SortOrder { get; set; }
        public long FileSize { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public string FileSizeFormatted
        {
            get { return FileHelpers.FileSize(FileSize); }
        }

        public string PathAbsolut
        {
            get { return AttachmentService.GetPathAbsolut(ObjId, Type, FileName); }
        }

        public string Path
        {
            get { return AttachmentService.GetPath(ObjId, Type, FileName, false); }
        }

        public string PathAdmin
        {
            get { return AttachmentService.GetPath(ObjId, Type, FileName, true); }
        }
    }

    public class TaskAttachment : Attachment
    {
        public override AttachmentType Type
        {
            get { return AttachmentType.Task; }
        }
    }

    public class LeadAttachment : Attachment
    {
        public override AttachmentType Type
        {
            get { return AttachmentType.Lead; }
        }
    }
}
