using System;
using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Core.Services.Attachments;
using AdvantShop.Customers;
using AdvantShop.Orders;

namespace AdvantShop.Core.Services.Crm
{
    public class Task : ICloneable
    {
        public int Id { get; set; }
        public int TaskGroupId { get; set; }
        public int? AssignedManagerId { get; set; }
        public int? AppointedManagerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public bool Accepted { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int? LeadId { get; set; }
        public int? OrderId { get; set; }
        public int? ReviewId { get; set; }
        public Guid? CustomerId { get; set; }
        public string ResultShort { get; set; }
        public string ResultFull { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public DateTime DateAppointed { get; set; }
        public bool IsAutomatic { get; set; }
        public bool IsDeferred { get; set; }

        public int SortOrder { get; set; }

        private List<AdminComment> _comments;
        public List<AdminComment> Comments
        {
            get { return _comments ?? (_comments = AdminCommentService.GetAdminComments(Id, AdminCommentType.Task)); }
        }

        private List<TaskAttachment> _attachments;
        public List<TaskAttachment> Attachments
        {
            get { return _attachments ?? (_attachments = AttachmentService.GetAttachments<TaskAttachment>(Id)); }
        }

        private Manager _assignedManager;
        public Manager AssignedManager
        {
            get
            {
                return AssignedManagerId.HasValue 
                    ? _assignedManager ?? (_assignedManager = ManagerService.GetManager(AssignedManagerId.Value))
                    : null;
            }
        }

        private Manager _appointedManager;
        public Manager AppointedManager
        {
            get
            {
                return AppointedManagerId.HasValue 
                    ? _appointedManager ?? (_appointedManager = ManagerService.GetManager(AppointedManagerId.Value))
                    : null;
            }
        }

        private TaskGroup _taskGroup;
        public TaskGroup TaskGroup
        {
            get
            {
                return _taskGroup ?? (_taskGroup = TaskGroupService.GetTaskGroup(TaskGroupId));
            }
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

        private Order _order;
        public Order Order
        {
            get
            {
                return OrderId.HasValue ? _order ?? (_order = OrderService.GetOrder(OrderId.Value)) : null;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
