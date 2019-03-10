using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using VkNet;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.RequestParams;

namespace AdvantShop.Core.Services.Vk
{
    public class VkApiService
    {
        #region Auth

        public VkApi Auth()
        {
            if (string.IsNullOrEmpty(SettingsVk.TokenUser) || SettingsVk.UserId == 0)
                return null;

            try
            {
                var vk = new VkApi();
                vk.Authorize(new ApiAuthParams() {AccessToken = SettingsVk.TokenUser, UserId = SettingsVk.UserId});

                return vk;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);

                var errors = SettingsVk.TokenUserErrorCount;
                if (errors > 5)
                {
                    SettingsVk.TokenUserErrorCount = 0;
                    SettingsVk.TokenUser = null;
                    SettingsVk.UserId = 0;
                }
                else
                {
                    SettingsVk.TokenUserErrorCount = errors + 1;
                }
            }

            return null;
        }

        public VkApi AuthGroup()
        {
            if (string.IsNullOrEmpty(SettingsVk.TokenGroup) || SettingsVk.UserId == 0)
                return null;

            try
            {
                var vk = new VkApi();
                vk.Authorize(new ApiAuthParams() { AccessToken = SettingsVk.TokenGroup, UserId = SettingsVk.UserId });

                return vk;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);

                var errors = SettingsVk.TokenGroupErrorCount;
                if (errors > 5)
                {
                    SettingsVk.TokenGroupErrorCount = 0;
                    SettingsVk.TokenGroup = null;
                    SettingsVk.UserId = 0;
                }
                else
                {
                    SettingsVk.TokenGroupErrorCount = errors + 1;
                }
            }

