namespace AdvantShop.Module.Disqus.Domain
{
    class DisqusService
    {
        public static string GetContainer()
        {
            return @"<div id=""disqus_thread""></div>
                <noscript>Please enable JavaScript to view the <a href=""https://disqus.com/?ref_noscript"">comments powered by Disqus.</a></noscript>";
        }

        public static string GetScript()
        {
            return string.Format(
                @"<script type=""text/javascript"" defer>
                    window.addEventListener('load', function load() {{
                        window.removeEventListener('load', load);
                        var dsq_max_attempts = 0;
                        var dsq_interval = setInterval(function() {{
                            dsq_max_attempts++;
                            if (document.getElementById('disqus_thread') != null) {{
                                clearInterval(dsq_interval);
                                var d = document, s = d.createElement('script');
                                s.src = '//{0}.disqus.com/embed.js';
                                s.setAttribute('data-timestamp', +new Date());
                                (d.head || d.body).appendChild(s);
                            }} else if (dsq_max_attempts == 20) {{
                                clearInterval(dsq_interval);
                            }}
                        }}, 200);
                    }});
                </script>
                <script id=""dsq-count-scr"" src=""//{0}.disqus.com/count.js"" async></script>",
                DisqusSettings.ShortName);
        }
    }
}
