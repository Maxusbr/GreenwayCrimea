<%@ Page AutoEventWireup="true" CodeBehind="StylesEditor.aspx.cs" Inherits="Admin.StylesEditor" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item"><a href="DesignConstructor.aspx">
                <%= Resource.Admin_MasterPageAdmin_DesignConstructor%></a></li>
            <li class="neighbor-menu-item"><a href="TemplateSettings.aspx">
                <%= Resource.Admin_MasterPageAdmin_TemplateSettings%></a></li>
             <li class="neighbor-menu-item selected"><a href="StylesEditor.aspx">
                <%= Resource.Admin_MasterPageAdmin_StylesEditor%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_StylesEditor %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_TemplateSettings_SubHeader %>"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="dvWarningNotify" style="margin-top: 10px; margin-bottom: 20px;">
            Внимание! Используйте дополнительные стили с осторожностью.<br /> Используйте файл, только если вы хорошо владеете навыками работы с CSS.
        </div>
        <div>
            <asp:Label ID="lblInfo" runat="server" ForeColor="Blue" CssClass="tpl-settings-result" />
        </div>
        <div id="style-editor" style="width:800px;height: 600px; margin-top:15px;"><%= CssContent %></div>
        <div style="margin-top:20px; margin-bottom:20px;">
            <a class="btn btn-add btn-middle save-styles" href="javascript:void(0);"><%= Resource.Admin_StylesEditor_Save %></a>
        </div>
    </div>
    <script type="text/javascript" src="js/ace/ace.js"></script>
    <script type="text/javascript" src="js/ace/ext-language_tools.js"></script>
</asp:Content>
