<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TelephonyHeadScript.ascx.cs" Inherits="AdvantShop.Admin.UserControls.MasterPage.TelephonyHeadScript" %>
<%@ Import Namespace="AdvantShop.Core.Services.IPTelephony" %>
<% if (IPTelephonyOperator.Current.WebPhoneActive)
   {%>
<link href="../admin/css/telephony/webphone.css" rel="stylesheet" type="text/css" />
<% }%>
<%= IPTelephonyOperator.Current.RenderIntoHead() %>

