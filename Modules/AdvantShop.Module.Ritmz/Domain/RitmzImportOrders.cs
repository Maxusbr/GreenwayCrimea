//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Linq;

using AdvantShop.Orders;

namespace AdvantShop.Modules
{
    public class RitmzImportOrders
    {
        private static string _moduleName = "Ritmz";
        // it work with using file to mimimaze memory used
        private enum RitmZErrors
        {
            StartTimeInvalid,
            EndTimeInvalid
        }

        //· подтвержден;
        //· отменен;
        //· ожидается оплата;
        //· согласуется наличие;
        //· не подтвержден;
        //· отложен;
        //· доставлен;
        //· доставка;
        //· возврат;
        //· частичный возврат;
        //· полный;
        //· ожидается поставка;
        //· отправлен почтой.

        public static void Import(DateTime? start, DateTime? end, string exportWay)
        {
            var listErr = new List<RitmZErrors>();
            if (start == null) listErr.Add(RitmZErrors.StartTimeInvalid);
            if (end == null) listErr.Add(RitmZErrors.EndTimeInvalid);

            if (listErr.Count > 0)
            {
                WriteError(listErr, exportWay);
                return;
            }

            GetOrders(start, end);
        }

        private static void WriteError(List<RitmZErrors> list, string exportWay)
        {
            using (var fs = new FileStream(exportWay, FileMode.Create))
            using (var writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                if (list.Count > 0)
                {
                    writer.WriteLine("<errors>");
                    for (int i = 0; i < list.Count; i++)
                    {
                        writer.WriteLine(string.Format("<error>{0}</error>", GetErrorString(list[i])));
                    }
                    writer.WriteLine("</errors>");
                }
            }
        }

        private static string GetErrorString(RitmZErrors item)
        {
            switch (item)
            {
                case RitmZErrors.StartTimeInvalid:
                    return "Не указана дата начала выгрузки";
                case RitmZErrors.EndTimeInvalid:
                    return "Не указана дата окончания выгрузки";
                default:
                    return string.Empty;
            }
        }
        
        public static void WriteToResponce(System.Web.HttpResponse httpResponse, string exportWay)
        {
            using (var fs = new FileStream(exportWay, FileMode.Open))
            using (var reader = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    httpResponse.Write(line);
                }
            }
        }

        public static void GetOrders(DateTime? start, DateTime? end)
        {

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            var endPointAddr = new EndpointAddress("http://cc.Ritm-Z.com:8008/RitmZ_GM82/ws/GetOrders.1cws");

            var getOrders = new RitmZOrders.WSGetOrdersPortTypeClient(binding, endPointAddr);

            getOrders.ClientCredentials.UserName.UserName = ModuleSettingsProvider.GetSettingValue<string>("RitmzLogin", _moduleName);
            getOrders.ClientCredentials.UserName.Password = ModuleSettingsProvider.GetSettingValue<string>("RitmzPassword", _moduleName);
            if (start == null || end == null)
                return;
            var ritmzOrders = getOrders.ПолучитьЗаказы(start.Value.ToString(), end.Value.ToString());
            if (ritmzOrders.orders != null)
            {
                for (int i = 0; i < ritmzOrders.orders.Length; ++i)
                {
                    var order = OrderService.GetOrder(ModulesRepository.ConvertTo<int>(ritmzOrders.orders[i].id));
                    if (order != null)
                    {
                        var customerName = ritmzOrders.orders[i].c_name.Split(new[] { " " },
                                                                              StringSplitOptions.RemoveEmptyEntries);
                        order.OrderCustomer.FirstName = customerName[0];
                        order.OrderCustomer.LastName = customerName.Length > 1 ? customerName[0] : string.Empty;
                        if (order.OrderCustomer != null)
                        {
                            order.OrderCustomer.Address = ritmzOrders.orders[i].c_address + " " + ritmzOrders.orders[i].c_contacts;
                        }
                        else
                        {
                            order.OrderCustomer = new OrderCustomer
                            {
                                Address = ritmzOrders.orders[i].c_address + " " + ritmzOrders.orders[i].c_contacts
                            };
                        }
                        order.Sum = ModulesRepository.ConvertTo<float>(ritmzOrders.orders[i].incl_deliv_sum.Replace(".", ","));
                        var oldStatus = order.OrderStatus;
                        order.OrderStatus = OrderService.GetOrderStatusByName(ritmzOrders.orders[i].o_state);
                        order.CustomerComment = ritmzOrders.orders[i].descriptions;
                        order.OrderCustomer.Email = ritmzOrders.orders[i].email;
                        foreach (var ritmzOrderItem in ritmzOrders.orders[i].items)
                        {
                            OrderItem orderItem = null;
                            orderItem = order.OrderItems.FirstOrDefault(item => item.ArtNo == ritmzOrderItem.id);
                            if(orderItem!= null)
                            {
                                orderItem.Amount = ModulesRepository.ConvertTo<int>(ritmzOrderItem.quantity);
                                orderItem.Price = ModulesRepository.ConvertTo<float>(ritmzOrderItem.price);
                            }
                        }

                        OrderService.UpdateOrderMain(order);
                        OrderService.AddUpdateOrderItems(order.OrderItems, order.OrderID);

                        if (oldStatus != order.OrderStatus)
                        {
                            OrderService.UpdateStatusComment(order.OrderID, string.Empty);
                            // TODO: Why not use OrderService.ChangeOrderStatus ?
                            //ModulesRenderer.OrderChangeStatus(order.OrderID);
                        }

                        ModulesRenderer.OrderUpdated(order.OrderID);
                    }
                }
            }
        }
    }
}