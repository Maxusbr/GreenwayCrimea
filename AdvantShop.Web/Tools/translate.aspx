<%@ Page Language="C#" AutoEventWireup="true" Inherits="Tools.Translate" Codebehind="Translate.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AdvantShop.NET Tools - IIS Pool checker</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="ContentDiv">
            <asp:Button ID="Button1" runat="server" Text="translate" 
                OnClick="Button1_Click" />
            <br />
            <br />
            <asp:Literal ID="Message" runat="server"></asp:Literal>
        </div>
        
        <div style="padding: 20px 0; width: 800px">
            <asp:TextBox ID="lblRu" runat="server" TextMode="MultiLine" Width="100%" Height="500px" />
        </div>
        <div style="padding: 20px 0; width: 800px">
            <asp:TextBox ID="lblEn" runat="server" TextMode="MultiLine" Width="100%" Height="500px" />
        </div>

    </form>
</body>
</html>
