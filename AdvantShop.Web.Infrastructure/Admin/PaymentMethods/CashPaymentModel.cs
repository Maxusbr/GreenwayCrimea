using AdvantShop.Core.Services.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class CashPaymentModel : PaymentMethodAdminModel
    {
        public override string PaymentViewPath { get { return null; } }
    }
}
