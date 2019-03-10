namespace AdvantShop.Payment
{
    public interface ICreditPaymentMethod
    {
        float MinimumPrice { get;  }
        float FirstPayment { get;  }
        int PaymentMethodId { get; }
    }
}