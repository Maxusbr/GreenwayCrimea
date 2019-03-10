using System;
using AdvantShop.Localization;
using VkNet.Model;

namespace AdvantShop.Core.Services.Vk
{
    public class VkUser
    {
        /// <summary>Идентификатор пользователя.</summary>
        public long Id { get; set; }

        /// <summary>Имя пользователя.</summary>
        public string FirstName { get; set; }

        /// <summary>Фамилия пользователя.</summary>
        public string LastName { get; set; }

        /// <summary>
        /// Возвращается при вызове без access_token, если пользователь установил настройку «Кому в интернете видна моя страница» — «Только пользователям ВКонтакте».
        /// Обратите внимание, в этом случае дополнительные поля fields не возвращаются.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>Пол пользователя.</summary>
        public string Sex { get; set; }

        /// <summary>
        /// Дата рождения пользователя. Возвращается в формате DD.MM.YYYY или DD.MM (если год рождения скрыт).
        /// Если дата рождения скрыта целиком, поле отсутствует в ответе.
        /// </summary>
        public string BirthDate { get; set; }
        
        /// <summary>
        /// Номер мобильного телефона (если нет записи или скрыт, то null).
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// Номер домашнего телефона (если нет записи или скрыт, то null).
        /// </summary>
        public string HomePhone { get; set; }
        
        
        /// <summary>Короткое имя (поддомен) страницы пользователя.</summary>
        public string ScreenName { get; set; }
        
        /// <summary>
        /// url квадратной фотографии пользователя, имеющей ширину 100 пикселей. В случае отсутствия у пользователя фотографии возвращается http://vk.com/images/camera_b.gif.
        /// </summary>
        public string Photo100 { get; set; }


        public Guid CustomerId { get; set; }

        public VkUser()
        {
        }

        public VkUser(User x)
        {
            Id = x.Id;
            FirstName = x.FirstName;
            LastName = x.LastName;
            BirthDate = x.BirthDate;
            Photo100 = x.Photo100 != null ? x.Photo100.ToString() : null;
            MobilePhone = x.MobilePhone;
            HomePhone = x.HomePhone;
            Sex = x.Sex.ToString();
            ScreenName = x.ScreenName;
            Hidden = x.Hidden;
        }
    }

    public class VkUserMessage
    {
        public long UserId { get; set; }
        public long? FromId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ScreenName { get; set; }
        public string Photo100 { get; set; }
        public DateTime Date { get; set; }
        public string DateFormatted { get { return Date.ToString("dd MMMM hh:mm"); } }
        public VkMessageType Type { get; set; }
        public string Body { get; set; }
    }
}
