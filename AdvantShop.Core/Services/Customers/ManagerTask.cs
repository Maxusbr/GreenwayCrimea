//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Customers
{
    public enum ManagerTaskStatus
    {
        [Localize("Core.Customers.ManagerTaskStatus.Opened")]
        Opened,

        [Localize("Core.Customers.ManagerTaskStatus.Closed")]
        Closed,

        [Localize("Core.Customers.ManagerTaskStatus.Canceled")]
        Canceled,

        [Localize("Core.Customers.ManagerTaskStatus.Postponed")]
        Postponed
    }

    public class ManagerTask
    {
        public int TaskId { get; set; }
        public int AssignedManagerId { get; set; }
        public int AppointedManagerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ManagerTaskStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DateCreated { get; set; }
        public int? OrderId { get; set; }
        public Guid? CustomerId { get; set; }
        public int? LeadId { get; set; }
        public string ResultShort { get; set; }
        public string ResultFull { get; set; }


        private Manager _assignedManager;
        public Manager AssignedManager
        {
            get { return _assignedManager ?? (_assignedManager = ManagerService.GetManager(AssignedManagerId)); }
        }

        private Manager _appointedManager;
        public Manager AppointedManager
        {
            get { return _appointedManager ?? (_appointedManager = ManagerService.GetManager(AppointedManagerId)); }
        }

        private Customer _clientCustomer;
        public Customer ClientCustomer
        {
            get
            {
                return CustomerId.HasValue
                    ? _clientCustomer ?? (_clientCustomer = CustomerService.GetCustomer(CustomerId.Value))
                    : null;
            }
        }
    }
}
