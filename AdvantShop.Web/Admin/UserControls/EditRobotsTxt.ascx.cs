//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

namespace Admin.UserControls
{
    public partial class EditRobotsTxt : System.Web.UI.UserControl
    {
        private string _filename;

        protected void Page_Load(object sender, EventArgs e)
        {
            _filename = Server.MapPath("~/robots.txt");
            FileHelpers.CreateFile(_filename);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                FileHelpers.CreateFile(_filename);
                using (var sr = new StreamReader(_filename))
                {
                    //String line = sr.ReadToEnd();
                    txtRobots.Text = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }
        public void SaveRobots()
        {
            try
            {
                FileHelpers.CreateFile(_filename);
                using (var wr = new StreamWriter(_filename))
                {
                    wr.Write(txtRobots.Text);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }
    }
}
