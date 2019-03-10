using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.CMS;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Customers;
using AdvantShop.Mails;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Handlers.AdminNotifications;
using AdvantShop.Web.Admin.Models.AdminComments;

namespace AdvantShop.Web.Admin.Handlers.AdminComments
{
    public class AddAdminCommentHandler
    {
        private AdminCommentModel _commentModel;
        private Customer _author;

        public AddAdminCommentHandler(AdminCommentModel model)
        {
            _commentModel = model;
            _author = CustomerContext.CurrentCustomer;
        }

        public AddAdminCommentResult Execute()
        {
            var comment = new AdminComment
            {
                ParentId = _commentModel.ParentId,
                ObjId = _commentModel.ObjId,
                Type = _commentModel.Type,
                CustomerId = _author.Id,
                Name = string.Join(" ", _author.FirstName, _author.LastName),
                Email = _author.EMail,
                Text = _commentModel.Text,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                ObjUrl = _commentModel.ObjUrl
            };

            comment.Id = AdminCommentService.AddAdminComment(comment);
            comment.Avatar = _author.Avatar;

            var result = new AddAdminCommentResult
            {
                Result = true,
                Comment = (AdminCommentModel)comment,
            };

            AdminComment parentComment = null;
            if (_commentModel.ParentId.HasValue)
            {
                parentComment = AdminCommentService.GetAdminComment(_commentModel.ParentId.Value);
                result.Comment.ParentComment = (AdminCommentModel)parentComment;
            }

            ProcessNotifications(comment, parentComment);

            return result;
        }

        private void ProcessNotifications(AdminComment comment, AdminComment parentComment)
        {
            // уведомление об ответе на комментарий
            AdminNotification parentCommentNotification = null;
            // уведомление о новом комментарии
            AdminNotification commentNotification = null;
            // id сотрудников для отправки уведомлений
            var customerIds = new List<Guid>();

            var emailsDict = new Dictionary<Guid, string>();
            MailTemplate commentMailTpl = null;

            switch (comment.Type)
            {
                case AdminCommentType.Task:
                    var task = TaskService.GetTask(comment.ObjId);
                    if (task == null)
                        break;
                    if (parentComment != null && parentComment.Customer != null && parentComment.Customer.HasRoleAction(RoleAction.Tasks))
                        parentCommentNotification = new OnTaskCommentAnswerNotification(task, _author, comment);

                    if (task.AssignedManager != null && task.AssignedManager.Customer != null && task.AssignedManager.Customer.HasRoleAction(RoleAction.Tasks))
                        customerIds.Add(task.AssignedManager.CustomerId);
                    if (task.AppointedManager != null && task.AppointedManager.Customer != null && task.AppointedManager.Customer.HasRoleAction(RoleAction.Tasks))
                        customerIds.Add(task.AppointedManager.CustomerId);
                    if (customerIds.Any())
                        commentNotification = new OnTaskCommentNotification(task, _author, comment);

                    TaskService.OnTaskCommentAdded(comment, task);
                    break;

                case AdminCommentType.Order:
                    var order = OrderService.GetOrder(comment.ObjId);
                    if (order == null)
                        break;

                    if (parentComment != null && parentComment.Customer != null && parentComment.Customer.HasRoleAction(RoleAction.Orders))
                        parentCommentNotification = new OrderCommentAnswerNotification(order, _author, comment);

                    commentMailTpl = new OrderCommentAddedMailTemplate(comment.Name, comment.CustomerId.ToString(), comment.Text, order.OrderID.ToString(), order.Number);

                    if (order.Manager != null && order.Manager.Customer != null && order.Manager.Customer.HasRoleAction(RoleAction.Orders))
                    {
                        customerIds.Add(order.Manager.CustomerId);
                        commentNotification = new OrderCommentNotification(order, _author, comment);

                        if (!emailsDict.ContainsKey(order.Manager.CustomerId))
                            emailsDict.Add(order.Manager.CustomerId, order.Manager.Email);
                    }

                    break;

                case AdminCommentType.Customer:
                    var customer = CustomerService.GetCustomer(comment.ObjId);
                    if (customer == null)
                        break;

                    if (parentComment != null && parentComment.Customer != null && parentComment.Customer.HasRoleAction(RoleAction.Customers))
                        parentCommentNotification = new CustomerCommentAnswerNotification(customer, _author, comment);

                    commentMailTpl = new CustomerCommentAddedMailTemplate(comment.Name, comment.CustomerId.ToString(), comment.Text, customer.Id.ToString(), customer.GetFullName());

                    if (customer.Manager != null && customer.Manager.Customer != null && customer.Manager.Customer.HasRoleAction(RoleAction.Orders))
                    {
                        customerIds.Add(customer.Manager.CustomerId);
                        commentNotification = new CustomerCommentNotification(customer, _author, comment);

                        if (!emailsDict.ContainsKey(customer.Manager.CustomerId))
                            emailsDict.Add(customer.Manager.CustomerId, customer.Manager.Email);
                    }

                    break;

                default:
                    if (parentComment != null && parentComment.Customer != null)
                        parentCommentNotification = new AdminCommentAnswerNotification(comment, _author);
                    break;
            }

            var notificationsHandler = new AdminNotificationsHandler();

            if (parentCommentNotification != null && parentComment.Customer.Id != _author.Id)
            {
                notificationsHandler.NotifyCustomers(parentCommentNotification, parentComment.Customer.Id);

                if (!emailsDict.ContainsKey(parentComment.Customer.Id))
                    emailsDict.Add(parentComment.Customer.Id, parentComment.Customer.EMail);

                // не оповещать о комментарии, если отправлено уведомление об ответе на комментарий
                customerIds.RemoveAll(x => x == parentComment.CustomerId.Value);
            }

            if (commentNotification != null)
            {
                notificationsHandler.NotifyCustomers(commentNotification, customerIds.Where(x => x != _author.Id).ToArray());
            }

            if (commentMailTpl != null)
            {
                commentMailTpl.BuildMail();
                foreach (var customerId in emailsDict.Keys.Where(x => x != _author.Id))
                {
                    SendMail.SendMailNow(customerId, emailsDict[customerId], commentMailTpl.Subject, commentMailTpl.Body, true);
                }
            }
        }
    }
}
