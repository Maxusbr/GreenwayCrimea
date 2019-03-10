//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.SEO;

namespace Admin.UserControls
{
    public partial class EditMetaFields : System.Web.UI.UserControl
    {
        public string Hint { get; set; }

        public MetaInfo MetaInfo
        {
            get
            {
                return new MetaInfo
                {
                    H1 = txtH1.Text,
                    MetaDescription = txtMetaDescription.Text,
                    MetaKeywords = txtMetaKeywords.Text,
                    Title = txtPageTitle.Text
                };
            }
            set
            {
                txtH1.Text = value.H1;
                txtPageTitle.Text = value.Title;
                txtMetaKeywords.Text = value.MetaKeywords;
                txtMetaDescription.Text = value.MetaDescription;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblHint.Text = Hint;
        }
    }
}
