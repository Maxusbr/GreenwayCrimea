//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Web.UI;

namespace AdvantShop.Core.Controls
{
    public abstract class InstallerStep : UserControl
    {
        public void LoadData()
        {
        }

        public void SaveData()
        {
        }

        public bool Validate()
        {
            return false;
        }

        public Control ErrContent { get; set; }
    }
}