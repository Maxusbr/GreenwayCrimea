<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyAccountLink.ascx.cs" Inherits="Admin.UserControls.MasterPage.MyAccountLink" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.Customers" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<a href="<%= !string.IsNullOrEmpty(CustomerContext.CurrentCustomer.EMail)
     ? Resources.Resource.Admin_MasterPageAdmin_MyAccountHref 
        + "/login.aspx?email=" + CustomerContext.CurrentCustomer.EMail 
        + "&hash=" + SecurityHelper.EncodeWithHmac(CustomerContext.CurrentCustomer.EMail, CustomerContext.CurrentCustomer.Password ?? "") 
        + "&shopid=" + SettingsLic.LicKey
     : ""%>" target="_blank" class="btn btn-middle btn-add">
    <%= Resources.Resource.Admin_MasterPageAdmin_MyAccount %>
</a>
