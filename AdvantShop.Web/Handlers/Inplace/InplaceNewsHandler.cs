using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.News;

namespace AdvantShop.Handlers.Inplace
{
    public class InplaceNewsHandler
    {
        public bool Execute(int id, string content, NewsInplaceField field)
        {
            var newsItem = NewsService.GetNewsById(id);
            if (newsItem == null)
                return false;

            switch (field)
            {
                case NewsInplaceField.TextAnnotation:
                    newsItem.TextAnnotation = content;
                    break;
                case NewsInplaceField.TextToPublication:
                    newsItem.TextToPublication = content;
                    break;
            }

            NewsService.UpdateNews(newsItem);
            return true;
        }
    }
}