<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TelephonyScript.ascx.cs" Inherits="AdvantShop.Admin.UserControls.MasterPage.TelephonyScript" %>
<%@ Import Namespace="AdvantShop.Core.Services.IPTelephony" %>

<%= IPTelephonyOperator.Current.RenderBottomScript() %>
<script src='../admin/js/telephony/telephony.js?v=1'></script>
<!--SignalR -->
<script src="../admin/js/signalr/jquery.signalR-2.1.2.min.js"></script>
<script src="../signalr/hubs"></script>
<!--SignalR-->
