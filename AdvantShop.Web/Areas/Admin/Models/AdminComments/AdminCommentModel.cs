using System;
using AdvantShop.CMS;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.FilePath;

namespace AdvantShop.Web.Admin.Models.AdminComments
{
    public class AdminCommentModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int ObjId { get; set; }
        public AdminCommentType Type { get; set; }
        public Guid? CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarSrc { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public string ObjUrl { get; set; }

        public string DateCreatedFormatted { get { return DateCreated.ToString("d MMMM yyyy HH:mm"); } }

        public bool IsOwner
        {
            get { return CustomerId.HasValue && CustomerId.Value == CustomerContext.CustomerId; }
        }

        public AdminCommentModel ParentComment { get; set; }

        public static explicit operator AdminCommentModel(AdminComment comment)
        {
            if (comment == null)
                return null;

            return new AdminCommentModel
            {
                Id = comment.Id,
                ParentId = comment.ParentId,
                ObjId = comment.ObjId,
                Type = comment.Type,
                CustomerId = comment.CustomerId,
                Name = comment.Name,
                Email = comment.Email,
                Text = comment.Text,
                DateCreated = comment.DateCreated,
                AvatarSrc = comment.Avatar.IsNotEmpty() 
                    ? FoldersHelper.GetPath(FolderType.Avatar, comment.Avatar, false)
                    : UrlService.GetAdminStaticUrl() + "images/no-avatar.jpg"
            };
        }
    }
}
