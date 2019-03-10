using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class WalletOneCheckoutPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string MerchantId
        {
            get { return Parameters.ElementOrDefault(WalletOneCheckoutTemplate.MerchantId); }
            set { Parameters.TryAddValue(WalletOneCheckoutTemplate.MerchantId, value.DefaultOrEmpty()); }
        }

        public string SecretKey
        {
            get { return Parameters.ElementOrDefault(WalletOneCheckoutTemplate.SecretKey); }
            set { Parameters.TryAddValue(WalletOneCheckoutTemplate.SecretKey, value.DefaultOrEmpty()); }
        }

        public string PayWaysEnabled
        {
            get { return Parameters.ElementOrDefault(WalletOneCheckoutTemplate.PayWaysEnabled); }
            set { Parameters.TryAddValue(WalletOneCheckoutTemplate.PayWaysEnabled, value.DefaultOrEmpty()); }
        }

        public string PayWaysDisabled
        {
            get { return Parameters.ElementOrDefault(WalletOneCheckoutTemplate.PayWaysDisabled); }
            set { Parameters.TryAddValue(WalletOneCheckoutTemplate.PayWaysDisabled, value.DefaultOrEmpty()); }
        }

        public bool SendReceiptData
        {
            get { return Parameters.ElementOrDefault(WalletOneCheckoutTemplate.SendReceiptData).TryParseBool(); }
            set { Parameters.TryAddValue(WalletOneCheckoutTemplate.SendReceiptData, value.ToString()); }
        }

        public override Tuple<string, string> Instruction
        {
            get { return new Tuple<string, string>("http://www.advantshop.net/help/pages/connect-wallet-one", "Инструкция. Подключение платежного модуля \"Единая касса\" Wallet one"); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MerchantId) ||
                string.IsNullOrWhiteSpace(SecretKey))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
