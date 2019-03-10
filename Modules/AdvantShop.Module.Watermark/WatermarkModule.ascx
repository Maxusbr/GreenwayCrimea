<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="AdvantShop.Module.Watermark.Admin_WatermarkModule" Codebehind="WatermarkModule.ascx.cs" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Watermark_Header%>" /></span>
            <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources: Watermark_Position%>"></asp:Localize>
        </td>
        <td>
            <div class="div_watermark">
                <table class="table_watermark">
                    <tr>
                        <td data-x="0" data-y="0">
                        </td>
                        <td data-x="50" data-y="0">
                        </td>
                        <td data-x="100" data-y="0">
                        </td>
                    </tr>
                    <tr>
                        <td data-x="0" data-y="50">
                        </td>
                        <td data-x="50" data-y="50">
                        </td>
                        <td data-x="100" data-y="50">
                        </td>
                    </tr>
                    <tr>
                        <td data-x="0" data-y="100">
                        </td>
                        <td data-x="50" data-y="100">
                        </td>
                        <td data-x="100" data-y="100">
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hfWatermarkPositionX" runat="server" Value="0" />
                <asp:HiddenField ID="hfWatermarkPositionY" runat="server" Value="0" />
            </div>
            <br />
            <br />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="text-align: left; width: 250px;">
            <asp:Localize ID="Localize12" runat="server" Text="<%$ Resources: Watermark_Image%>"></asp:Localize>
        </td>
        <td>
            <asp:Panel ID="pnlWatermarkImage" runat="server" Width="100%">
                <div style="border-color: gray; border-style: solid; border-width: 1px; height: 60px; overflow: auto; width: 300px;">
                    <asp:Image ID="imgWatermark" runat="server" />
                </div>
                <asp:Button ID="btnDeleteWatermark" runat="server" Text="<%$ Resources: Watermark_Delete%>"
                            OnClick="btnDeleteWatermark_Click" />
            </asp:Panel>
            <asp:FileUpload ID="fuWatermarkImage" runat="server" Height="20px" Width="300px"
                            BackColor="White" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="color: red;">
            <br/>
            <asp:Literal ID="watermarkNote" runat="server" Text="<%$ Resources: Watermark_Note%>"></asp:Literal>
            <br/>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: Watermark_Save%>" />
        </td>
    </tr>
</table>
<script type="text/javascript">
    $(document).ready(function() {
        $(".div_watermark table tr td").click(function() {
            $(".div_watermark table tr td").css("background-image", "");
            $(this).css("background-image", "url('../Admin/images/td_watermark_adv.png')");
            $("#<%= hfWatermarkPositionX.ClientID %>").val($(this).attr("data-x"));
            $("#<%= hfWatermarkPositionY.ClientID %>").val($(this).attr("data-y"));
        });

        $(".div_watermark table tr td[data-x='" + $("#<%= hfWatermarkPositionX.ClientID %>").val() + "'][data-y='" + $("#<%= hfWatermarkPositionY.ClientID %>").val() + "']").css("background-image", "url('../Admin/images/td_watermark_adv.png')");
    });
</script>