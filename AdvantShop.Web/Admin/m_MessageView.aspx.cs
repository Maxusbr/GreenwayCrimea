using System;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using Resources;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace Admin
{
    public partial class m_MessageView : AdvantShopAdminPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_m_MessageView_View));

            if (!IsPostBack)
            {

                int id;
                if (Int32.TryParse(Page.Request["id"], out id))
                {
                    try
                    {
                        LoadContent(id);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }
                else
                {
                    Panel1.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = Resource.Admin_m_MessageView_WrongPageParameters;
                }
            }
        }

        protected void LoadContent(int id)
        {
            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandType = CommandType.Text;
                    db.cmd.CommandType = CommandType.StoredProcedure;
                    db.cmd.CommandText = "[dbo].[sp_GetMessageByID]";
                    db.cmd.Parameters.Clear();
                    db.cmd.Parameters.AddWithValue("@ID", id);
                    db.cnOpen();
                    using(SqlDataReader reader = db.cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            if (reader["AddDate"] != DBNull.Value)
                            {
                                lblDate.Text = AdvantShop.Localization.Culture.ConvertDate((DateTime)reader["AddDate"]);
                            }
                            else
                            {
                                lblDate.Text = "";

                                lblDate.Visible = false;
                                lblHeadDate.Visible = false;
                            }

                            if (reader["Title"] != DBNull.Value)
                            {
                                lblTitle.Text = reader["Title"].ToString();
                            }
                            else
                            {
                                lblTitle.Visible = false;
                                lblHeadTitle.Visible = false;
                            }

                            if (reader["MessageText"] != DBNull.Value)
                            {
                                lblMessageText.Text = reader["MessageText"].ToString();
                            }
                            else
                            {
                                lblMessageText.Visible = false;
                                lblMessageText.Text = "";
                                lblHeadMessagetext.Visible = false;
                            }
                        }
                
                    db.cnClose();

                }
            }
            catch (Exception ex)
            {

                lblError.Visible = true;
                lblError.Text += ex.Message + @" <br/>";
                Debug.Log.Error(ex);
            }

        }
    }
}