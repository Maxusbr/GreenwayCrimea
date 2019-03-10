using System;
using System.Collections.Generic;
using AdvantShop.Customers;
using AdvantShop.Core.Services.Attachments;
namespace AdvantShop.CMS
{
    public class AdminComment
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }

        public int ObjId { get; set; }
        public AdminCommentType Type { get; set; }

        public Guid? CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }

        public string Text { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public bool Deleted { get; set; }

        public string ObjUrl { get; set; }

        private Customer _customer;
        public Customer Customer
        {
            get { return _customer ?? (_customer = CustomerId.HasValue ? CustomerService.GetCustomer(CustomerId.Value) : null); }
        }

        //public virtual List<T> GetAttachments<T>() where T : Attachment, new()
        //{
        //    return AttachmentService.GetAttachments<T>(ObjId);
        //}
    }

    //public class TaskComment : AdminComment
    //{
    //    public new AdminCommentType Type
    //    {
    //        get { return AdminCommentType.Task; }
    //    }

    //    //private List<TaskAttachment> _attachments;
    //    //public List<TaskAttachment> Attachments
    //    //{
    //    //    get { return _attachments ?? (_attachments = base.GetAttachments<TaskAttachment>()); }
    //    //}
    //}
}