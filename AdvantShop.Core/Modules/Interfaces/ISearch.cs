
namespace AdvantShop.Core.Modules.Interfaces
{
    public interface ISearch : IModule
    {
        bool OverrideStandardSearch();

        string RenderContent(string term);

        string RenderBottom(string term);
    }
}