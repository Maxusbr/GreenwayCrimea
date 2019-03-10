<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" ValidateRequest="False" AutoEventWireup="true" Inherits="Admin.EditTheme" Title="" CodeBehind="EditTheme.aspx.cs" %>

<%@ Import Namespace="Resources" %>
<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">

    <style type="text/css" media="screen">
        #editor {
            width: 75%;
            height: 500px;
            margin: 10px 0;
        }

        #tabs-contents td {
            vertical-align: middle;
        }
    </style>

    <div class="content-top">
        <menu class="neighbor-menu neighbor-catalog">
            <li class="neighbor-menu-item selected"><a href="DesignConstructor.aspx">
                <%= Resource.Admin_MasterPageAdmin_DesignConstructor%></a></li>
            <li class="neighbor-menu-item"><a href="TemplateSettings.aspx">
                <%= Resource.Admin_MasterPageAdmin_TemplateSettings%></a></li>
            <li class="neighbor-menu-item"><a href="StylesEditor.aspx">
                <%= Resource.Admin_MasterPageAdmin_StylesEditor%></a></li>
        </menu>
    </div>
    <div class="content-own">
        <div>
            <table cellpadding="0" cellspacing="0" width="100%" style="padding-left: 10px;">
                <tbody>
                    <tr>
                        <td style="width: 72px;">
                            <img src="images/settings_ico.gif" alt="" />
                        </td>
                        <td class="style1">
                            <asp:Label ID="lbl" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_EditTheme_Header %>"></asp:Label><br />
                            <asp:Label ID="lblName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_EditTheme_Header %>"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Blue"></asp:Label>
        </div>
        <div>
            <div id="themeeditor">
                <ul class="two-column">
                    <li class="two-column-item">
                        <h2 class="justify-item products-header">Тема "<%= ThemeTitle %>"</h2>
                        <div class="theme-treeview">
                            <div class="theme-treeview-item">
                                <a href="javascript:void(0)" class="theme-editor">css</a>
                                <div class="theme-treeview-items">
                                    <a href="javascript:void(0)" class="theme-editor">styles.css</a>
                                </div>
                            </div>
                            <div class="theme-treeview-item">
                                <a class="theme-pictures" href="javascript:void(0)">images</a>
                            </div>
                        </div>
                    </li>
                    <li class="two-column-item">
                        <div class="tab-editor">
                            <div id="editor"><%= CssContent %></div>
                            <a class="btn btn-add btn-small save-theme" href="javascript:void(0);"><%= Resource.Admin_Save %></a>
                        </div>
                        <div class="tab-images" style="display: none;">
                            <input id="file_upload" name="file_upload" type="file" multiple>
                            <div id="progress-up" class="progress-up" style="display: none">
                                <div class="progress-bar progress-bar-success"></div>
                            </div>
                            <div class="files-info"></div>
                        </div>
                    </li>
                </ul>
                <input type="hidden" id="hfTheme" value="<%= ThemeName %>" />
                <input type="hidden" id="hfDesign" value="<%= Design %>" />
            </div>
        </div>
    </div>
    <script type="text/javascript" src="js/ace/ace.js"></script>
    <script type="text/javascript" src="js/ace/ext-language_tools.js"></script>
</asp:Content>
