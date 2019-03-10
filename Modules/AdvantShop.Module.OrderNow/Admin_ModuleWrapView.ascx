<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="AdvantShop.Module.OrderNow.Admin_ModuleWrapView" CodeBehind="Admin_ModuleWrapView.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Module.OrderNow.Service" %>

<%
    int v = ModuleSettings.Version;
    string moduleId = ModuleSettings.ModuleID;
%>

<link rel="stylesheet" href="../Modules/<%=moduleId %>/Content/Styles/admin-style.css?<%=v %>" />
<script type="text/javascript" src="../Modules/<%=moduleId %>/Content/Scripts/admin-script.js?<%=v %>"></script>

<div id="adminModuleWrap"></div>