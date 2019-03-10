using System.Web.Mvc;
using AdvantShop.Models;
using AdvantShop.Payment;
using AdvantShop.Shipping;

namespace AdvantShop
{
    public class BinderConfig
    {
        public static void Regist()
        {
            ModelBinders.Binders.Add(typeof(float), new FractionalNumberModelBinder());
            ModelBinders.Binders.Add(typeof(double), new FractionalNumberModelBinder());
            ModelBinders.Binders.Add(typeof(decimal), new FractionalNumberModelBinder());

            ModelBinders.Binders.Add(typeof(BaseShippingOption), new TypeModelBinder());
            ModelBinders.Binders.Add(typeof(BasePaymentOption), new TypeModelBinder());
        }
    }
}