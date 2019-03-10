<%@ Page Language="C#" AutoEventWireup="true" Inherits="Admin.m_StaticBlock"
    MasterPageFile="m_MasterPage.master" ValidateRequest="false" Codebehind="m_StaticBlock.aspx.cs" %>


<asp:Content ID="contentCenter" ContentPlaceHolderID="cphCenter" runat="server">
    <div style="padding-top: 5px;">
        <div style="text-align: center;" >
            <asp:Label ID="lblBigHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_PagePart_lblMain %>"></asp:Label>
            <br/>
            <asp:Label ID="lblSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_PagePart_Create %>"></asp:Label><br/>
            <asp:Label ID="lblRestrict" runat="server" Text="Label" Font-Bold="True" Visible="False"
                       ForeColor="Red"></asp:Label>
        </div>
        <br />
        <table class="info-center-tb catalog_link">
            <tr>
                <td class="lpart">
                    <asp:Literal ID="Literal1" Text="<%$ Resources:Resource, Admin_PageParts_Key %>" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtKey" runat="server" Width="230px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="lpart">
                    <asp:Literal ID="Literal2" Text="<%$ Resources:Resource, Admin_PageParts_Title %>" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtPageTitle" runat="server" Width="230px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="lpart">
                    <asp:Literal ID="Literal3" Text="<%$ Resources:Resource, Admin_PageParts_Enabled %>" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="chbEnabled" runat="server" Checked="true" />
                    <asp:Label runat="server" ID="lCopyrightMessage" ForeColor="Red" Visible="false"/>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left" Width="700px" CssClass="all-mar">
                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, Admin_PageParts_Text %>"></asp:Label>
                        <br />
                         <asp:TextBox ID="CKEditorControl1" TextMode="MultiLine" runat="server" CssClass="js-wysiwyg" Height="300px" Width="700px" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <center>
            &nbsp;<asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            <asp:HiddenField ID="hfMetaId" runat="server" />
            <br />
            <asp:Button ID="btnAdd" runat="server" Text="Add" Width="103px" ValidationGroup="vGroup" OnClick="btnAdd_Click" />&nbsp;
        </center>
        <br />
        <br />
    </div>
</asp:Content>
