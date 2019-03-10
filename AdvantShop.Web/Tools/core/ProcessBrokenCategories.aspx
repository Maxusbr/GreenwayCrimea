<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="Tools.core.ProcessBrokenCategories" MasterPageFile="MasterPage.master" Codebehind="ProcessBrokenCategories.aspx.cs" %>

<asp:Content runat="server" ID="cntHead" ContentPlaceHolderID="head">
    <title>AdvantShop.NET Core Tools - Processing of broken categories</title>
    <script type="text/javascript" src="../../js/jq/jquery-1.7.1.min.js"></script>
</asp:Content>
<asp:Content runat="server" ID="cntMain" ContentPlaceHolderID="main">
    <table style="border-collapse: collapse; width: 100%; padding: 0; margin: 0;">
        <tr>
            <td style="border-bottom: 1px solid black; text-align: center;">
                <h1>Processing of broken categories</h1>
            </td>
        </tr>
        <tr>
            <td>
                <div style="width: 50%; margin: auto; padding: 10px;">
                    <div style="text-align: center; padding-bottom: 10px;">
                        <asp:Button ID="btnShowBrokenCategories" runat="server" OnClick="btnShowBrokenCategoriesClick"
                            Text="Show broken categories" />
                        <asp:Button ID="btnDeleteBrokenCategories" runat="server" OnClick="btnDeleteBrokenCategoriesClick"
                            Text="Delete broken categories" />
                    </div>
                    <fieldset>
                        <legend>Broken categories without childs</legend>
                        <asp:ListView ID="lvBrokenCcategories" runat="server" ItemPlaceholderID="itemPlaceholderID">
                            <LayoutTemplate>
                                <table>
                                    <tr runat="server" id="itemPlaceholderID">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# Eval("Name") %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div style="padding: 5px;">
                                    This place for list of broken categories
                                </div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </fieldset>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
