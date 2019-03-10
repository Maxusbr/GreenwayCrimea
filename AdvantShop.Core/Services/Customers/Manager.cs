//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.FilePath;
using Newtonsoft.Json;

namespace AdvantShop.Customers
{
    public class Manager
    {
        public int ManagerId { get; set; }
        public Guid CustomerId { get; set; }
        public int? DepartmentId { get; set; }
        public string Position { get; set; }
        public bool Active { get; set; }

        private ManagerPhoto _managerLogo;
        [Obsolete("use AvatarSrc")]
        public ManagerPhoto Photo
        {
            get
            {
                return _managerLogo ?? (_managerLogo = PhotoService.GetPhotoByObjId<ManagerPhoto>(ManagerId, PhotoType.Manager));
            }
            set
            {
                _managerLogo = value;
            }
        }

        public string AvatarSrc
        {
            get
            {
                return Customer != null && Customer.Avatar.IsNotEmpty() 
                    ? FoldersHelper.GetPath(FolderType.Avatar, Customer.Avatar, false) 
                    : string.Empty;
            }
        }

        private Customer _customer;
        [JsonIgnore]
        public Customer Customer
        {
            get { return _customer ?? (_customer = CustomerService.GetCustomer(CustomerId)); }
        }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public string FirstName
        {
            get { return Customer != null ? Customer.FirstName : string.Empty; }
        }

        public string LastName
        {
            get { return Customer != null ? Customer.LastName : string.Empty; }
        }

        public long? StandardPhone
        {
            get { return Customer != null ? Customer.StandardPhone : null; }
        }

        public string Email
        {
            get { return Customer != null ? Customer.EMail : string.Empty; }
        }


        public override string ToString()
        {
            return FullName;
        }
    }
}