            return null;
        }

        #endregion

        public List<VkGroup> GetUserGroups()
        {
            var vk = Auth();
            if (vk == null)
                return null;

            var groups = vk.Groups.Get(new GroupsGetParams() { Filter = GroupsFilters.Moderator});
            if (groups != null && groups.Count > 0)
            {
                return
                    vk.Groups.GetById(groups.Select(x => x.Id.ToString()), null, GroupsFields.Description)
                        .Select(x => new VkGroup(x))
                        .ToList();
            }

            return null;
        }

        /// <summary>
        /// Активность интеграции с Vk
        /// </summary>
        /// <returns></returns>
        public bool IsVkActive()
        {
            return !string.IsNullOrEmpty(SettingsVk.TokenGroup) && SettingsVk.UserId != 0;
        }


        public List<VkMessage> GetLastMessagesByApi()
        {
            var vk = AuthGroup();
            
            var messages = GetReceivedGroupMessages(vk);
            if (messages.Count > 0)
            {
                var userIds = messages.Where(x => x.UserId != null).Select(x => x.UserId.Value).Distinct();
                var users = GetUsersInfo(userIds);

                SaveUsers(users, messages);

                VkService.AddMessages(messages);
            }
            
            var sendedMessages = GetSendedGroupMessages(vk);
            if (sendedMessages.Count > 0)
            {
                VkService.AddMessages(sendedMessages);
            }

            return messages;
        }

        /// <summary>
        /// Получаем входящие сообщения группы
        /// </summary>
        private List<VkMessage> GetReceivedGroupMessages(VkApi vk)
        {
            if (vk == null)
                throw new BlException("VkService.GetMessages авторизация не прошла");

            var messages = new List<VkMessage>();

            var lastMessageId = SettingsVk.LastMessageId;
            var isFirstTime = lastMessageId == null;
            var iterator = 0;

            while (true)
            {
                var vkMessages =
                    vk.Messages.Get(new MessagesGetParams()
                    {
                        Filters = MessagesFilter.All,
                        Count = 200,
                        LastMessageId = lastMessageId
                    });

                if (vkMessages == null || vkMessages.Messages.Count == 0)
                    break;

                lastMessageId = vkMessages.Messages.Max(x => x.Id);

                if (!isFirstTime)
                {
                    var msgs =
                        vkMessages.Messages.Where(x => x.IsDeleted == null || !x.IsDeleted.Value)
                            .Select(x => new VkMessage(x));

                    messages.AddRange(msgs);
                }

                if (vkMessages.Messages.Count < 200)
                    break;

                if (iterator > 300)
                    throw new BlException("VkService.GetMessages зациклился или очень много сообщений");

                iterator++;
            }

            SettingsVk.LastMessageId = lastMessageId ?? 0;

            return messages;
        }

        /// <summary>
        /// Получаем отосланные сообщения группы
        /// </summary>
        private List<VkMessage> GetSendedGroupMessages(VkApi vk)
        {
            if (vk == null)
                throw new BlException("VkService.GetSendedMessages авторизация не прошла");

            var messages = new List<VkMessage>();

            var lastMessageId = SettingsVk.LastSendedMessageId;
            var isFirstTime = lastMessageId == null;

            while (true)
            {
                var vkMessages =
                    vk.Messages.Get(new MessagesGetParams()
                    {
                        Filters = MessagesFilter.All,
                        Count = 200,
                        LastMessageId = lastMessageId,
                        Out = MessageType.Sended
                    });

                if (vkMessages == null || vkMessages.Messages.Count == 0)
                    break;

                if (!isFirstTime)
                {
                    var msgs =
                        vkMessages.Messages.Where(x => x.IsDeleted == null || !x.IsDeleted.Value)
                            .Select(x => new VkMessage(x));

                    messages.AddRange(msgs);
                }

                lastMessageId = vkMessages.Messages.Max(x => x.Id);

                if (vkMessages.Messages.Count < 200)
                    break;
            }

            SettingsVk.LastSendedMessageId = lastMessageId;

            return messages;
        }

        public List<VkUser> GetUsersInfo(IEnumerable<long> userIds)
        {
            var vk = new VkApi();

            var users =
                vk.Users.Get(userIds,
                    ProfileFields.FirstName | ProfileFields.LastName | ProfileFields.BirthDate | ProfileFields.Photo100 | ProfileFields.Sex | ProfileFields.ScreenName | ProfileFields.IsHiddenFromFeed,
                    NameCase.Nom, true)
                    .Select(x => new VkUser(x)).ToList();

            return users;
        }

        public List<VkUser> GetUsersInfo(IEnumerable<string> userIds)
        {
            var vk = new VkApi();

            var users =
                vk.Users.Get(userIds,
                    ProfileFields.FirstName | ProfileFields.LastName | ProfileFields.BirthDate | ProfileFields.Photo100 | ProfileFields.Sex | ProfileFields.ScreenName | ProfileFields.IsHiddenFromFeed,
                    NameCase.Nom, true)
                    .Select(x => new VkUser(x)).ToList();

            return users;
        }

        private void SaveUsers(List<VkUser> users, List<VkMessage> messages)
        {
            if (users == null || users.Count == 0)
                return;

            foreach (var user in users)
            {
                var u = VkService.GetUser(user.Id);
                if (u != null)
                {
                    // Если все лиды закрыты, то создаем новый
                    var customerLeads = LeadService.GetLeadsByCustomer(u.CustomerId);
                    var hasNoClosedLeads = customerLeads.Any(x => x.DealStatusId != SettingsCrm.FinalDealStatusId);
                    if (!hasNoClosedLeads)
                    {
                        // Если все заказы закрыты, то создаем лид
                        var hasNoClosedOrders = OrderService.GetCustomerOrderHistory(u.CustomerId).Any(x => !OrderStatusService.GetOrderStatus(x.StatusID).IsCompleted);

                        if (!hasNoClosedOrders || customerLeads.Count == 0)
                        {
                            CreateLead(CustomerService.GetCustomer(u.CustomerId), u, messages);
                            continue;
                        }
                    }

                    // Если пришло сообщение к товару и открытых лидов с таким товаром нет, то создаем новый
                    var msg = messages.FirstOrDefault(x => x.Offer != null);
                    if (msg != null)
                    {
                        var hasNoOpenLeadWithSameOffer =
                            customerLeads.Any(
                                x =>
                                    x.DealStatusId != SettingsCrm.FinalDealStatusId && 
                                    x.LeadItems != null && 
                                    x.LeadItems.Count == 1 &&
                                    x.LeadItems.Find(i => i.ArtNo == msg.Offer.ArtNo) != null);
                        if (!hasNoOpenLeadWithSameOffer)
                        {
                            CreateLead(CustomerService.GetCustomer(u.CustomerId), u, messages);
                            continue;
                        }
                    }

                    BizProcessExecuter.MessageReply(CustomerService.GetCustomer(u.CustomerId));
                    continue;
                }

                try
                {
                    var phone = user.MobilePhone ?? user.HomePhone;

                    // add customer
                    var customer = new Customer(CustomerGroupService.DefaultCustomerGroup)
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Phone = phone,
                        StandardPhone = !string.IsNullOrEmpty(phone) ? StringHelper.ConvertToStandardPhone(phone) : null,
                    };
                    CustomerService.InsertNewCustomer(customer);

                    // add vk user
                    user.CustomerId = customer.Id;
                    VkService.AddUser(user);

                    // add lead
                    CreateLead(customer, user, messages);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
            }
        }

        private void CreateLead(Customer customer, VkUser user, List<VkMessage> messages)
        {
            var source = OrderSourceService.GetOrderSource(OrderType.Vk);
            var customerMessages = messages.Where(x => x.UserId == user.Id).ToList();
            var lastMessage = customerMessages.Count > 0 ? customerMessages.OrderByDescending(x => x.Date).FirstOrDefault() : null;

            var lead = new Lead()
            {
                CustomerId = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                Comment = String.Join("<br>\r\n ", customerMessages.OrderBy(x => x.Date).Select(x => x.Body)),
                OrderSourceId = source.Id,
                CreatedDate = lastMessage != null && lastMessage.Date != null ? lastMessage.Date.Value.AddSeconds(-1) : DateTime.Now,
                LeadItems = new List<LeadItem>()
            };

            foreach (var offer in customerMessages.Where(x => x.Offer != null).Select(x => x.Offer))
            {
                lead.LeadItems.Add(new LeadItem(offer, 1));
            }

            LeadService.AddLead(lead, true);
        }


        /// <summary>
        /// Посылает личное сообщение
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, которому отправляется сообщение</param>
        /// <param name="message">Сообщение</param>
        /// <returns>Идентификатор отправленного сообщения</returns>
        public long SendMessageByGroup(long userId, string message)
        {
            try
            {
                var vk = AuthGroup();
                if (vk == null)
                    throw new BlException("VkService.SendMessage авторизация не прошла");

                var groupId = -SettingsVk.Group.Id;

                var messageId = vk.Messages.Send(new MessagesSendParams()
                {
                    Message = message ?? "",
                    UserId = userId,
                    PeerId = groupId,
                });
                
                // add to db
                VkService.AddMessage(new VkMessage()
                {
                    MessageId = messageId,
                    UserId = userId,
                    FromId = groupId,
                    Body = message ?? "",
                    Type = VkMessageType.Sended,
                });
                
                return messageId;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return 0;
        }
    }
}
