//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Design;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Resources;

namespace Admin.UserControls
{
    public partial class ThemeSettingsControl : System.Web.UI.UserControl
    {
        public eDesign DesignType { get; set; }
        private List<Theme> _currentDesigns;
        //private List<Theme> _onLineDesigns;
        private const string _none = "_none";

        private string designFolderPath = SettingsDesign.Template != TemplateService.DefaultTemplateId
                                              ? HttpContext.Current.Server.MapPath("~/Templates/" + SettingsDesign.Template + "/")
                                              : HttpContext.Current.Server.MapPath("~/");

        #region Errors

        private void MsgErr(bool clear)
        {
            lblError.Visible = false;
            lblError.Text = string.Empty;
        }

        private void MsgErr(string messageText)
        {
            lblError.Visible = true;
            lblError.Text += messageText;
        }

        #endregion

        #region MsgConstants

        private string successAddingMsg;
        private string choosegMsg;
        private string successDeleteMsg;
        private string extensionErrMsg;

        #endregion

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MsgErr(true);

            CacheManager.RemoveByPattern(CacheNames.GetDesignCacheObjectName(""));
            _currentDesigns = DesignService.GetDesigns(DesignType);
            //_onLineDesigns = DesignService.GetAvaliableDesignsOnLine(DesignType);

            LoadData();

            switch (DesignType)
            {
                case eDesign.Background:
                    lblLoadNew.Text = Resource.Admin_ThemesSettings_LoadNewBackground;
                    successAddingMsg = Resource.Admin_ThemesSettings_SuccessAddingBackground;
                    successDeleteMsg = Resource.Admin_ThemesSettings_SuccessDeleteBackground;
                    choosegMsg = Resource.Admin_ThemesSettings_ChooseBackground;
                    extensionErrMsg = Resource.Admin_ThemesSettings_ExtErrBackground;
                    break;

                case eDesign.Theme:
                    lblLoadNew.Text = Resource.Admin_ThemesSettings_LoadNewTheme;
                    successAddingMsg = Resource.Admin_ThemesSettings_SuccessAddingTheme;
                    successDeleteMsg = Resource.Admin_ThemesSettings_SuccessDeleteTheme;
                    choosegMsg = Resource.Admin_ThemesSettings_ChooseTheme;
                    extensionErrMsg = Resource.Admin_ThemesSettings_ExtErrTheme;
                    break;

                case eDesign.Color:
                    lblLoadNew.Text = Resource.Admin_ThemesSettings_LoadNewColor;
                    successAddingMsg = Resource.Admin_ThemesSettings_SuccessAddingColor;
                    successDeleteMsg = Resource.Admin_ThemesSettings_SuccessDeleteColor;
                    choosegMsg = Resource.Admin_ThemesSettings_ChooseColor;
                    extensionErrMsg = Resource.Admin_ThemesSettings_ExtErrColor;
                    break;
            }
        }


        private void LoadData()
        {
            CacheManager.RemoveByPattern(CacheNames.GetDesignCacheObjectName(""));
            _currentDesigns = DesignService.GetDesigns(DesignType);
            //_onLineDesigns = DesignService.GetAvaliableDesignsOnLine(DesignType);

            DataListDesigns.DataSource = (_currentDesigns ?? new List<Theme>()).OrderBy(design => design.Name);
            DataListDesigns.DataBind();
        }

        protected void dlItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            MsgErr(true);

            //if (e.CommandName == "Add")
            //{
            //    string url = ((HiddenField) e.Item.FindControl("hfSource")).Value;
            //    string themeName = ((HiddenField) e.Item.FindControl("hfName")).Value;
            //    InstallDesign(url, themeName);
            //}

            if (e.CommandName == "Delete")
            {
                string themeName = ((HiddenField) e.Item.FindControl("hfName")).Value;
                if (themeName != _none)
                {
                    UninstallDesign(themeName);
                }
            }

            if (e.CommandName == "ApplyTheme")
            {
                string themeName = e.CommandArgument.ToString();

                switch (DesignType)
                {
                    case eDesign.Theme:
                        if (themeName != "_none" && SettingsDesign.Background != "_none")
                        {
                            SettingsDesign.Background = "_none";
                        }
                        SettingsDesign.Theme = themeName;
                        break;
                    case eDesign.Background:
                        if (themeName != "_none" && SettingsDesign.Theme != "_none")
                        {
                            SettingsDesign.Theme = "_none";
                        }
                        SettingsDesign.Background = themeName;
                        break;
                    case eDesign.Color:
                        SettingsDesign.ColorScheme = themeName;
                        break;
                }
            }

