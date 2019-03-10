namespace AdvantShop.Models.Common
{

    public partial class FaviconModel : BaseModel
    {
        public bool Visible { get; set; }
        public bool GetOnlyImage { get; set; }
        public string ImgSource { get; set; }
        public string CssClassImage { get; set; }
        public bool ForAdmin { get; set; }

        public FaviconModel()
        {
            GetOnlyImage = false;
            Visible = true;
        }

        public string Html { get; set; }
    }
}