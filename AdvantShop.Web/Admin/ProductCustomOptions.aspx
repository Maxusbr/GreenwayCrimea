<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageEmpty.master" AutoEventWireup="true" CodeBehind="ProductCustomOptions.aspx.cs" Inherits="Admin.ProductCustomOptions" EnableViewStateMac="false" %>

<%@ Register Src="~/Admin/UserControls/Product/ProductCustomOption.ascx" TagName="ProductCustomOption" TagPrefix="adv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <style>
        html, body {
            background: none !important;
        }
    </style>
    <adv:ProductCustomOption ID="productCustomOption" runat="server" ShowSaveButton="True" />
</asp:Content>
