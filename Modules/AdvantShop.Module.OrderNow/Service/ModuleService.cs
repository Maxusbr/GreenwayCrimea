using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using AdvantShop.Module.OrderNow.ViewModel;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;

namespace AdvantShop.Module.OrderNow.Service
{
    public class ModuleService
    {
        public static bool Query(string query, SqlParameter[] parameters = null)
        {
            try
            {
                if (parameters != null)
                {
                    ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text, parameters);
                }
                else
                {
                    ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return false;
            }
        }

        public static bool Install()
        {
            var install = true;
            install = ModuleSettings.SetDefaultSettings();
            install &= CreateTable();
            return install;
        }

        public static bool Update()
        {
            return CreateTable();
        }

        public static bool UnInstall()
        {
            return true;
        }

        private static bool CreateTable()
        {
            bool created = false;
            if (!ModulesRepository.IsExistsModuleTable("Module", "OrderNowMessages"))
            {
                var createQuery = @"create table Module.OrderNowMessages(
                    ID int identity(1,1) not null,
                    productid int null,
                    Message nvarchar(max),
                    TimeoutMessage nvarchar(max),
                    primary key (ID),
                    foreign key (productid) references Catalog.Product(productid) on update cascade on delete cascade)";
                created = Query(createQuery);
            }
            else
                created = true;
            return created;
        }

        public static AdminTabViewmodel GetTabsContent(string productid)
        {
            try
            {
                var getTabContent = "select * from Module.OrderNowMessages where productid = " + productid;
                return ModulesRepository.Query<AdminTabViewmodel>(getTabContent, null, CommandType.Text).ToList().FirstOrDefault();
            }
            catch
            {
                return new AdminTabViewmodel() { Message = string.Empty, TimeoutMessage = string.Empty, productid = productid.TryParseInt() };
            }
        }

        public static string GetTimeleft()
        {
            DateTime current = DateTime.Now;
            DateTime timeout = DateTime.Parse(DateTime.Now.ToShortDateString() +" "+ ModuleSettings.TimeoutTime+":00");
            if (current < timeout)
            {
                TimeSpan showTime = timeout.Subtract(current);
                return ChooseHoursEnding(showTime.Hours) + ChooseMinutesEnding(showTime.Minutes);
            }
            else
            {
                timeout = timeout.AddDays(1);
                TimeSpan showTime = timeout.Subtract(current);
                return ChooseHoursEnding(showTime.Hours) + ChooseMinutesEnding(showTime.Minutes);
            }
        }

        public static string ChooseHoursEnding(int hours)
        {
            if (hours == 0) return string.Empty;
            if (hours >= 11 && hours <= 20)
            {
                return hours.ToString() + " часов ";
            }
            string hourString = hours.ToString();
            if (hourString.EndsWith("1"))
            {
                return hours.ToString() + " часа ";
            }
            else
            {
                return hours.ToString() + " часов ";
            }

        }

        public static string ChooseMinutesEnding(int minutes)
        {
            if (minutes == 0) return "минуты";
            if (minutes == 1)
            {
                return minutes.ToString() + " минуты";
            }
            if (minutes >= 2 && minutes <= 20)
            {
                return minutes.ToString() + " минут";
            }
            if (minutes.ToString().EndsWith("1"))
            {
                return minutes.ToString() + " минуты";
            }
            return minutes.ToString() + " минут";

        }

        public static string GetMessage(string productid)
        {
            var getMessage = "select Message from Module.OrderNowMessages where productid = " + productid;
            try
            {
                var mes = SQLDataAccess.ExecuteScalar(getMessage, CommandType.Text).ToString();
                return mes;
            }
            catch
            {
                return null;
            }
        }

        public static string GetTimeoutMessage(string productid)
        {
            var getMessage = "select TimeoutMessage from Module.OrderNowMessages where productid = " + productid;
            try
            {
                var mes = SQLDataAccess.ExecuteScalar(getMessage, CommandType.Text).ToString();
                return mes;
            }
            catch
            {
                return null;
            }

        }

