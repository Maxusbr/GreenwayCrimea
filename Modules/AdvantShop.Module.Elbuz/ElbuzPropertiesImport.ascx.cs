//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;


//todo переделать без завязывания на движке
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop;
using AdvantShop.FullSearch;
using AdvantShop.Helpers;
using AdvantShop.Module.Elbuz;
using AdvantShop.Module.Elbuz.Domain;
using AdvantShop.Saas;
using AdvantShop.Statistic;
using Image = System.Drawing.Image;

namespace Advantshop.Modules.UserControls
{
    public partial class Admin_ElbuzPropertiesImportModule : System.Web.UI.UserControl
    {

        private readonly string _filePath;
        private readonly string _fullPath;
        private readonly string _directoryPicturesPath = HttpContext.Current.Server.MapPath("~\\Modules\\" + Elbuz.ModuleID + "\\pictures_elbuz\\");

        public Admin_ElbuzPropertiesImportModule()
        {
            _filePath = HttpContext.Current.Server.MapPath("~\\Modules\\" + Elbuz.ModuleID + "\\temp\\");
            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }
            _fullPath = _filePath + "productsProperties.csv";
        }

        private void MsgErr(bool clean)
        {
            if (clean)
            {
                lblError.Visible = false;
                lblError.Text = "";
            }
            else
            {
                lblError.Visible = false;
            }
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text = @"<br/>" + messageText;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            if (!FileUpload1.HasFile)
            {
                MsgErr((string)GetLocalResourceObject("ElbuzPropertiesImport_ChooseFile"));
                return;
            }

            FileUpload1.SaveAs(_fullPath);

            LoadPropertiesExceptions();

            if (fupZipPhotos.HasFile)
            {
                var filePath = HttpContext.Current.Server.MapPath("~\\Modules\\" + Elbuz.ModuleID + "\\temp\\elbuzZipPhotos.zip");

                fupZipPhotos.SaveAs(filePath);

                if (File.Exists(filePath))
                {
                    if (!Directory.Exists(_directoryPicturesPath))
                    {
                        Directory.CreateDirectory(_directoryPicturesPath);
                    }

                    FileHelpers.UnZipFile(filePath, _directoryPicturesPath);
                }
            }

            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }

            if (File.Exists(_fullPath))
            {
                File.Delete(_fullPath);
            }
            FileUpload1.SaveAs(_fullPath);

