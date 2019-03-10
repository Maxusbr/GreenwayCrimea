<%@ Page Language="C#" AutoEventWireup="true" Inherits="Admin.m_MessageView"
    Title="MessageView" MasterPageFile="m_MasterPage.master"  Codebehind="m_MessageView.aspx.cs" %>

<asp:Content runat="server" ContentPlaceHolderID="cphCenter">
    <div>
        <asp:Panel ID="Panel1" runat="server" Width="100%">
            <hr />
            <asp:Label ID="lblHeadDate" runat="server" Text="<%$ Resources:Resource, Admin_m_MessageView_Date %>"
                Font-Bold="True"></asp:Label>
            <br />
            <asp:Label ID="lblDate" runat="server" Text="Label"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lblHeadTitle" runat="server" Text="<%$ Resources:Resource, Admin_m_MessageView_Title %>"
                Font-Bold="True"></asp:Label>
            <br />
            <asp:Label ID="lblTitle" runat="server" Text="Label"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lblHeadMessagetext" runat="server" Font-Bold="True" Text="<%$ Resources:Resource, Admin_m_MessageView_Text %>"></asp:Label>
            <br />
            <asp:Label ID="lblMessageText" runat="server"></asp:Label>
            <br />
            <br />
            <hr />
        </asp:Panel>
        &nbsp;
        <br />
        <center>
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
        </center>
    </div>
</asp:Content>
