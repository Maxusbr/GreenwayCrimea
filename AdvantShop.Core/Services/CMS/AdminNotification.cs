using System;
using System.Text;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Orders;

namespace AdvantShop.CMS
{
    public class AdminNotification
    {
        public AdminNotification()
        {
            DateCreated = DateTime.Now;
            InNewTab = SettingsNotifications.WebNotificationInNewTab;
        }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        public string Tag { get; set; }
        public AdminNotificationType Type { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string IconPath { get; set; }
        public bool InNewTab { get; set; }
        public object Data { get; set; }
    }

    /// <summary>
    /// Notification with avatar of person who sent the notification
    /// </summary>
    public class PersonAdminNotification : AdminNotification
    {
        public PersonAdminNotification(Customer sender)
        {
            Type = AdminNotificationType.Notify;
            IconPath = sender != null && sender.Avatar.IsNotEmpty()
                ? FoldersHelper.GetPath(FolderType.Avatar, sender.Avatar, false)
                : UrlService.GetAdminStaticUrl() + "images/no-avatar.jpg";
        }
    }

    #region AdminCommentNotifications

    public class AdminCommentNotification : PersonAdminNotification
    {
        public AdminCommentNotification(AdminComment comment, Customer author) : base(author)
        {
            Title = LocalizationService.GetResource("Core.Services.CMS.AdminCommentNotification.Title");
            comment.Text = comment.Text.Replace("\n", " ");
            Body = string.Format("{0}", comment.Text.Length > 250 ? comment.Text.Substring(0, 250) + " ..." : comment.Text);
            if (comment.ObjUrl.IsNotEmpty())
            {
                if (comment.ObjUrl.Contains("#") && comment.ObjUrl.Contains("modal") && !comment.ObjUrl.Contains("modalShow"))
                    comment.ObjUrl += "&modalShow=true";
                Data = new
                {
                    Url = comment.ObjUrl
                };
            }
        }
    }

    public class AdminCommentAnswerNotification : AdminCommentNotification
    {
        public AdminCommentAnswerNotification(AdminComment comment, Customer author) : base(comment, author)
        {
            Title = LocalizationService.GetResource("Core.Services.CMS.AdminCommentAnswerNotification.Title");
        }
    }

    #endregion

    #region OrderNotifications

    public class OrderNotification : AdminNotification
    {
        public OrderNotification(Order order)
        {
            Type = AdminNotificationType.Notify;
            Data = new
            {
                Url = UrlService.GetAdminUrl("orders/edit/" + order.OrderID)
            };
        }
    }

    public class OrderAddedNotification : OrderNotification
    {
        public OrderAddedNotification(Order order) : base(order)
        {
            Title = LocalizationService.GetResource("Core.Services.CMS.OrderAddedNotification.Title");
            Body = LocalizationService.GetResourceFormat("Core.Services.CMS.OrderAddedNotification.Body", order.Number);
        }
    }

    #region OrderCommentNotifications

    public class OrderCommentNotification : AdminCommentNotification
    {
        public OrderCommentNotification(Order order, Customer author, AdminComment comment) : base(comment, author)
        {
            Title = LocalizationService.GetResourceFormat("Core.Services.CMS.OrderCommentNotification.Title", order.Number);
            if (Data == null)
            {
                Data = new { Url = UrlService.GetAdminUrl("orders/edit/" + order.OrderID) };
            }
        }
    }

    public class OrderCommentAnswerNotification : OrderCommentNotification
    {
        public OrderCommentAnswerNotification(Order order, Customer author, AdminComment comment) : base(order, author, comment)
        {
            Title = LocalizationService.GetResourceFormat("Core.Services.CMS.OrderCommentAnswerNotification.Title", order.Number);
        }
    }

    #endregion
    #endregion

    #region CustomerNotifications

    public class CustomerNotification : AdminNotification
    {
        public CustomerNotification(Customer customer)
        {
            Type = AdminNotificationType.Notify;
            Data = new
            {
                Url = UrlService.GetAdminUrl("customers/edit/" + customer.Id)
            };
        }
    }

    #region CustomerCommentNotifications

    public class CustomerCommentNotification : AdminCommentNotification
    {
        public CustomerCommentNotification(Customer customer, Customer author, AdminComment comment) : base(comment, author)
        {
            Title = LocalizationService.GetResourceFormat("Core.Services.CMS.CustomerCommentNotification.Title", customer.GetFullName());
            if (Data == null)
            {
                Data = new { Url = UrlService.GetAdminUrl("customers/edit/" + customer.Id) };
            }
        }
    }

