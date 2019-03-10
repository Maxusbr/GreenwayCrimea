<%@ Page Language="C#" AutoEventWireup="true" Inherits="Tools.core.Default" Codebehind="Default.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdvantShop.NET Core Tools</title>
    <style type="text/css">
        .Header1{font-family:Tahoma;font-weight:700}
        .ContentDiv{font:.75em 'Lucida Grande' ,sans-serif}
        .Label{font-family:Tahoma;font-size:16px;color:#666}
        .clsText{border:1px solid #DDD;padding:3px;font-size:14px}
        .clsText_faild{border:1px solid #E5A3A3;padding:3px;font-size:14px;background-color:#FFCFCF}
        .label-box{border: 1px solid#DBDBDB;color:#666;font-size:14px;line-height:1.45em;padding:.85em 10px;text-transform:lowercase;width:735px;display:block}
        .label-box.good{background-color:#D3F9BF;border-color:#E1EFDB}
        .label-box.error{background-color:#FFCFCF;background-image:none;border-color:#E5A3A3;color:#801B1B;padding-left:10px}
        .btn{background:url('data:image/gif;base64,R0lGODlhCgC9AfQfAPPz8+vr6/7+/uHh4fn5+eXl5d7e3vv7++jn6Ofo5/j4+Ojo6d/g4Ofn59/g3+Pk5OPj4/f29vv7+vz7/ODf3/Dw8Pb29vv8+/z8/ODf4O/v7+fn5unp6N/f3+Dg4P///yH5BAUAAB8ALAAAAAAKAL0BAAX/oCCOZGmeIqau7OG6UiwdRG3fSqTs/G79wCDAMiwSiYCkUllpOp+aqHQa0FSvVmtgy+VyvoEvZxFeIBKLRQK93rg3hbf7UaAX6vQHZM/vD/6AgR6DhIUdBgaHHYuLiI6PkJEfk5SVlpeYmZqbnJ2en6ChoqOkpaanqKmqq6ytrq+wsbKztLW2t7i5uru8vb6/wMHCw8TFxsfIycrLwijOz9DRLNMYEy8u1jAy2zM33gQ94TkR5OQW5UHpQElH7Evv8O9P8xVT9VL3U/pXW1ld/wABihko5kyDBAgaIFCTYEMDOQ7d3JlIEY+eBxgx9tm4J5DHAR4+hvSQoQMFDwwKujFgtIiCA0aPDiGSGammJGY4c+rcybOnz59AgwodSrSo0aNIkypdyrRps2hQo0otoUIANRUTLhyYgOHAha4utIa9NkMGjAMxvEnAocBGW3A94O7QMW6ujwg/8ALRq+6HEXd+4wkeXGEJvcOH9SleLAWL44CQIwskGOZLmgVj0mQ+eDDhmYQNHoZ2M/oOHNMVU9/JmBGCRo4b//AZMPujx5CCcIMktLsDgwwOUHro4GAly+OJaM60yfxRCAA7') /*../img/bg-btn.gif*/ repeat-x scroll 0 0 #DDD;border-color:#DDD #DDD #CCC;border-style:solid;border-width:1px;color:#333;cursor:pointer;font:11px/14px "Lucida Grande" ,sans-serif;margin:0;overflow:visible;padding:4px 8px 5px;width:auto}
        .btn-m{background-position:0 -200px;font-size:15px;line-height:20px!important;padding:5px 15px 6px}
        .btn-m:hover,.btn-m:focus{background-position:0 -206px}
        .fieldsetData{margin-bottom:22px;padding:12px 15px 15px;font-family:Tahoma}
        .tableActualData{font-size:14px}
        .LinkExtraSpace a{display:inline-block;margin-bottom:10px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">

    <span style="font-family: Tahoma; font-weight: bold;">AdvantShop.NET Core tools</span> -
    <asp:HyperLink ID="HyperLink6" runat="server" ForeColor="Green" NavigateUrl="~/tools/"
        Text="Back to main page"></asp:HyperLink><asp:Label ID="lblSplit" runat="server"
            Text=" - " OnClick="lbnExitCoreAuth_Click"></asp:Label><asp:LinkButton ID="lbnExitCoreAuth"
                runat="server" Text="ExitCoreAuth" OnClick="lbnExitCoreAuth_Click"></asp:LinkButton>
    <br />
    <br />
    <asp:Panel ID="pnlauth" runat="server">
        <fieldset style="margin-bottom: 22px; padding: 15px 15px 15px 15px;">
            <table cellpadding="0" cellspacing="0" style="width: 186px; height: 62px">
                <tr>
                    <td style="text-align: right; padding-right: 5px;">
                        Login
                    </td>
                    <td>
                        <asp:TextBox ID="txtLogin" runat="server" TextMode="Password" CssClass="clsText"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; padding-right: 5px;">
                        Pass
                    </td>
                    <td>
                        <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="clsText"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table cellspacing="0" cellpadding="0" style="width: 186px;">
                <tr>
                    <td style="text-align: right; width: 113px;">
                        <asp:Label ID="lblAuthRes" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="height: 25px; text-align: right;">
                        <asp:Button ID="btnLogin" runat="server" Text="login" OnClick="btnLogin_Click" />
                        <asp:LinkButton runat="server" ID="lbLogin" Text=""></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="pnlcmd" runat="server">
        <fieldset style="margin-bottom: 22px; padding: 15px 15px 15px 15px;">
            <div style="color: Red;">
                Warning! If Session was not initialized.
                <br />
                Check PingDB function at first, push Initialize Session then<br />
            </div>
            <br />
            <asp:LinkButton ID="lkbPingDB" runat="server" OnClick="lkbPingDB_Click">Ping DataBase</asp:LinkButton>
            <br />
            <br />
            Ping result: <b><asp:Label ID="lblPingResult" runat="server" Text=""></asp:Label></b>
            <br />
            <br />
            <asp:LinkButton ID="lkbInitSession" runat="server" OnClick="lkbInitSession_Click">Start InitSession</asp:LinkButton>
            <br />
            <br />
            Init result: <b><asp:Label ID="lblInitSession" runat="server" Text=""></asp:Label></b>
            <br />
            <br />
            <asp:LinkButton ID="lnkShowCurrentCN" runat="server" OnClick="lnkShowCurrentCN_Click">Show Current CN</asp:LinkButton>
            <br />
            <br />
            Current cn: <b><asp:Label ID="lblCurCN" runat="server" Text=""></asp:Label></b>
            <br />
            <br />
            <asp:LinkButton ID="lnkShowCurrentPath" runat="server" 
                onclick="lnkShowCurrentPath_Click">Show Current Files Path</asp:LinkButton>
            <br />
            <br />
            Current path: <b><asp:Label ID="lblCurPath" runat="server" Text=""></asp:Label></b>
            <br />
            <br />
            <asp:LinkButton ID="lnkShowMode1" runat="server" OnClick="lnkShowMode1_Click">Show Shop Mode</asp:LinkButton>
            <br />
            <br />
            Current Mode:
            <asp:Label ID="lblShowMode1" runat="server" Text=""></asp:Label>
        </fieldset>
        <fieldset class="LinkExtraSpace" style="margin-bottom: 22px; padding: 15px 15px 15px 15px;">
            <legend>TOP-3</legend>
            <a href="Sqlexec.aspx" style="color: Blue;">SQL Command execute</a><br />
            <a href="Cleanup.aspx" style="color: Blue;">Cleanup tool</a><br />
            <a href="AliveTest.aspx" style="color: Blue">Alive test</a><br />
        </fieldset>
        <fieldset style="margin-bottom: 22px; padding: 15px 15px 15px 15px;">
            <a href="AppRestartLog.aspx" style="color: Blue">Application restart log</a><br />
            <a href="DeleteSvn.aspx" style="color: Blue;">Delete all .svn directories</a><br />
            <a href="ProcessBrokenCategories.aspx" style="color: Blue;">Processing of broken categories</a><br />
            <a href="updater.aspx" style="color: Blue;">Updater</a><br />
            <a href="updaterfromfile.aspx" style="color: Blue;">Update from file</a><br />
            <a href="iscustom.aspx" style="color: Blue;">Check for changes</a><br />
            <a href="backuper.aspx" style="color: Blue;">Backuper</a><br />
            <br />
            <asp:Button ID="CreateFullTextIndexes" Text="Create full text indexes" runat="server"
                OnClick="CreateFullTextIndexes_Click"></asp:Button>
            <asp:Label ID="indexDone" Visible="false" runat="server" Text="Done!"></asp:Label>
        </fieldset>
        <fieldset style="margin-bottom: 22px; padding: 15px 15px 15px 15px;">
            <div style="color: Red;">
                Warning: This button permamently reset order counter!<br />
                Warning: Do not use if you not sure!!!<br />
                <br />
            </div>
            <asp:LinkButton ID="lbkReserOrderNum" runat="server" OnClick="btnClearOrders_Click">Reser order number to 1</asp:LinkButton>
            <br />
            <br />
            Reset result: <b>
                <asp:Label ID="lblResetOrderResult" runat="server" Text=""></asp:Label></b>
        </fieldset>
    </asp:Panel>
    </form>
</body>
</html>
