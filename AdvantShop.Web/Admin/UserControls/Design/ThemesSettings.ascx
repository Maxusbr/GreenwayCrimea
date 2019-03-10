<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.ThemeSettingsControl" Codebehind="ThemesSettings.ascx.cs" %>
<%@ Import Namespace="Resources" %>

<div id="newtheme" style="padding: 5px 0">
    <a class="btn btn-add btn-small createTheme" data-design="<%= DesignType %>" href="javascript:void(0)"><%= Resource.Admin_ThemesSettings_CreateNewTheme %></a>
    <div class="new-theme-modal-block <%= DesignType %>" style="display:none">
        <div style="margin: 50px 0 10px 0;">
            <%= Resource.Admin_ThemesSettings_ThemeName %>: <input type="text" class="themename" placeholder="mytheme" data-design="<%= DesignType %>" />
        </div>
    </div>
</div>

<div style="padding: 5px 0 10px 0">
    <asp:Label ID="lblLoadNew" runat="server" Text="" /> <input id="<%= DesignType %>" class="file_upload <%= DesignType %>" name="file_upload" type="file">
</div>

<asp:Label ID="lblError" runat="server" Text="" Style="color: blue; padding: 10px;
    display: block; border: solid 1px blue; width: 300px; margin: 0 0 10px 0;" Visible="false" />
<asp:Repeater ID="DataListDesigns" runat="server" OnItemCommand="dlItems_ItemCommand"
    OnItemDataBound="dlItems_ItemDataBound">
    <HeaderTemplate>
        <table width="100%" border="0" cellspacing="0" cellpadding="3" class="grid-main">
            <tr class="header">
                <td style="width: 40px; padding-left: 10px;">
                    <%= Resource.Admin_ThemesSettings_Active %>
                </td>
                <td style="width: 35px;">
                    &nbsp;
                </td>
                <td>
                    <b>
                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, Admin_ThemesSettings_Name %>"></asp:Label></b>
                </td>
                <td style="width: 120px"></td>
                <td align="right" style="width: 50px">
                    &nbsp;
                </td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr class="row1" style="height: 40px;">
            <td style="text-align: center;">
                <asp:ImageButton ID="btnCurrentTheme" runat="server" CausesValidation="false" ImageUrl="~/Admin/images/check_active.png"
                    CssClass="addbtn showtooltip" ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_Current %>' />
                <asp:ImageButton ID="btnActivate" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Name") %>'
                    CommandName="ApplyTheme" ImageUrl="~/Admin/images/check_noactive.png" CssClass="addbtn showtooltip"
                    ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_Apply %>' />
            </td>
            <td>
                <%# Eval("PreviewImage") == null || string.IsNullOrEmpty(Eval("PreviewImage").ToString()) ? "" : "<img class='imgtooltip' src='images/adv_photo_ico.gif' abbr='" + Eval("PreviewImage")+ "' alt='' style='padding-left:5px;' />"%>
                <asp:HiddenField ID="hfName" runat="server" Value='<%# Eval("Name") %>' />
                <asp:HiddenField ID="hfSource" runat="server" Value='<%# Eval("Source") %>' />
            </td>
            <td>
                <asp:Literal runat="server" Text='<%#Eval("Title")%>'></asp:Literal>
            </td>
            <td>
                <asp:HyperLink runat="server" ID="hlEdit" Text="<%$ Resources:Resource, Admin_ThemesSettings_EditTheme %>"
                    NavigateUrl='<%# "~/admin/EditTheme.aspx?theme=" + Eval("Name") + "&design=" + DesignType%>' />
            </td>
            <td>
                <%--<asp:ImageButton runat="server" ID="btnAdd" CausesValidation="false" CommandArgument='<%# Eval("Name") %>'
                    CommandName="Add" ImageUrl="~/Admin/images/download.png" CssClass="addbtn showtooltip"
                    ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_Add %>' />--%>
                <asp:LinkButton runat="server" ID="btnDelete" CausesValidation="false" CommandArgument='<%# Eval("Name") %>'
                    CommandName="Delete" CssClass="valid-confirm showtooltip deletebtn"
                    data-confirm="<%$ Resources:Resource, Admin_ThemesSettings_Confirmation %>"
                    ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
            </td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr class="row2" style="height: 40px;">
            <td style="text-align: center;">
                <asp:ImageButton ID="btnCurrentTheme" runat="server" CausesValidation="false" ImageUrl="~/Admin/images/check_active.png"
                    CssClass="addbtn showtooltip" ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_Current %>' />
                <asp:ImageButton ID="btnActivate" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Name") %>'
                    CommandName="ApplyTheme" ImageUrl="~/Admin/images/check_noactive.png" CssClass="addbtn showtooltip"
                    ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_Apply %>' />
            </td>
            <td>
                <%# Eval("PreviewImage") == null || string.IsNullOrEmpty(Eval("PreviewImage").ToString()) ? "" : "<img class='imgtooltip' src='images/adv_photo_ico.gif' abbr='" + Eval("PreviewImage").ToString() + "' alt=''' style='padding-left:5px;' />"%>
                <asp:HiddenField ID="hfName" runat="server" Value='<%# Eval("Name") %>' />
                <asp:HiddenField ID="hfSource" runat="server" Value='<%# Eval("Source") %>' />
            </td>
            <td>
                <asp:Literal runat="server" ID="ltArtNo" Text='<%#Eval("Title")%>'></asp:Literal>
            </td>
            <td>
                <asp:HyperLink runat="server" ID="hlEdit" Text="<%$ Resources:Resource, Admin_ThemesSettings_EditTheme %>"
                    NavigateUrl='<%# "~/admin/EditTheme.aspx?theme=" + Eval("Name") + "&design=" + DesignType%>' />
            </td>
            <td>
                <%--<asp:ImageButton runat="server" ID="btnAdd" CausesValidation="false" CommandArgument='<%#  Eval("Title") %>'
                    CommandName="Add" ImageUrl="~/Admin/images/download.png" CssClass="addbtn showtooltip"
                    ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_Add %>' />--%>
                <asp:LinkButton runat="server" ID="btnDelete" CausesValidation="false" CommandArgument='<%#  Eval("Title") %>'
                    CommandName="Delete" CssClass="valid-confirm showtooltip deletebtn"
                    data-confirm="<%$ Resources:Resource, Admin_ThemesSettings_Confirmation %>"
                    ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
            </td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
<script type="text/javascript">
    $(function () {
        
        function upload<%= DesignType %>() {
            $('#<%= DesignType %>').fileupload({
                url: 'HttpHandlers/Design/UploadTheme.ashx?type=<%= DesignType %>',
                dataType: 'json',
                done: function(e, data) {
                    var result = JSON.parse(data.jqXHR.responseText);
                    alert(result.msg);
                    location.reload();
                },
                fail: function(e, data) {
                    alert(data.jqXHR.responseText);
                }
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { upload<%= DesignType %>(); });
    });
</script>