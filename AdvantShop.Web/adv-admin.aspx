<%@ Page Language="C#" AutoEventWireup="true" Inherits="ClientPages.Adv_Admin" Codebehind="adv-admin.aspx.cs" %>
<%@ Import Namespace="AdvantShop.Saas" %>
<%@ Register Src="~/Admin/UserControls/Captcha.ascx" TagName="CaptchaControl" TagPrefix="adv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Resources.Resource.Client_Admin_PageTitle%></title>
    <style type="text/css">
        body {}
        .Admin_ValidTextBox {border:1px solid gray; padding:3px; font-size:19px;}
        .Admin_InvalidTextBox {border:1px solid #E5A3A3; padding:3px; font-size:19px; background-color:#FFCFCF;}
        .Admin_MainDiv {background: url('images/controls/advlogin_bg.jpg') left top no-repeat; width:436px; height:355px; font-family:Tahoma; font-size:12px;}
        .Admin_SubMainDiv {padding-left:20px;padding-top:3px;}
        .Admin_Captcha{width:90px;}
        .txtLogPass {width:180px;}
        .captcha-input {border:1px solid gray; padding:3px; font-size:19px; width:180px;}
        .adminLabel {color: #ffffff; font-size: 17px; font-weight: bold;}
        .labels {font-size:19px;}
        .captha-img img {border: 1px solid buttonshadow; height: 28px; margin-bottom: 8px;}
        .btn {background:url('data:image/gif;base64,R0lGODlhCgC9AfQfAPPz8+vr6/7+/uHh4fn5+eXl5d7e3vv7++jn6Ofo5/j4+Ojo6d/g4Ofn59/g3+Pk5OPj4/f29vv7+vz7/ODf3/Dw8Pb29vv8+/z8/ODf4O/v7+fn5unp6N/f3+Dg4P///yH5BAUAAB8ALAAAAAAKAL0BAAX/oCCOZGmeIqau7OG6UiwdRG3fSqTs/G79wCDAMiwSiYCkUllpOp+aqHQa0FSvVmtgy+VyvoEvZxFeIBKLRQK93rg3hbf7UaAX6vQHZM/vD/6AgR6DhIUdBgaHHYuLiI6PkJEfk5SVlpeYmZqbnJ2en6ChoqOkpaanqKmqq6ytrq+wsbKztLW2t7i5uru8vb6/wMHCw8TFxsfIycrLwijOz9DRLNMYEy8u1jAy2zM33gQ94TkR5OQW5UHpQElH7Evv8O9P8xVT9VL3U/pXW1ld/wABihko5kyDBAgaIFCTYEMDOQ7d3JlIEY+eBxgx9tm4J5DHAR4+hvSQoQMFDwwKujFgtIiCA0aPDiGSGammJGY4c+rcybOnz59AgwodSrSo0aNIkypdyrRps2hQo0otoUIANRUTLhyYgOHAha4utIa9NkMGjAMxvEnAocBGW3A94O7QMW6ujwg/8ALRq+6HEXd+4wkeXGEJvcOH9SleLAWL44CQIwskGOZLmgVj0mQ+eDDhmYQNHoZ2M/oOHNMVU9/JmBGCRo4b//AZMPujx5CCcIMktLsDgwwOUHro4GAly+OJaM60yfxRCAA7') /*images/controls/bg-btn.gif*/ repeat-x scroll 0 0 #DDDDDD; border: 1px solid gray; color:#333333; cursor:pointer; font:11px/14px "Lucida Grande",sans-serif; margin:0; overflow:visible; padding:4px 8px 5px; width:101px;}
        .btn-m {background-position:0 -200px; font-size:15px; line-height:20px !important; padding:5px 15px 6px;}
        .btn-m:hover, .btn-m:focus {background-position:0 -206px;}
        .errmessage {font-size:14px; font-weight:bold;}
        .dvBtnOrient {margin-left:99px; width:74px;}
        .version {font-family:Tahoma; font-size:12px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <br />
    <br />
    <br />
    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnLogIn">
        <center>
            <div class="Admin_MainDiv">
                <div class="Admin_SubMainDiv">
                    <table border="0" cellpadding="0" cellspacing="0" style="text-align:center; width:300px;">
                        <tr>
                            <td colspan="4" style="height:25px;">
                                <span class="adminLabel"><asp:Localize ID="Localiz1" runat="server" 
                                    Text="<%$ Resources:Resource, Client_Admin_GoInside %>"></asp:Localize></span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="height:25px;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr style="height: 42px;">
                            <td style="text-align: right; width: 60px;">
                                <span class="labels"><asp:Localize ID="Localize2" runat="server" 
                                    Text="<%$ Resources:Resource, Client_Admin_Login %>"></asp:Localize></span>
                            </td>
                            <td style="width: 15px;">
                                &nbsp;
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtLogin" runat="server" TextMode="Password" CssClass="Admin_ValidTextBox txtLogPass"></asp:TextBox>
                            </td>
                            <td style="width: 15px;">
                                <div style="color: Red;">*</div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">
                                <span class="labels"><asp:Localize ID="Localize3" runat="server" 
                                    Text="<%$ Resources:Resource, Client_Admin_Pass %>"></asp:Localize></span>
                            </td>
                            <td style="width: 15px;">
                                &nbsp;
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="Admin_ValidTextBox txtLogPass"></asp:TextBox>
                            </td>
                            <td style="width:15px;">
                                <div style="color: Red;">*</div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="height: 15px;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">
                                <span class="labels"><asp:Localize ID="Localize4" runat="server" 
                                    Text="<%$ Resources:Resource, Client_Admin_Code %>"></asp:Localize></span>
                            </td>
                            <td style="width: 15px;">
                                &nbsp;
                            </td>
                            <td>
                                <table style="width: 100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="text-align: left;">
                                            <adv:CaptchaControl ID="validShield" runat="server" DisplayAnyWay="True" />
                                        </td>
                                        <td style="width: 85px;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 15px;">
                                <div style="color: Red;">*</div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="height:55px;">
                               <span class="errmessage">
                                    <asp:Label ID="lblError" runat="server" ForeColor="Blue" />
                                </span>
                            </td>
                        </tr>
                        <tr style="height:50px;">
                            <td colspan="4" style="text-align:left;">
                                <div class="dvBtnOrient">
                                    <asp:Button ID="btnLogIn" runat="server" CssClass="btn btn-m"
                                        Text="<%$ Resources:Resource, Client_Admin_Wayin %>" OnClick="btnLogIn_Click" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="version">
                <%=AdvantShop.Configuration.SettingsGeneral.SiteVersion %> 
                <%=AdvantShop.Trial.TrialService.IsTrialEnabled ? "(trial)" : "" %> 
                <%=SaasDataService.IsSaasEnabled ? "(saas)" : "" %>
                <%=!SaasDataService.IsSaasEnabled && !AdvantShop.Trial.TrialService.IsTrialEnabled ? "(license)" : "" %>
            </div>
        </center>
    </asp:Panel>
    <br />
    </form>
</body>
</html>
