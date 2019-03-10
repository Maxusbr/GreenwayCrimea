//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using Resources;

namespace Admin
{
    public partial class PrintCertificate : AdvantShopAdminPage
    {
 
        protected string RenderSelectedOptions(IList<EvaluatedCustomOptions> evlist)
        {
            if (evlist == null || evlist.Count == 0)
                return "&nbsp;";

            var html = new StringBuilder();
            html.Append("<ul>");

            foreach (EvaluatedCustomOptions ev in evlist)
            {
                html.Append(string.Format("<li>{0}: {1}</li>", ev.CustomOptionTitle, ev.OptionTitle));
            }

            html.Append("</ul>");
            return html.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_CertificateAdmin_Header));

            if (string.IsNullOrEmpty(Request["certificatecode"]))
            {
                Response.Redirect("default.aspx");
            }

            try
            {
                if (!IsPostBack)
                {
                    var certificate = GiftCertificateService.GetCertificateByCode(Request["certificatecode"]);

                    if (certificate != null)
                    {
                        lblCertificateID.Text = certificate.CertificateId.ToString();
                        lblCertificateCode.Text = certificate.CertificateCode;
                        lblOrderNumber.Text = certificate.ApplyOrderNumber;
                        lblSum.Text = certificate.Sum.ToString();
                        lblFrom.Text = certificate.FromName;
                        lblTo.Text = certificate.ToName;
                        lblUserMessage.Text = certificate.CertificateMessage;
                        lblEmail.Text = certificate.ToEmail;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }
    }
}