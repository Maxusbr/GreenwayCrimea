<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Admin.UserControls.Settings.SocialSettings" Codebehind="SocialSettings.ascx.cs" %>
<table class="info-tb" style="width: 600px;">
    <tr class="rowPost">
        <td style="height: 34px;">
            <span class="spanSettCategory">
                <%= Resources.Resource.Admin_CommonSettings_SocialShare%>
            </span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 155px;">
            <asp:CheckBox runat="server" ID="chkSocialShareEnabled" Text="<%$ Resources: Resource, Admin_CommonSettings_SocialShareEnabled%>"/>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td style="width: 155px;">
            <input runat="server" class="social-settings-checkbox" id="rbSocialShareDefault" type="radio" name="socialShare" value="default" style="height:18px; float:left; margin-right:5px;" />
            <label class="form-lbl" for="<%= rbSocialShareDefault.ClientID %>" class="Label"><%= Resources.Resource.Admin_CommonSettings_SocialShareDefault%></label>
        </td>
    </tr>
    <tr class="rowsPost row-interactive">
        <td>
            <input runat="server" class="social-settings-checkbox social-settings-checkbox-custom" id="rbSocialShareCustom" type="radio" name="socialShare" value="custom" style="height:18px; float:left; margin-right:5px;" />
            <label class="form-lbl" for="<%= rbSocialShareCustom.ClientID %>" class="Label"><%= Resources.Resource.Admin_CommonSettings_SocialShareСustom%></label>
        </td>
    </tr>
    <tr class="rowsPost row-interactive js-social-custom-code">
        <td>
            <adv:AdvTextBox ID="txtSocialCustomCode" TextMode="Multiline" runat="server" CssClassWrap="textAreaSocial" />
        </td>
    </tr>
</table>
<div class="dvSubHelp" style="margin-top:10px; margin-bottom:10px;">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
    <a href="http://www.advantshop.net/help/pages/change-social-code" target="_blank">Инструкция. Cоциальные кнопки в карточке товара</a>
</div>
<span class="subSaveNotify">
    Чтобы сохранить изменения, нажмите оранжевую кнопку "Сохранить" вверху.
</span>
<script type="text/javascript">
    $(function () {

        if ($('.social-settings-checkbox-custom').is(':checked')) {
            $('.js-social-custom-code').show();
        }
        else {
            $('.js-social-custom-code').hide();
        }

        $('.social-settings-checkbox').on('click', function () {

            if ($(this).hasClass('social-settings-checkbox-custom')) {
                $('.js-social-custom-code').show();
            } else {
                $('.js-social-custom-code').hide();
            }
        });
    });
</script>