            if (File.Exists(_fullPath))
            {
                pnlExceptions.Visible = true;
                pUploadExcel.Visible = false;
            }
        }

        protected void btnStartProcess_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CommonStatistic.IsRun)
                {
                    if (!File.Exists(_fullPath))
                    {
                        pnlExceptions.Visible = false;
                        pUploadExcel.Visible = true;
                    }

                    CommonStatistic.Init();
                    CommonStatistic.IsRun = true;
                    CommonStatistic.CurrentProcess = Request.Url.PathAndQuery;
                    linkPropertiesCancel.Visible = true;
                    MsgErr(true);

                    lblPropertiesRes.Text = string.Empty;

                    pUploadExcel.Visible = false;

                    CommonStatistic.StartNew(() => ProcessData(ddlTypeArtNo.SelectedItem.Value));

                    pUploadExcel.Visible = false;
                    pnlExceptions.Visible = false;
                    OutPropertiesDiv.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        private static void LogInvalidData(string message)
        {
            CommonStatistic.WriteLog(message);
            CommonStatistic.TotalErrorRow++;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pUploadExcel.Visible = !CommonStatistic.IsRun;
                OutPropertiesDiv.Visible = CommonStatistic.IsRun;
                linkPropertiesCancel.Visible = CommonStatistic.IsRun;
                ModulesRepository.ModuleExecuteNonQuery("DELETE from [Catalog].[ImportLog]", CommandType.Text);
            }
        }

        private void ProcessData(string typeArtNo)
        {
            using (var streamReaderCount = new StreamReader(_fullPath))
            {
                int count = 0;
                while (!streamReaderCount.EndOfStream)
                {
                    streamReaderCount.ReadLine();
                    count++;
                }
                streamReaderCount.Close();
                CommonStatistic.TotalRow = count - 1;
            }

            var codeMapToArtno = new Dictionary<string, string>();

            using (var streamReader = new StreamReader(_fullPath))
            {
                var headerString = streamReader.ReadLine();

                if (string.IsNullOrEmpty(headerString))
                {
                    streamReader.Close();
                    CommonStatistic.IsRun = false;
                    return;
                }

                var elbuzExceptions = ElbuzRepository.GetElbuzExceptions();
                var header = headerString.Split(new[] { "\t" }, StringSplitOptions.None);
                while (!streamReader.EndOfStream)
                {
                    var temp = streamReader.ReadLine();

                    if (string.IsNullOrEmpty(temp))
                    {
                        CommonStatistic.TotalErrorRow++;
                        continue;
                    }

                    var tempValue = temp.Split(new[] { "\t" }, StringSplitOptions.None);
                    if (string.Equals(tempValue[0], "pics2"))
                    {
                        ProcessProductFoto(tempValue, codeMapToArtno);
                    }
                    else
                    {
                        ProcessProduct(tempValue, header, elbuzExceptions, codeMapToArtno, typeArtNo);
                    }
                    CommonStatistic.RowPosition++;
                }

                streamReader.Close();
                CommonStatistic.IsRun = false;
                LuceneSearch.CreateAllIndexInBackground();
                ProductService.PreCalcProductParamsMassInBackground();
            }

            CategoryService.SetCategoryHierarchicallyEnabled(0);
        }

        private void ProcessProduct(IList<string> temp, IList<string> header, IList<string> exceptions, Dictionary<string, string> codeMapToArtno, string typeArtNo)
        {
            //0. Код товара
            //1. Код категории
            //2. Производитель
            //3. Артикул
            //4. Наименование
            //5. Фото маленькое
            //6. Фото большое
            //7. Производитель
            //8. Модель
            //9. Описание

            var artNo = string.Equals(typeArtNo, "ArtNo") ? temp[3] : temp[0];

            if (string.IsNullOrEmpty(artNo))
            {
                LogInvalidData("Selected ArtNo is empty, in row:" + CommonStatistic.RowPosition + " artNo: " + temp[3] + " code: " + temp[0]);
                return;
            }

            var productId = ProductService.GetProductId(artNo);
            if (productId == 0)
            {
                LogInvalidData("Product not found, in row:" + CommonStatistic.RowPosition + " artNo: " + artNo);
                return;
            }

            codeMapToArtno.Add(temp[0], artNo);

            for (int i = 5; i < header.Count; i++)
            {
                if (!string.IsNullOrEmpty(temp[i]) && !exceptions.Contains(header[i]))
                {
                    PropertyService.UpdateOrInsertProductProperty(productId, header[i], temp[i], 0);
                }
            }
            CommonStatistic.TotalUpdateRow++;
        }

        private void ProcessProductFoto(IList<string> temp, Dictionary<string, string> codeMapToArtno)
        {
            var offerArtNo = temp[1];
            if (codeMapToArtno.ContainsKey(temp[1]))
            {
                offerArtNo = codeMapToArtno[temp[1]];
            }

            var offer = OfferService.GetOffer(offerArtNo);
            var currentPicturePath = _directoryPicturesPath + temp[3];

            if (offer == null)
            {                
                LogInvalidData("Not found offer artno: " + offerArtNo);
                return;
            }
            if (!File.Exists(currentPicturePath))
            {                
                LogInvalidData("Not found poduct photo: " + temp[3]);
                return;
            }
            var productPhotos = PhotoService.GetPhotos(offer.ProductId, PhotoType.Product);
            if (productPhotos.Any(item => item.OriginName == temp[3]))
            {                
                LogInvalidData("Poduct photo already exists: " + temp[3]);
                return;
            }

            var photo = new Photo(0, offer.ProductId, PhotoType.Product)
            {
                OriginName = temp[3],
                Description = temp[7],
                Main = string.Equals(temp[4].ToLower(), "m")
            };

            int sortOrder = 0;
            if (int.TryParse(temp[6], out sortOrder))
            {
                photo.PhotoSortOrder = sortOrder;
            }

            if (offer.ColorID != null)
            {
                photo.ColorID = offer.ColorID;
            }

            var tempName = PhotoService.AddPhoto(photo);

            if (string.IsNullOrWhiteSpace(tempName))
            {
                LogInvalidData("Error in process, productId: " + offer.ProductId);
                return;
            }

            //Save product photo
            using (var image = Image.FromFile(currentPicturePath))
            {
                FileHelpers.SaveProductImageUseCompress(tempName, image);
            }
            File.Delete(currentPicturePath);
            CommonStatistic.TotalUpdateRow++;
        }

        protected void linkCancel_Click(object sender, EventArgs e)
        {
            CommonStatistic.IsRun = false;
            hlDownloadImportLog.Attributes.CssStyle["display"] = "inline";
        }

        protected void lvProperties_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (string.Equals(e.CommandName, "AddException"))
            {
                ElbuzRepository.AddElbuzException(Convert.ToString(e.CommandArgument));
                LoadPropertiesExceptions();
            }
        }

        protected void lvExceptions_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (string.Equals(e.CommandName, "RemoveException"))
            {
                ElbuzRepository.RemoveElbuzException(Convert.ToString(e.CommandArgument));
                LoadPropertiesExceptions();
            }
        }

        protected void LoadPropertiesExceptions()
        {
            var elbuzExceptions = ElbuzRepository.GetElbuzExceptions();
            using (var streamReader = new StreamReader(_fullPath))
            {
                var firstLine = streamReader.ReadLine();
                if (!string.IsNullOrEmpty(firstLine))
                {
                    var listProperties = new List<object>();
                    foreach (var property in firstLine.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (elbuzExceptions.Contains(property))
                        {
                            listProperties.Add(new
                            {
                                Property = property,
                                isUsed = true
                            });
                        }
                        else
                        {
                            listProperties.Add(new
                            {
                                Property = property,
                                isUsed = false
                            });
                        }
                    }

                    lvProperties.DataSource = listProperties;
                    lvProperties.DataBind();
                }
                streamReader.Close();
            }

            lvExceptions.DataSource = elbuzExceptions;
            lvExceptions.DataBind();
        }
    }
}