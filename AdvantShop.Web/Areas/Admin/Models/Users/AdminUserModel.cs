using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Web.Admin.Handlers.Users;

namespace AdvantShop.Web.Admin.Models.Users
{
    public class AdminUserModel
    {
        // customer fields
        public Guid CustomerId { get; set; }
        public Role CustomerRole { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public bool Enabled { get; set; }
        public Guid? HeadCustomerId { get; set; }
        public DateTime? BirthDay { get; set; }
        public string City { get; set; }

        // manager fields
        public int? AssociatedManagerId { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Position { get; set; }

        public string FullName { get; set; }

        public bool EditHimself { get; set; }

        public string PhotoEncoded { get; set; }
        public string PhotoSrc
        {
            get
            {
                return Avatar.IsNotEmpty()
                    ? string.Format("{0}?rnd={1}", FoldersHelper.GetPath(FolderType.Avatar, Avatar, false), new Random().Next())
                    : UrlService.GetAdminStaticUrl() + "images/no-avatar.jpg";
            }
        }

        private List<UserRoleActionModel> _roleActionKeys;
        public List<UserRoleActionModel> RoleActionKeys
        {
            get { return _roleActionKeys ?? (_roleActionKeys = new GetUserRoleActions(CustomerId).Execute()); }
            set { _roleActionKeys = value; }
        }

        private List<ManagerRole> _managerRoles;
        public List<ManagerRole> ManagerRoles
        {
            get { return _managerRoles ?? (_managerRoles = CustomerId != Guid.Empty ? ManagerRoleService.GetManagerRoles(CustomerId) : new List<ManagerRole>()); }
        }

        private List<int> _managerRolesIds;
        public List<int> ManagerRolesIds
        {
            get { return _managerRolesIds ?? (_managerRolesIds = ManagerRoles.Select(x => x.Id).ToList()); }
            set { _managerRolesIds = value; }
        }

        public string Roles
        {
            get
            {
                var roles = new List<string>
                {
                    CustomerRole.Localize()
                };
                roles.AddRange(ManagerRoles.Select(x => x.Name));
                return roles.AggregateString("; ");
            }
        }

        private bool _canDeleteInited;
        private void InitCanDelete()
        {
            if (_canDeleteInited)
                return;
            if (CustomerId != Guid.Empty)
            {
                List<string> messages;
                _canBeDeleted = CustomerService.CanDelete(CustomerId, out messages);
                _сantDeleteMessage = messages.AggregateString("<br/>");
            }
            _canDeleteInited = true;
        }

        public string _сantDeleteMessage;
        public string CantDeleteMessage
        {
            get
            {
                InitCanDelete();
                return _сantDeleteMessage;
            }
        }

        public bool _canBeDeleted;
        public bool CanBeDeleted
        {
            get
            {
                InitCanDelete();
                return _canBeDeleted;
            }
        }
    }
}
