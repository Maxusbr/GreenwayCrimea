<%@ Control Language="C#" AutoEventWireup="true" Codebehind="EditRobotsTxt.ascx.cs" Inherits="Admin.UserControls.EditRobotsTxt" %>
<span class="spanSettCategory">
    <%= Resources.Resource.Admin_UserControl_EditRobotsTxt_EditTab%>
</span>
<br />
<span class="subTitleNotify">
    Здесь вы можете задать правила обработки URL для поисковых роботов. <br />
    Будьте внимательны при редактировании данного файла.
</span>
<hr color="#C2C2C4" size="1px" />
<asp:TextBox ID="txtRobots" runat="server" TextMode="MultiLine" CssClass="txtRobotsStyle" ></asp:TextBox>
<span class="subTitleNotify">
    Чтобы сохранить изменения, нажмите оранжевую кнопку "Сохранить" вверху.
</span>
<div class="dvSubHelp" style="margin-bottom:0px;">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/messagebox_help.png" />
    <a href="http://www.advantshop.net/help/pages/robots" target="_blank">Инструкция. Настройка файла robots.txt</a>
</div>