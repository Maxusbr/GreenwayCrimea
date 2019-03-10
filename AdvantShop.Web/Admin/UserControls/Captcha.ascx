<%@ Control Language="C#" Inherits="UserControls.Captcha" ClientIDMode="AutoID" Codebehind="Captcha.ascx.cs" %>
<div class="captcha-wrap">
    <div class="captha-img">
    <input type="hidden" runat="server" id="hfBase64" />
    <input type="hidden" class="valid-captchasource" runat="server" id="hfSource" />
    <img src='<%="./admin/httphandlers/captcha/getimg.ashx?captchatext=" + HttpUtility.UrlEncode(Base64Text)%>' alt="" />
    </div>
    <div class="captcha-txt">
        <adv:AdvTextBox ValidationType="Captcha" CssClassWrap="input-wrap-captcha" CssClass="captcha-input" ID="txtValidCode" runat="server" />
    </div>
</div>
