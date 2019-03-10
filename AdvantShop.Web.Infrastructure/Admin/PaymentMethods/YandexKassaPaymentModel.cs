using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class YandexKassaPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string ShopId
        {
            get { return Parameters.ElementOrDefault(YandexKassaTemplate.ShopID); }
            set { Parameters.TryAddValue(YandexKassaTemplate.ShopID, value.DefaultOrEmpty()); }
        }

        public string ScId
        {
            get { return Parameters.ElementOrDefault(YandexKassaTemplate.ScID); }
            set { Parameters.TryAddValue(YandexKassaTemplate.ScID, value.DefaultOrEmpty()); }
        }

        public string YaPaymentType
        {
            get { return Parameters.ElementOrDefault(YandexKassaTemplate.YaPaymentType); }
            set { Parameters.TryAddValue(YandexKassaTemplate.YaPaymentType, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> YaPaymentTypes
        {
            get
            {
                var types = new List<SelectListItem>()
                {
                    new SelectListItem() {Text = "Умный платеж (все доступные методы)", Value = ""},
                    new SelectListItem() {Text = "Со счета в Яндекс.Деньгах", Value = "PC"},
                    new SelectListItem() {Text = "С банковской карты", Value = "AC"},
                    new SelectListItem() {Text = "Со счета мобильного телефона", Value = "MC"},
                    new SelectListItem() {Text = "По коду через терминал", Value = "GP"},
                    new SelectListItem() {Text = "Оплата через Сбербанк: оплата по SMS или Сбербанк Онлайн", Value = "SB"},
                    new SelectListItem() {Text = "Оплата через мобильный терминал (mPOS)", Value = "WM"},
                    new SelectListItem() {Text = "Оплата через Альфа-Клик", Value = "AB"},
                    new SelectListItem() {Text = "Оплата через MasterPass", Value = "МА"},
                    new SelectListItem() {Text = "Оплата через Промсвязьбанк", Value = "PB"},
                    new SelectListItem() {Text = "Оплата через QIWI Wallet", Value = "QW"},
                    new SelectListItem() {Text = "Оплата через КупиВкредит (Тинькофф Банк)", Value = "KV"},
                    new SelectListItem() {Text = "Оплата через Доверительный платеж на Куппи.ру", Value = "QP"}
                };

                var type = types.Find(x => x.Value == YaPaymentType);
                if (type != null)
                    type.Selected = true;

                return types;
            }
        }

        public string Password
        {
            get { return Parameters.ElementOrDefault(YandexKassaTemplate.Password); }
            set { Parameters.TryAddValue(YandexKassaTemplate.Password, value.DefaultOrEmpty()); }
        }

        public bool DemoMode
        {
            get { return Parameters.ElementOrDefault(YandexKassaTemplate.DemoMode).TryParseBool(); }
            set { Parameters.TryAddValue(YandexKassaTemplate.DemoMode, value.ToString()); }
        }

        public bool SendReceiptData
        {
            get { return Parameters.ElementOrDefault(YandexKassaTemplate.SendReceiptData).TryParseBool(); }
            set { Parameters.TryAddValue(YandexKassaTemplate.SendReceiptData, value.ToString()); }
        }
        

        public override Tuple<string, string> Instruction
        {
            get { return new Tuple<string, string>("http://www.advantshop.net/help/pages/connect-yandex-kassa", "Инструкция. Подключение платежного модуля \"Касса от Яндекс.Денег\""); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ShopId) ||
                string.IsNullOrWhiteSpace(ScId) ||
                string.IsNullOrWhiteSpace(Password))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
