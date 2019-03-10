namespace AdvantShop.ViewModel.User
{
    public class LoginOpenIdViewModel
    {
        public bool DisplayGoogle { get; set; }
        public bool DisplayYandex { get; set; }
        public bool DisplayFacebook { get; set; }
        public bool DisplayVk { get; set; }
        public bool DisplayMailRu { get; set; }
        public bool DisplayOdnoklassniki { get; set; }
        public string PageToRedirect { get; set; }
    }
}