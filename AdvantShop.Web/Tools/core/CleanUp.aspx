<%@ Page Language="C#" AutoEventWireup="true" Inherits="Tools.core.CleanUp"
    MasterPageFile="MasterPage.master" Codebehind="CleanUp.aspx.cs" %>

<asp:Content runat="server" ID="cntHead" ContentPlaceHolderID="head">
    <title>AdvantShop.NET Core Tools - Cleanup</title>
</asp:Content>
<asp:Content runat="server" ID="cntmain" ContentPlaceHolderID="main">
    <fieldset style="margin-bottom: 22px; padding: 15px 15px 15px 15px;">
        <div style="margin-bottom: 22px;">
            <asp:Button ID="btnCleanUpPictureFolder" runat="server" Text="Clean product pictures folder"
                OnClick="btnCleanUpPictureFolder_Click" />
        </div>
        <div style="margin-bottom: 22px;">
            <asp:CheckBox ID="chboxDeleteFiles" runat="server" Text="Delete files" />
        </div>
        <div style="font-family: Courier New; color: Blue; font-weight: bold; font-size: 11pt;
            margin-bottom: 11px;">
            <asp:Label ID="lCompleted" runat="server" EnableViewState="false" Visible="false">Cleanup successfuly completed</asp:Label>
        </div>
        <div>
            <asp:Label ID="lResultHeader" runat="server" Visible="false" EnableViewState="false">Deleted files:</asp:Label>
        </div>
        <div style="font-family: Courier New; font-size: 10pt;">
            <div style="text-align: left;">
                <asp:Literal ID="lResult" runat="server" EnableViewState="false" Text="" />
            </div>
        </div>
    </fieldset>
    <fieldset style="margin-bottom: 22px; padding: 15px 15px 15px 15px;">
        <div style="margin-bottom: 22px;">
            <asp:Button ID="btnCleanUpBD" runat="server" Text="Cleanup DB" OnClick="btnCleanUpBD_Click" />
        </div>
        <div style="margin-bottom: 22px;">
            <asp:CheckBox ID="chboxMakeNull" runat="server" Text="Delete wrong data" />
        </div>
        <div style="font-family: Courier New; color: Blue; font-weight: bold; font-size: 11pt;
            margin-bottom: 11px;">
            <asp:Label ID="lDBCleanupCompleted" runat="server" EnableViewState="false" Visible="false">Cleanup successfuly completed</asp:Label>
        </div>
        <div style="font-family: Courier New; font-size: 10pt;">
            <div style="text-align: left;">
                <asp:Literal ID="lDBResult" runat="server" EnableViewState="false" Text="" />
            </div>
        </div>
    </fieldset>
    <fieldset style="margin-bottom: 22px; padding: 15px 15px 15px 15px;">
        <div style="margin-bottom: 22px;">
            Folder '~/pictures_deleted/' current size:
            <asp:Label ID="lblFolderSize" runat="server" EnableViewState="false" />
            <asp:Button ID="Button3" runat="server" Text="Get FolderSize" OnClick="GetDeletedFolderSize" /><br /><br />
            <asp:Button ID="Button2" runat="server" Text="Clear '~/pictures_deleted' folder" OnClick="CleanDeletedFolder" />
        </div>

    </fieldset>
</asp:Content>