        public static string PrepareMessage(bool timeout = false, int productid = -1)
        {
            var cachebreaker = DateTime.Now.ToString().Replace(".", "").Replace("/", "").Replace(" ", "");
            string formattedMessage = "";
            if (ModuleSettings.IconUsed && !timeout)
                formattedMessage += "<table><tr style=\"border-color:transparent;\"><td style=\"border-color:transparent;\"><div " +
                    "style=\"background: url(../modules/ordernow/content/images/icon.png?"+cachebreaker+") no-repeat; " +
                    "width:" + ModuleSettings.IconHeight.ToString() + "px; " +
                    "height:" + ModuleSettings.IconHeight.ToString() + "px; " +
                    "background-size: " + ModuleSettings.IconHeight.ToString() + "px; " +
                    "overflow: hidden;\">" +
                    "</div></td><td style=\"border-color:transparent;\">";
            if (timeout)
            {
                if (productid != -1)
                {
                    var timeoutMessage = GetTimeoutMessage(productid.ToString());
                    if (timeoutMessage != null && !string.IsNullOrWhiteSpace(timeoutMessage))
                        formattedMessage += timeoutMessage;
                    else
                        formattedMessage += ModuleSettings.TimeoutMessage;
                }
                else
                    formattedMessage += ModuleSettings.TimeoutMessage;
            }
            else
            {
                if (productid != -1)
                {
                    var message = GetMessage(productid.ToString());
                    if (message != null && !string.IsNullOrWhiteSpace(message))
                        formattedMessage += message;
                    else formattedMessage += ModuleSettings.Message.TrimStart(' ');

                }
                else
                    formattedMessage += ModuleSettings.Message.TrimStart(' ');
            }
            if (ModuleSettings.IconUsed)
                formattedMessage += "</td></tr></table>";
            formattedMessage = formattedMessage.Replace("#TOMORROW#", "<span class=\"ONtomorrow\">завтра</span>");
            formattedMessage = formattedMessage.Replace("#TODAY#", "<span class=\"ONtoday\">сегодня</span>");
            formattedMessage = formattedMessage.Replace("#2DAYS#", "<span class=\"ON2days\">" + DateTime.Now.AddDays(2).ToShortDateString() + "</span>");
            formattedMessage = formattedMessage.Replace("#3DAYS#", "<span class=\"ON3days\">" + DateTime.Now.AddDays(3).ToShortDateString() + "</span>");
            formattedMessage = formattedMessage.Replace("#NDAYS#", "<span class=\"ONndays\">" + DateTime.Now.AddDays(ModuleSettings.Ndays).ToShortDateString() + "</span>");
            formattedMessage = formattedMessage.Replace("#DATE#", "<span class=\"ONdate\">" + GetTimeleft() + "</span>");
            

            return formattedMessage;
        }

        public static void DeleteImage()
        {
            ModuleSettings.IconUsed = false;
            var path = HostingEnvironment.MapPath("~/modules/" + OrderNow.ModuleStringId);
            path += "/content/images/icon.png";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void AddUpdateProductTimeoutMessage(string productid, string message)
        {
            var getMessage = "select TimeoutMessage from Module.OrderNowMessages where productid = " + productid;
            try
            {
                var dbMessage = SQLDataAccess.ExecuteScalar(getMessage, CommandType.Text).ToString();
                var update = "update Module.OrderNowMessages set TimeoutMessage = '" + message + "' where productid = " + productid;
                SQLDataAccess.ExecuteNonQuery(update, CommandType.Text);
            }
            catch
            {
                var insertNew = "insert into Module.OrderNowMessages (productid, Message, TimeoutMessage) " +
                    "values (" + productid + ", ' ','" + message + "')";
                SQLDataAccess.ExecuteNonQuery(insertNew, CommandType.Text);
            }
        }

        public static void AddUpdateProductMessage(string productid, string message)
        {
            var getMessage = "select Message from Module.OrderNowMessages where productid = " + productid;
            try
            {
                var dbMessage = SQLDataAccess.ExecuteScalar(getMessage, CommandType.Text).ToString();
                var update = "update Module.OrderNowMessages set Message = '" + message + "' where productid = " + productid;
                SQLDataAccess.ExecuteNonQuery(update, CommandType.Text);
            }
            catch
            {
                var insertNew = "insert into Module.OrderNowMessages (productid, Message, TimeoutMessage) " +
                    "values (" + productid + ", '" + message + "' ,' ')";
                SQLDataAccess.ExecuteNonQuery(insertNew, CommandType.Text);
            }
        }

        public static string WriteMessage(string productid)
        {
            var message = GetMessage(productid);
            return message == null ? string.Empty : message;
        }

        public static string WriteTimeoutMessage(string productid)
        {
            var timeoutMessage = GetTimeoutMessage(productid);
            return timeoutMessage == null ? string.Empty : timeoutMessage;
        }

        public static bool ProcessMessage(string productid, string message)
        {
            AddUpdateProductMessage(productid, message);
            return true;
        }

        public static bool ProcessTimeoutMessage(string productid, string message)
        {
            AddUpdateProductTimeoutMessage(productid, message);
            return true;
        }
    }
}
