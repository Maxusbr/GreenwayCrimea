<%@ Page Language="C#" AutoEventWireup="true" Inherits="Tools.pool" Codebehind="pool.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AdvantShop.NET Tools - IIS Pool checker</title>
    <style type="text/css" >
        .Header1 {font-family: Tahoma; font-weight: bold;}
        .ContentDiv {font:0.75em 'Lucida Grande', sans-serif;}
        .Label {font-family: Tahoma; font-size: 16px; color: #666666;}
        .clsText {border:1px solid #DDDDDD; padding:3px; font-size:14px;}
        .clsExtend {padding:6px;}
        .label-box {border-color:#DBDBDB; border-style:solid; border-width:1px 1px 1px 1px; color:#666666; font-size:14px; line-height:1.45em; padding:0.85em 10px 0.85em 10px; text-transform:lowercase; width: 735px; display: block;}
        .label-box.good {background-color:#D3F9BF; border-color:#E1EFDB;}
        .label-box.error {background-color:#FFCFCF; background-image:none; border-color:#E5A3A3; color:#801B1B; padding-left:10px;}
        .btn {background:url('data:image/gif;base64,R0lGODlhCgC9AfQfAPPz8+vr6/7+/uHh4fn5+eXl5d7e3vv7++jn6Ofo5/j4+Ojo6d/g4Ofn59/g3+Pk5OPj4/f29vv7+vz7/ODf3/Dw8Pb29vv8+/z8/ODf4O/v7+fn5unp6N/f3+Dg4P///yH5BAUAAB8ALAAAAAAKAL0BAAX/oCCOZGmeIqau7OG6UiwdRG3fSqTs/G79wCDAMiwSiYCkUllpOp+aqHQa0FSvVmtgy+VyvoEvZxFeIBKLRQK93rg3hbf7UaAX6vQHZM/vD/6AgR6DhIUdBgaHHYuLiI6PkJEfk5SVlpeYmZqbnJ2en6ChoqOkpaanqKmqq6ytrq+wsbKztLW2t7i5uru8vb6/wMHCw8TFxsfIycrLwijOz9DRLNMYEy8u1jAy2zM33gQ94TkR5OQW5UHpQElH7Evv8O9P8xVT9VL3U/pXW1ld/wABihko5kyDBAgaIFCTYEMDOQ7d3JlIEY+eBxgx9tm4J5DHAR4+hvSQoQMFDwwKujFgtIiCA0aPDiGSGammJGY4c+rcybOnz59AgwodSrSo0aNIkypdyrRps2hQo0otoUIANRUTLhyYgOHAha4utIa9NkMGjAMxvEnAocBGW3A94O7QMW6ujwg/8ALRq+6HEXd+4wkeXGEJvcOH9SleLAWL44CQIwskGOZLmgVj0mQ+eDDhmYQNHoZ2M/oOHNMVU9/JmBGCRo4b//AZMPujx5CCcIMktLsDgwwOUHro4GAly+OJaM60yfxRCAA7') /*img/bg-btn.gif*/ repeat-x scroll 0 0 #DDDDDD; border-color:#DDDDDD #DDDDDD #CCCCCC; border-style:solid; border-width:1px; color:#333333; cursor:pointer; font:11px/14px "Lucida Grande",sans-serif; margin:0; overflow:visible; padding:4px 8px 5px; width:auto;}
        .btn-m {background-position:0 -200px; font-size:15px; line-height:20px !important; padding:5px 15px 6px;}
        .btn-m:hover, .btn-m:focus {background-position:0 -206px;}
        #tableParams tr {height: 26px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <span style="font-family: Tahoma; font-weight: bold;">AdvantShop.NET tools</span>
        -
        <asp:HyperLink ID="HyperLink6" runat="server" ForeColor="Green" NavigateUrl="Default.aspx"
            Text="Back to main page"></asp:HyperLink>
        <br />
        <br />
        <div class="ContentDiv">
            <asp:Button ID="Button1" runat="server" Text="Check IIS pool" OnClick="Button1_Click" Width="218px" CssClass="btn btn-m" />
            <br />
            <br />
            <asp:Literal ID="Message" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>