    public class CustomerCommentAnswerNotification : CustomerCommentNotification
    {
        public CustomerCommentAnswerNotification(Customer customer, Customer author, AdminComment comment) : base(customer, author, comment)
        {
            Title = LocalizationService.GetResourceFormat("Core.Services.CMS.CustomerCommentAnswerNotification.Title", customer.GetFullName());
        }
    }

    #endregion
    #endregion

    #region Task Notifications

    public class TaskNotification : PersonAdminNotification
    {
        public TaskNotification(Task task, Customer modifier) : base(modifier)
        {
            Data = new
            {
                Url = UrlService.GetAdminUrl("tasks/#?modalShow=true&modal=" + task.Id)
            };
        }
    }

    public class OnSetTaskNotification : TaskNotification
    {
        public OnSetTaskNotification(Task task, Customer modifier) : base(task, modifier)
        {
            Title = LocalizationService.GetResourceFormat("Core.Services.CMS.OnSetTaskNotification.Title", task.Id);
            Body = task.Name;
        }
    }

    public class OnTaskChangeNotification : TaskNotification
    {
        public OnTaskChangeNotification(Task task, Customer modifier, string field, string oldValue, string newValue) : base(task, modifier)
        {
            Title = LocalizationService.GetResourceFormat("Core.Services.CMS.OnTaskChangeNotification.Title", task.Id);
            var sb = new StringBuilder();
            sb.Append(task.Name);
            if (field.IsNotEmpty())
                sb.AppendFormat("\n{0}", field);
            if (oldValue.IsNotEmpty() || newValue.IsNotEmpty())
            {
                if (oldValue.IsNotEmpty() && newValue.IsNotEmpty())
                    sb.AppendFormat("\n{0} -> {1}", oldValue, newValue);
                else
                    sb.AppendFormat("\n{0}", oldValue.IsNullOrEmpty() ? newValue : oldValue);
            }
            Body = sb.ToString();
            Data = new
            {
                Url = UrlService.GetAdminUrl("tasks/#?modalShow=true&modal=" + task.Id)
            };
        }
    }

    public class OnTaskDeletedNotification : TaskNotification
    {
        public OnTaskDeletedNotification(Task task, Customer modifier) : base(task, modifier)
        {
            Title = LocalizationService.GetResourceFormat("Core.Services.CMS.OnTaskDeletedNotification.Title", task.Id);
            Body = task.Name;
            Data = null;
        }
    }

    public class OnTaskAcceptedNotification : TaskNotification
    {
        public OnTaskAcceptedNotification(Task task, Customer modifier) : base(task, modifier)
        {
            Title = LocalizationService.GetResourceFormat("Core.Services.CMS.OnTaskAcceptedNotification.Title", task.Id);
            Body = task.Name;
        }
    }

    #region TaskCommentNotifications

    public class OnTaskCommentNotification : AdminCommentNotification
    {
        public OnTaskCommentNotification(Task task, Customer author, AdminComment comment) : base(comment, author)
        {
            Title = LocalizationService.GetResourceFormat("Core.Services.CMS.OnTaskCommentNotification.Title", task.Id);
            if (Data == null)
            {
                Data = new { Url = UrlService.GetAdminUrl("tasks/#?modalShow=true&modal=" + task.Id) };
            }
        }
    }

    public class OnTaskCommentAnswerNotification : OnTaskCommentNotification
    {
        public OnTaskCommentAnswerNotification(Task task, Customer author, AdminComment comment) : base(task, author, comment)
        {
            Title = LocalizationService.GetResourceFormat("Core.Services.CMS.OnTaskCommentAnswerNotification.Title", task.Id);
        }
    }

    #endregion

    #endregion

    #region LeadNotifications

    public class LeadNotification : AdminNotification
    {
        public LeadNotification(Lead lead)
        {
            Type = AdminNotificationType.Notify;
            Data = new
            {
                Url = UrlService.GetAdminUrl("leads/edit/" + lead.Id)
            };
        }
    }

    public class LeadAddedNotification : LeadNotification
    {
        public LeadAddedNotification(Lead lead) : base(lead)
        {
            Title = LocalizationService.GetResource("Core.Services.CMS.LeadAddedNotification.Title");
            Body = LocalizationService.GetResourceFormat("Core.Services.CMS.LeadAddedNotification.Body", lead.Id);
        }
    }


    #endregion
}