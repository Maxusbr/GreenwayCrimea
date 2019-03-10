<%@ Page Language="C#" AutoEventWireup="true" Inherits="Tools.Sqlexec" Codebehind="Sqlexec.aspx.cs" MasterPageFile="MasterPage.master" %>

<asp:Content runat="server" ID="cntHead" ContentPlaceHolderID="head">
    <title>AdvantShop.NET Core Tools - Cleanup</title>
    <style type="text/css" >
        .Header1 {font-family: Tahoma; font-weight: bold;}
        .ContentDiv {font:0.75em 'Lucida Grande', sans-serif;}
        .Label {font-family: Tahoma; font-size: 16px; color: #666666;}
        .clsText {border:1px solid #DDDDDD; padding:3px; font-size:14px;}
        .clsExtend {padding:6px;}
        .label-box {border-color:#DBDBDB; border-style:solid; border-width:1px 1px 1px 1px; color:#666666; font-size:14px; line-height:1.45em; padding:0.85em 10px 0.85em 10px; text-transform:lowercase; width: 735px; display: block; margin-bottom:3px;}
        .label-box.good {background-color:#D3F9BF; border-color:#00d200;}
        .label-box.error {background-color:#FFCFCF; background-image:none; border-color:#E5A3A3; color:#801B1B; padding-left:10px;}
        .btn {background:url('data:image/gif;base64,R0lGODlhCgC9AfQfAPPz8+vr6/7+/uHh4fn5+eXl5d7e3vv7++jn6Ofo5/j4+Ojo6d/g4Ofn59/g3+Pk5OPj4/f29vv7+vz7/ODf3/Dw8Pb29vv8+/z8/ODf4O/v7+fn5unp6N/f3+Dg4P///yH5BAUAAB8ALAAAAAAKAL0BAAX/oCCOZGmeIqau7OG6UiwdRG3fSqTs/G79wCDAMiwSiYCkUllpOp+aqHQa0FSvVmtgy+VyvoEvZxFeIBKLRQK93rg3hbf7UaAX6vQHZM/vD/6AgR6DhIUdBgaHHYuLiI6PkJEfk5SVlpeYmZqbnJ2en6ChoqOkpaanqKmqq6ytrq+wsbKztLW2t7i5uru8vb6/wMHCw8TFxsfIycrLwijOz9DRLNMYEy8u1jAy2zM33gQ94TkR5OQW5UHpQElH7Evv8O9P8xVT9VL3U/pXW1ld/wABihko5kyDBAgaIFCTYEMDOQ7d3JlIEY+eBxgx9tm4J5DHAR4+hvSQoQMFDwwKujFgtIiCA0aPDiGSGammJGY4c+rcybOnz59AgwodSrSo0aNIkypdyrRps2hQo0otoUIANRUTLhyYgOHAha4utIa9NkMGjAMxvEnAocBGW3A94O7QMW6ujwg/8ALRq+6HEXd+4wkeXGEJvcOH9SleLAWL44CQIwskGOZLmgVj0mQ+eDDhmYQNHoZ2M/oOHNMVU9/JmBGCRo4b//AZMPujx5CCcIMktLsDgwwOUHro4GAly+OJaM60yfxRCAA7') /*../img/bg-btn.gif*/ repeat-x scroll 0 0 #DDDDDD; border-color:#DDDDDD #DDDDDD #CCCCCC; border-style:solid; border-width:1px; color:#333333; cursor:pointer; font:11px/14px "Lucida Grande",sans-serif; margin:0; overflow:visible; padding:4px 8px 5px; width:auto;}
        .btn-m {background-position:0 -200px; font-size:15px; line-height:20px !important; padding:5px 15px 6px;}
        .btn-m:hover, .btn-m:focus {background-position:0 -206px;}
        .spnote{margin-left:3px; color:gray;}
    </style>
</asp:Content>

<asp:Content runat="server" ID="cntmain" ContentPlaceHolderID="main">
    <div class="ContentDiv">
        <span class="Label">Connection string:</span><br /><br />
        <asp:TextBox ID="txtCNtext" runat="server" CssClass="clsText clsExtend" Width="98%"></asp:TextBox>&nbsp;<br /><br />
        <asp:Button ID="btnTestConnection" runat="server" Text="test cn" CssClass="btn btn-m" Width="105px" OnClick="btnTestConnection_Click"/><br /><br />
        <asp:TextBox ID="txtSqlText" runat="server" CssClass="clsText" Height="144px" TextMode="MultiLine" Width="750px"></asp:TextBox><br />
        <span class="spnote">Для выполенения нескольких sql комманд используйте GO-- в качестве слова разделитель</span><br /><br />
        <table style="width:750px;">
            <tr>
                <td style="width:142px;"><asp:Button ID="btnGoExec" runat="server" Text="go" Width="105px" CssClass="btn btn-m"  OnClick="btnGoExec_Click"/></td>
                <td style="width:151px;"><asp:CheckBox ID="chkShowResult" runat="server" Text="Show result table" /></td>
                <td><asp:CheckBox ID="chkIsStoreProcedure" runat="server" Text="Text is stored procedure" /></td>
            </tr>
        </table>
        <br />
        <asp:Literal ID="Message" runat="server"></asp:Literal><br />
        <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CCCCCC"
            BorderStyle="None" BorderWidth="1px" CellPadding="3" Width="99%">
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        </asp:GridView>
        <br />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
    </div>
    <hr />

</asp:Content>