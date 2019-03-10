using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;

namespace AdvantShop.Areas.AdminMobile.Models.Tasks
{
    public class TaskModel
    {
        public int TaskId { get; set; }

        public DateTime DueDate { get; set; }
        public string DueDateFormated
        {
            get { return DueDate.ToString(SettingsMain.ShortDateFormat); }
        }

        public DateTime DateCreated { get; set; }
        public string DateCreatedFormated
        {
            get { return DateCreated.ToString("dd.MM.yy HH:mm"); }
        }

        public string Name { get; set; }

        public int Status { get; set; }
        public string StatusName
        {
            get { return ((ManagerTaskStatus)Status).Localize(); }
        }
    }
}