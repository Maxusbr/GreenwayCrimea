<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" Inherits="Admin.Achievements_Page" Codebehind="Achievements.aspx.cs" %>

<%@ Import Namespace="AdvantShop" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="Resources" %>

<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="content-own">
        <div class="achievements-header">
            <span class="AdminHead">
                <asp:Localize ID="Localize_Admin_Achievements_Header" runat="server" Text="<%$ Resources:Resource, Admin_Achievements_Header %>"></asp:Localize></span>
        </div>
        <div class="achievements-progressbar-wrap">
            <div class="achievements-progressbar">
                <span class="achievements-progressbar-inner" style="width: <%= Percent %>%"></span>
            </div>
        </div>
        <div class="achievements-points">
            <div class="achievements-points-sum"><%= Resources.Resource.Admin_Achievements_YourPoints %>: <%= SettingsMain.AchievementsPoints %></div>
            <%= SettingsMain.AchievementsDescription%>
        </div>
        <div class="achievements-content-wrap">
            <section class="achievements-content">
                <div class="tabs tabs-hr tabs-achievements" data-plugin="tabs">
                    <div class="tabs-headers">
                        <asp:ListView runat="server" ID="lvLevels" ItemPlaceholderID="liLevelItem">
                            <LayoutTemplate>
                                <div runat="server" id="liLevelItem"></div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div data-tabs-header="true" class="tab-header tabs-header achievements-level <%# !(bool)Eval("Unlocked") ? "achievements-level-lock" : "" %> <%# (bool)Eval("Complete") ? "achievements-level-complete" : "" %>" id="tabAchievements_<%# Eval("Id") %>">
                                    <%# Eval("Title") %>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                    <div class="tabs-contents">
                        <asp:ListView runat="server" ID="lvContentsContainer" ItemPlaceholderID="contents" OnItemDataBound="lvContentsContainer_OnItemDataBound">
                            <LayoutTemplate>
                                <div id="contents" runat="server"></div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div data-tabs-content="true" class="tab-content tabs-content <%# (bool)Eval("Unlocked") ? "achievements-content-unlock" : "achievements-content-lock" %>">
                                    <asp:ListView runat="server" ID="lvAchievements" ItemPlaceholderID="liAchievementItem">
                                        <LayoutTemplate>
                                            <ul class="js-achievements-list achievements-list">
                                                <li runat="server" id="liAchievementItem"></li>
                                            </ul>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <li class="js-achievements-list-row achievements-list-row <%# (bool)Eval("Complete") ? "achievements-list-complete" : "achievements-list-progress" %>" data-achievements-id="<%# Eval("Id") %>" data-achievement-level-id="<%# LevelId %>">
                                                <figure class="achievements-list-pic">
                                                    <span class="achievements-list-pic-wrap">
                                                        <img class="achievements-list-img <%#  !(bool)Eval("Complete") && isLevelUnlock ? "achievements-list-img-hidden" : "" %>" src="<%# !isLevelUnlock ? "images/new_admin/achievements/noicon.png" : Eval("Icon") %>" alt=" <%# Eval("Title") %>" />
                                                        <%# !isLevelUnlock ? "<span class=\"achievements-list-unlock-text\">" + Resources.Resource.Admin__Achievements_NotAvalable + "</span>" : ""  %>
                                                    </span>
                                                </figure>
                                                <article class="achievements-list-info">
                                                    <h3 class="js-achievements-list-header achievements-list-header"><%# Eval("Title") %></h3>
                                                    <div class="achievements-list-descr">
                                                        <%# Eval("Description") %>
                                                    </div>
                                                    <div class="achievements-list-point">
                                                        <div class="achievements-list-point-number">+<%# Eval("Points") %></div>
                                                        <%= Resources.Resource.Admin_Achievements_Points %>
                                                    </div>
                                                    <%# isLevelUnlock && (bool)Eval("Complete") ? " <span class=\"achievement-success\"></span>": ""%>
                                                    <%# GetAdditionalLinks(LevelId, (int)Eval("Id"), Eval("Instructions")!= null ? Eval("Instructions").ToString() : "" , Eval("Recommendations")!= null ?Eval("Recommendations").ToString(): "") %>
                                                </article>
                                                <aside class="js-achievements-help-inside-static achievements-help-inside-static">
                                                    <div>
                                                        <%# Eval("Instructions") %>
                                                        <div class="achievements-next-step" style="<%# (int)Eval("Id") == AchievementLastId ? "display:none;" : "" %>">
                                                            <a href="javascript:void(0)" class="js-achievements-next-step"><%= Resources.Resource.Admin_Achievements_NextStep %></a>
                                                        </div>
                                                    </div>
                                                </aside>
                                            </li>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
