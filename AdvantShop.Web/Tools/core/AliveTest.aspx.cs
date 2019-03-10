//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Tools.core
{
    public partial class AliveTest : System.Web.UI.Page
    {

        public string _strActualData;

        protected void btnRunTestStandard_Click(object sender, EventArgs e)
        {

            // Validation. Reset to default

            txtUserFolderToTest.CssClass = "clsText";
            StatusRepeater.Visible = true;

            // Process

            var testData = new string[] {"~/", 
                "~/admin/css/",
                "~/admin/js/",
                "~/App_Code/",
                "~/App_Code/AdvantShop/Modules/", 
                "~/App_Data/", 
                "~/App_Data/errlog/", 
                "~/App_Data/Lucene/", 
                "~/combine/",
                "~/css/",
                "~/design/",
                "~/exports/",
                "~/js/",
                "~/Modules/",
                "~/pictures/",
                "~/pictures/category/",
                "~/pictures/category/small/",
                "~/pictures/news/", 
                "~/pictures/product/",
                "~/pictures/product/big/",
                "~/pictures/product/middle/",
                "~/pictures/product/small/",
                "~/pictures/product/xsmall/",
                "~/pictures_deleted/",
                "~/content/price_temp/", 
                "~/templates/",
                "~/content/upload_images/",
                "~/userfiles/", 
                "~/userfiles/file/", 
                "~/userfiles/flash/", 
                "~/userfiles/image/"};

            _strActualData = getActualDataString();

            RunTest(testData);

        }

        protected void btnRunTestUserFolder_Click(object sender, EventArgs e)
        {

            // Validation 

            if (!string.IsNullOrEmpty(txtUserFolderToTest.Text))
            {
                txtUserFolderToTest.CssClass = "clsText";
                StatusRepeater.Visible = true;
            }
            else
            {
                txtUserFolderToTest.CssClass = "clsText_faild";
                StatusRepeater.Visible = false;
                return;
            }

            // Process

            var testData = new string[] { "~/" + txtUserFolderToTest.Text };

            _strActualData = getActualDataString();

            RunTest(testData);

        }

        #region " Main functions "

        private void RunTest(string[] testData)
        {

            var testResults = new List<TestData>();

            foreach (string strTestData in testData)
            {
                var tr = new TestData { Name = strTestData };
                string fullName = HttpContext.Current.Server.MapPath(strTestData);
                tr.AbsolutePath = fullName;
                if (Directory.Exists(fullName))
                {
                    tr.Exist = true;
                    var di = new DirectoryInfo(fullName);
                    try
                    {
                        tr.FilesCount = di.GetFiles().Length;
                    }
                    catch (Exception)
                    {
                    }

                    CheckDirectory(di, tr);
                }
                testResults.Add(tr);
            }

            StatusRepeater.DataSource = testResults;
            StatusRepeater.DataBind();

        }

        private static void CheckDirectory(DirectoryInfo directory, TestData data)
        {
            data.AllowCreate = false;   //ppdAllowCreate = false;
            data.AllowRead = false;     //ppdAllowOpen = false;
            data.AllowWrite = false;    //ppdAllowModify = false;
            data.AllowDelete = false;   //ppdAllowDelete = false;

            string testFileName = directory.FullName + "testFile";

            try
            {
                File.Create(testFileName).Close();
                data.AllowCreate = true;
            }
            catch (Exception)
            {
            }

            var tf = new FileInfo(testFileName);
            BinaryWriter bw = null;
            try
            {
                bw = new BinaryWriter(tf.Open(FileMode.Open));
                data.AllowRead = true;
            }
            catch (Exception)
            {
            }

            if (data.AllowRead)
            {
                bw.Write("test string");
                data.AllowWrite = true;
            }

            if (bw != null)
            {
                bw.Close();
            }

            try
            {
                tf.Delete();
                data.AllowDelete = true;
            }
            catch (Exception)
            {
            }
        }

        protected string getActualDataString()
        {
            return string.Format("Actual data for: '{0}'", DateTime.Now.ToString());
        }

        #endregion

        #region " Model "

        public class TestData
        {
            public string Name { get; set; }
            public string AbsolutePath { get; set; }
            public bool Exist { get; set; }
            public int FilesCount { get; set; }
            public bool AllowCreate { get; set; }
            public bool AllowRead { get; set; }
            public bool AllowWrite { get; set; }
            public bool AllowDelete { get; set; }
        }

        #endregion

    }
}