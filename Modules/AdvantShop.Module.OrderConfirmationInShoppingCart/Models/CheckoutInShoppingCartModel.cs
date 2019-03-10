namespace AdvantShop.Module.CheckoutInShoppingCart.Models
{
    public class CheckoutInShoppingCartModel
    {
        public bool DisplayName { get; set; }
        public bool DisplayEmail { get; set; }
        public bool DisplayPhone { get; set; }
        public bool DisplayComment { get; set; }

        public string LabelName { get; set; }
        public string LabelEmail { get; set; }
        public string LabelPhone { get; set; }
        public string LabelComment { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }

        public string FirstText { get; set; }
        public string FinalText { get; set; }
        public string Error { get; set; }

        public bool IsRequiredName { get; set; }
        public bool IsRequiredEmail { get; set; }
        public bool IsRequiredPhone { get; set; }
        public bool IsRequiredComment { get; set; }
        public bool IsShowUserAgreementText { get; set; }
        public string UserAgreementText { get; set; }
        public bool Agreement { get; set; }
    }
}