            LoadData();
        }

        protected void dlItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                var name = ((HiddenField) item.FindControl("hfName")).Value;
                //var btnAdd = ((ImageButton) item.FindControl("btnAdd"));
                var btnDelete = ((LinkButton) item.FindControl("btnDelete"));
                var hlEdit = ((HyperLink)item.FindControl("hlEdit"));

                var theme = _currentDesigns.Find(t => t.Name == name);

                if (theme != null)
                {
                    //btnAdd.Visible = false;
                    btnDelete.Visible = theme.Name != _none;
                    hlEdit.Visible = theme.Name != _none;

                    bool isCurrent = false;

                    switch (DesignType)
                    {
                        case eDesign.Theme:
                            isCurrent = theme.Name == SettingsDesign.Theme;
                            break;
                        case eDesign.Background:
                            isCurrent = theme.Name == SettingsDesign.Background;
                            break;
                        case eDesign.Color:
                            isCurrent = theme.Name == SettingsDesign.ColorScheme;
                            break;
                    }

                    (item.FindControl("btnCurrentTheme")).Visible = isCurrent;
                    (item.FindControl("btnActivate")).Visible = !isCurrent;
                }
                else
                {
                    //btnAdd.Visible = true;
                    btnDelete.Visible = false;
                    hlEdit.Visible = false;

                    (item.FindControl("btnCurrentTheme")).Visible = false;
                    (item.FindControl("btnActivate")).Visible = false;
                }
            }
        }

        //private void InstallDesign(string url, string themeName)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(url))
        //        {
        //            string filename = string.Format("{0}design\\{1}s\\{2}.zip", designFolderPath, DesignType.ToString(),
        //                                            themeName);

        //            using (var client = new WebClient())
        //            {
        //                client.DownloadFile(url, filename);
        //            }

        //            if (FileHelpers.UnZipFilesAndFolders(filename))
        //            {
        //                MsgErr(successAddingMsg);
        //            }
        //            else
        //            {
        //                MsgErr(Resource.Admin_ThemesSettings_ErrorUnZip);
        //            }
        //            FileHelpers.DeleteFile(filename);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.Log.Error(ex);
        //        MsgErr(Resource.Admin_ThemesSettings_ErrorHappens);
        //    }
        //}

        private void UninstallDesign(string themeName)
        {
            if (themeName == _none)
                return;

            try
            {
                _currentDesigns = DesignService.GetDesigns(DesignType);
                string dirname = string.Format("{0}design\\{1}s\\{2}", designFolderPath, DesignType.ToString(),
                                               themeName);

                switch (DesignType)
                {
                    case eDesign.Theme:
                        if (SettingsDesign.Theme == themeName)
                        {
                            var theme = _currentDesigns.FirstOrDefault(t => t.Name != themeName && t.Name != _none);
                            SettingsDesign.Theme = theme != null ? theme.Name : _none;
                        }
                        break;
                    case eDesign.Background:
                        if (SettingsDesign.Background == themeName)
                        {
                            var theme = _currentDesigns.FirstOrDefault(t => t.Name != themeName && t.Name != _none);
                            SettingsDesign.Background = theme != null ? theme.Name : _none;
                        }
                        break;
                    case eDesign.Color:
                        if (SettingsDesign.ColorScheme == themeName)
                        {
                            var theme = _currentDesigns.FirstOrDefault(t => t.Name != themeName && t.Name != _none);
                            SettingsDesign.ColorScheme = theme != null ? theme.Name : _none;
                        }
                        break;
                }

                FileHelpers.DeleteDirectory(dirname);
                MsgErr(successDeleteMsg);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                MsgErr(Resource.Admin_ThemesSettings_ErrorDelete);
            }
        }

        //protected void InstallAll(object sender, EventArgs e)
        //{
        //    LoadData();

        //    if (_currentDesigns == null || _onLineDesigns == null)
        //        return;

        //    var listToInstall = _onLineDesigns.Except(_currentDesigns);
        //    foreach (var design in listToInstall)
        //    {
        //        InstallDesign(design.Source, design.Name);
        //    }
        //    LoadData();

        //    MsgErr(true);
        //    MsgErr(successAddingMsg);
        //}

        //protected void UpdateInstalled(object sender, EventArgs e)
        //{
        //    LoadData();

        //    if (_currentDesigns == null || _onLineDesigns == null)
        //        return;

        //    var listToUpdate = _onLineDesigns.Intersect(_currentDesigns);
        //    foreach (var design in listToUpdate)
        //    {
        //        UninstallDesign(design.Name);
        //        InstallDesign(design.Source, design.Name);
        //    }
        //    LoadData();

        //    MsgErr(true);
        //    MsgErr(successAddingMsg);
        //}

        //protected void DeleteAll(object sender, EventArgs e)
        //{
        //    LoadData();

        //    if (_currentDesigns == null)
        //        return;

        //    foreach (var design in _currentDesigns)
        //    {
        //        UninstallDesign(design.Name);
        //    }
        //    LoadData();

        //    MsgErr(true);
        //    MsgErr(successDeleteMsg);
        //}
    }
}