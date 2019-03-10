<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin.UserControls.MasterPage.TrialBlock" CodeBehind="TrialBlock.ascx.cs" %>
<%@ Import Namespace="AdvantShop.Trial" %>
<span id="pnlInfo" runat="server">
    | <%= Resources.Resource.Admin_MasterPageAdmin_TrialPeriodTill %>:
    <asp:Label runat="server" ID="lDate"></asp:Label>
</span>

<div hidden>
    <div runat="server" id="achievementsPopup" class="js-achievements-popup" style="width: 800px">
        <%= achievementsPopupMessage %>
    </div>
</div>

<!-- Yandex.Metrika counter -->
<script type="text/javascript">(function (d, w, c) { (w[c] = w[c] || []).push(function () { try { w.yaCounter16316386 = new Ya.Metrika({ id: 16316386, webvisor: true, clickmap: true, trackLinks: true, accurateTrackBounce: true, ut: "noindex" }); } catch (e) { } }); var n = d.getElementsByTagName("script")[0], s = d.createElement("script"), f = function () { n.parentNode.insertBefore(s, n); }; s.type = "text/javascript"; s.async = true; s.src = (d.location.protocol == "https:" ? "https:" : "http:") + "//mc.yandex.ru/metrika/watch.js"; if (w.opera == "[object Opera]") { d.addEventListener("DOMContentLoaded", f, false); } else { f(); } })(document, window, "yandex_metrika_callbacks");</script>
<noscript>
    <div>
        <img src="//mc.yandex.ru/watch/16316386?ut=noindex" style="position: absolute; left: -9999px;" alt="" />
    </div>
</noscript>
<!-- /Yandex.Metrika counter -->

<script>
    (function (i, s, o, g, r, a, m) {
        i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
            (i[r].q = i[r].q || []).push(arguments)
        }, i[r].l = 1 * new Date(); a = s.createElement(o),
        m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
    })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

    ga('create', 'UA-41699428-1', 'advantshop.net');
    ga('send', 'pageview');

</script>
<script>
    <%
    if (HttpContext.Current.Items["TrialEvents"] != null)
    {

        foreach (var ev in (List<KeyValuePair<TrialEvents, string>>)HttpContext.Current.Items["TrialEvents"])
        {
    %>
    ga('send', 'event', 'TrialEvents', '<%= HttpUtility.HtmlEncode(ev.Key) %>', '<%= HttpUtility.HtmlEncode(ev.Value) %>');
    <% }
    }
    %>
</script>
