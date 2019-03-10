using System;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using VkNet.Enums;
using VkNet.Model;

namespace AdvantShop.Core.Services.Vk
{
    public class VkMessage
    {
        /// <summary>Идентификатор в vk</summary>
        public long? MessageId { get; set; }

        /// <summary>
        /// Идентификатор автора сообщения (для исходящего сообщения — идентификатор получателя).
        /// </summary>
        public long? UserId { get; set; }
        
        /// <summary>
        /// Дата отправки сообщения.
        /// </summary>
        public DateTime? Date { get; set; }
        
        /// <summary>Заголовок сообщения или беседы.</summary>
        public string Title { get; set; }

        /// <summary>
        /// Received, Sended
        /// </summary>
        public VkMessageType Type { get; set; }

        /// <summary>
        /// Unreaded, Readed
        /// </summary>
        public string ReadState { get; set; }

        /// <summary>Текст сообщения.</summary>
        public string Body { get; set; }
        
        /// <summary>
        /// Идентификатор автора сообщения.
        /// </summary>
        public long? FromId { get; set; }
       
        /// <summary>
        /// Идентификатор беседы.
        /// </summary>
        public long? ChatId { get; set; }

        public Offer Offer { get; set; }

        public VkMessage()
        {
            
        }

        public VkMessage(Message message)
        {
            MessageId = message.Id;
            UserId = message.UserId;
            Date = message.Date;
            Body = message.Body;
            ChatId = message.ChatId;

            FromId = 
                message.Type != null && message.Type == MessageType.Sended 
                        ? -SettingsVk.Group.Id 
                        : message.FromId;
            Type =
                message.Type != null
                    ? (message.Type == MessageType.Received ? VkMessageType.Received : VkMessageType.Sended)
                    : VkMessageType.Other;
            ReadState = message.ReadState != null ? message.ReadState.ToString() : "";

            if (message.Attachments != null && message.Attachments.Count > 0)
            {
                foreach (var attachment in message.Attachments.Where(x => x.Type == typeof(Market)))
                {
                    var market = (Market) attachment.Instance;
                    Body = "Товар: " + market.Title + "<br>\r\n" + Body;
                    Offer = GetOffer(market);
                }
            }
        }

        private Offer GetOffer(Market market)
        {
            var moduleType = AttachedModules.GetModules<IVkProduct>().FirstOrDefault();
            if (moduleType != null)
            {
                var module = (IVkProduct)Activator.CreateInstance(moduleType);

                var productId = module.GetProductIdByMarketId(market.Id);
                if (productId != 0)
                {
                    var p = ProductService.GetProduct(productId);
                    if (p != null && p.Offers != null && p.Offers.Count > 0)
                        return p.Offers[0];
                }
            }

            var title = market.Title.ToLower();
            var index = title.IndexOf("арт.", StringComparison.Ordinal);
            if (index >= 0)
            {
                var artno = market.Title.Substring(index + 4).Trim();

                var o = OfferService.GetOffer(artno);
                if (o != null)
                    return o;

                var p = ProductService.GetProduct(artno);
                if (p != null && p.Offers != null && p.Offers.Count > 0)
                    return p.Offers[0];
            }

            return null;
        }
    }

    public enum VkMessageType
    {
        Received,
        Sended,
        Other
    }
}
