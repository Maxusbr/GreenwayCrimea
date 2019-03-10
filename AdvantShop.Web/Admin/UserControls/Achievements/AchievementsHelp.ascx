<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.AchievementsHelp" Codebehind="AchievementsHelp.ascx.cs" %>
<aside class="js-achievements-help achievements-help achievements-help-default-position <%=AchievementsHelpType == eAchievementsHelpType.Admin ? "achievements-help-admin" : "achievements-help-client" %>">
    <header class="js-achievements-help-title achievements-help-title">
        <%= Resources.Resource.Client_Achievements_Instruction %>: <span class="js-achievements-help-title-text"></span>
        <div class="achievements-help-window-controls">
            <i class="js-achievements-help-collapse achievements-help-collapse"></i>
            <a class="js-achievements-help-getout achievements-help-getout" href="<%=AchievementsHelpType == eAchievementsHelpType.Admin ? "achievements.aspx" : "admin/achievements.aspx" %>" target="_blank"></a>
            <i class="js-achievements-help-close achievements-help-close"></i>
        </div>
    </header>
    <div class="js-achievements-help-container">
        <div class="achievements-help-content-wrap js-achievements-help-content-wrap">
            <article class="js-achievements-help-content achievements-help-content" data-achievement-url-get="<%=AchievementsHelpType == eAchievementsHelpType.Admin ? "httphandlers/achievements/getachievementhelp.ashx" : "admin/httphandlers/achievements/getachievementhelp.ashx" %>">
            </article>
        </div>
        <footer class="achievements-help-footer">
            <a href="<%=AchievementsHelpType == eAchievementsHelpType.Admin ? "achievements.aspx" : "admin/achievements.aspx" %>" class="achievements-help-btn-next js-achievements-help-next"><%= Resources.Resource.Client_Achievements_NextStep %></a>
            <a href="javascript:void(0);" class="achievements-help-btn-ready js-achievements-help-ready"><%= Resources.Resource.Client_Achievements_Ready %></a>
        </footer>
    </div>
</aside>