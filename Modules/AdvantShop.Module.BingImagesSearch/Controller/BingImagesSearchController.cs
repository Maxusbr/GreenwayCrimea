using AdvantShop.Catalog;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Module.BingImagesSearch.Domain;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AdvantShop.Module.BingImagesSearch.Controllers
{
    [AdminAuth, Module(Type = "BingImagesSearchModule")]
    public class BingImagesSearchController : ModuleController
    {
        private const int ResultsPerPage = 8;

        public enum enumObjTypes
        {
            Product = 0,
            Category = 1,
            Brand = 2
        }

        public JsonResult SearchImages(string term, int page = 0)
        {
            if (string.IsNullOrWhiteSpace(term))
                return ErrorResult("Введите название товара");

            if (!CustomerContext.CurrentCustomer.IsAdmin && !(CustomerContext.CurrentCustomer.IsModerator
                && CustomerContext.CurrentCustomer.HasRoleAction(RoleAction.Catalog)))
            {
                return ErrorResult("Доступ запрещен");
            }

            if (string.IsNullOrEmpty(BingImagesSearchSettings.ApiKey))
            {
                return ErrorResult("Пожалуйста, настройте модуль \"Поиск фотографий для товара\"");
            }

            using (var wc = new WebClient { Encoding = Encoding.UTF8 })
            {
                wc.Headers.Add("Ocp-Apim-Subscription-Key", "7590d7df692e49da8975c04dd33b0d0e"); //BingImagesSearchSettings.ApiKey);
                wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; Touch; rv:11.0) like Gecko");
                wc.Headers.Add("X-Search-ClientIP", Request.UserHostAddress);
                //wc.Headers.Add("X-Search-Location", "");
                wc.Headers.Add("Host", "api.cognitive.microsoft.com");

                string res;
                try
                {
                    //res = wc.DownloadString(
                    //    string.Format(
                    //        "https://api.cognitive.microsoft.com/bing/v5.0/images/search?q={0}&count={1}&offset={2}",
                    //        HttpUtility.UrlEncode(term),
                    //        ResultsPerPage,
                    //        page * ResultsPerPage + 1
                    //        ));
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    return ErrorResult("Не удалось получить ответ от Bing Search API. Проверьте настройки модуля \"Поиск фотографий для товара\".");
                }
                res = "{\"_type\": \"Images\", \"instrumentation\": {\"pageLoadPingUrl\": \"https:\\/\\/www.bingapis.com\\/api\\/ping\\/pageload?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&Type=Event.CPT&DATA=0\"}, \"webSearchUrl\": \"https:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=ZvAC_R4kEHbJvNi1nqDBw47azku5xCdEkTV5dURMb4k&v=1&r=https%3a%2f%2fwww.bing.com%2fimages%2fsearch%3fq%3d%25D0%2598%25D0%25B7%25D1%258F%25D1%2589%25D0%25BD%25D0%25BE%25D0%25B5%2520%25D0%25BF%25D0%25BB%25D0%25B0%25D1%2582%25D1%258C%25D0%25B5%26FORM%3dOIIARP&p=DevEx,5079.1\", \"totalEstimatedMatches\": 134, \"value\": [{\"name\": \"Изящное платье Karen Millen. Цвет , купить за 7 127р в ...\", \"webSearchUrl\": \"https:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=8r5IjX45fZIdWxtVkI5olsVh9oqYIWXPzeEmbY5KtNM&v=1&r=https%3a%2f%2fwww.bing.com%2fimages%2fsearch%3fview%3ddetailv2%26FORM%3dOIIRPO%26q%3d%25d0%2598%25d0%25b7%25d1%258f%25d1%2589%25d0%25bd%25d0%25be%25d0%25b5%2b%25d0%25bf%25d0%25bb%25d0%25b0%25d1%2582%25d1%258c%25d0%25b5%26id%3d97229393B1B130A37A7B3DA7B4F9C801B9814FED%26simid%3d608018756407066704&p=DevEx,5007.1\", \"thumbnailUrl\": \"https:\\/\\/tse3.mm.bing.net\\/th?id=OIP.tEV2Y7X9aoZqggbdjXkgewDFE8&pid=Api\", \"datePublished\": \"2017-07-13T12:03:00\", \"contentUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=_XIKzhQxw1Lt0N3UXL0U-PTJHK8C-GWvu7ClVwUQZVc&v=1&r=http%3a%2f%2fstatic1.ptecs.ru%2fP0%2f02%2f12%2f68%2f21%2f6b.jpg&p=DevEx,5009.1\", \"hostPageUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=Uow7qVyxEOCbGbvSuu97IGFoJ9OEKmhingKmubnRIYk&v=1&r=http%3a%2f%2fwww.karenmillen.ru%2fshop%2fproduct%2fizjashhnoe-plate-karen-millen-dt1531036&p=DevEx,5008.1\", \"contentSize\": \"135979 B\", \"encodingFormat\": \"jpeg\", \"hostPageDisplayUrl\": \"www.karenmillen.ru\\/shop\\/product\\/izjashhnoe-plate-karen-millen...\", \"width\": 875, \"height\": 1400, \"thumbnail\": {\"width\": 197, \"height\": 316}, \"imageInsightsToken\": \"ccid_tEV2Y7X9*mid_97229393B1B130A37A7B3DA7B4F9C801B9814FED*simid_608018756407066704\", \"imageId\": \"97229393B1B130A37A7B3DA7B4F9C801B9814FED\", \"accentColor\": \"945F37\"}, {\"name\": \"Изящное платье Karen Millen. Цвет , купить за 7 127р в ...\", \"webSearchUrl\": \"https:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=RP5WgZ5rSRr6WTxvp74Mx2nElJYdQrwAudu3mjCuILM&v=1&r=https%3a%2f%2fwww.bing.com%2fimages%2fsearch%3fview%3ddetailv2%26FORM%3dOIIRPO%26q%3d%25d0%2598%25d0%25b7%25d1%258f%25d1%2589%25d0%25bd%25d0%25be%25d0%25b5%2b%25d0%25bf%25d0%25bb%25d0%25b0%25d1%2582%25d1%258c%25d0%25b5%26id%3d5FED5D84A284EE6A12606258D232BCDF0E9B01C0%26simid%3d607996005993941223&p=DevEx,5013.1\", \"thumbnailUrl\": \"https:\\/\\/tse2.mm.bing.net\\/th?id=OIP.ZTxk5UM61L1QXZ8Aau-o9gDFE8&pid=Api\", \"datePublished\": \"2014-12-16T07:57:00\", \"contentUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=DwiQAhNAeqzEPzeXs5Y4vCoBEpv7Or6IgGGsNnktyF4&v=1&r=http%3a%2f%2fstatic1.ptecs.ru%2fP0%2f02%2f12%2f68%2f22%2f6b.jpg&p=DevEx,5015.1\", \"hostPageUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=-r64UE6wm2pJJ9B0W6Q6bsyIIoMXNXKyzTnuAL3lyVg&v=1&r=http%3a%2f%2fwww.karenmillen.ru%2fshop%2fproduct%2fizjashhnoe-plate-karen-millen-dt1531081&p=DevEx,5014.1\", \"contentSize\": \"138260 B\", \"encodingFormat\": \"jpeg\", \"hostPageDisplayUrl\": \"www.karenmillen.ru\\/shop\\/product\\/izjashhnoe-plate-karen-millen...\", \"width\": 875, \"height\": 1400, \"thumbnail\": {\"width\": 197, \"height\": 316}, \"imageInsightsToken\": \"ccid_ZTxk5UM6*mid_5FED5D84A284EE6A12606258D232BCDF0E9B01C0*simid_607996005993941223\", \"imageId\": \"5FED5D84A284EE6A12606258D232BCDF0E9B01C0\", \"accentColor\": \"392066\"}, {\"name\": \"Купить Изящное платье с длинными рукавами за 3 700 р. фото ...\", \"webSearchUrl\": \"https:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=jxPZQ6T14hVnBC_E8eHYcwam3oz2PWNGfS45L7gs-VY&v=1&r=https%3a%2f%2fwww.bing.com%2fimages%2fsearch%3fview%3ddetailv2%26FORM%3dOIIRPO%26q%3d%25d0%2598%25d0%25b7%25d1%258f%25d1%2589%25d0%25bd%25d0%25be%25d0%25b5%2b%25d0%25bf%25d0%25bb%25d0%25b0%25d1%2582%25d1%258c%25d0%25b5%26id%3dBEA7973419875F8EF2F636EC4CCA3E3FA15B5386%26simid%3d608050814044671886&p=DevEx,5019.1\", \"thumbnailUrl\": \"https:\\/\\/tse2.mm.bing.net\\/th?id=OIP.Aoa4Q7uT8RAddZxYbu_u0gDMEy&pid=Api\", \"datePublished\": \"2017-07-23T21:52:00\", \"contentUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=xpsy2EDaOM971y1cfZcZqNZtT9EPQhQMjoFh6LhbiXA&v=1&r=http%3a%2f%2fmodamio.ru%2fimage%2fcache%2fcatalog%2fproducts_pic%2f99Fem%2f999004SMOR1500%2fDSC05422-533x800.jpg&p=DevEx,5021.1\", \"hostPageUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=v6bCmOAQSScaPIgcP0eASTGl_W_8TNubGiQ8R5aB1T4&v=1&r=http%3a%2f%2fmodamio.ru%2fizyaschnoe-plate-s-dlinnymi-rukavami&p=DevEx,5020.1\", \"contentSize\": \"103476 B\", \"encodingFormat\": \"jpeg\", \"hostPageDisplayUrl\": \"modamio.ru\\/izyaschnoe-plate-s-dlinnymi-rukavami\", \"width\": 533, \"height\": 800, \"thumbnail\": {\"width\": 204, \"height\": 306}, \"imageInsightsToken\": \"ccid_Aoa4Q7uT*mid_BEA7973419875F8EF2F636EC4CCA3E3FA15B5386*simid_608050814044671886\", \"imageId\": \"BEA7973419875F8EF2F636EC4CCA3E3FA15B5386\", \"accentColor\": \"535765\"}, {\"name\": \"Изящное платье с золотым верхом и черным низом Коллекция ...\", \"webSearchUrl\": \"https:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=KEeelhLYt3h4vJAjBdiyY1k8Vegk1HcZHXqrAJVZDck&v=1&r=https%3a%2f%2fwww.bing.com%2fimages%2fsearch%3fview%3ddetailv2%26FORM%3dOIIRPO%26q%3d%25d0%2598%25d0%25b7%25d1%258f%25d1%2589%25d0%25bd%25d0%25be%25d0%25b5%2b%25d0%25bf%25d0%25bb%25d0%25b0%25d1%2582%25d1%258c%25d0%25b5%26id%3d567488AC29357C103FD57C8B1F86078BB8FAB197%26simid%3d608027908989914163&p=DevEx,5025.1\", \"thumbnailUrl\": \"https:\\/\\/tse1.mm.bing.net\\/th?id=OIP.2tELTtoV2bOo6aLOVRFVHQDYEg&pid=Api\", \"datePublished\": \"2014-01-25T18:48:00\", \"contentUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=uucXtFy5_SUhmb2a8fByMOoAZmmjWynyNIyLuBLXCFg&v=1&r=http%3a%2f%2fqtiq.ru%2fimage%2fcache%2f600-800%2fdata%2fproduct-1302%2f1302%2520(2).jpg&p=DevEx,5027.1\", \"hostPageUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=L8laGNBJGaY0qrB6Yt0uGe9XMY5rwvVW97-2A_-D7sk&v=1&r=http%3a%2f%2fqtiq.ru%2fvechernie-platya%2fizyashnoe-plate-s-zolotym-verhom-i-chernym-nizom-1302.html&p=DevEx,5026.1\", \"contentSize\": \"57605 B\", \"encodingFormat\": \"jpeg\", \"hostPageDisplayUrl\": \"qtiq.ru\\/vechernie-platya\\/izyashnoe-plate-s-zolotym-verhom-i-chernym...\", \"width\": 600, \"height\": 800, \"thumbnail\": {\"width\": 216, \"height\": 288}, \"imageInsightsToken\": \"ccid_2tELTtoV*mid_567488AC29357C103FD57C8B1F86078BB8FAB197*simid_608027908989914163\", \"imageId\": \"567488AC29357C103FD57C8B1F86078BB8FAB197\", \"accentColor\": \"8C3F4A\"}, {\"name\": \"Изящное платье nds20\", \"webSearchUrl\": \"https:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=FetBNpX7tFHPcIIZHRSpUh9aSEto4NA7-e-NCWIFwBg&v=1&r=https%3a%2f%2fwww.bing.com%2fimages%2fsearch%3fview%3ddetailv2%26FORM%3dOIIRPO%26q%3d%25d0%2598%25d0%25b7%25d1%258f%25d1%2589%25d0%25bd%25d0%25be%25d0%25b5%2b%25d0%25bf%25d0%25bb%25d0%25b0%25d1%2582%25d1%258c%25d0%25b5%26id%3d784B887E80A8EB3EA6070FA4E43A4E7C7ABCE6E7%26simid%3d607987257145821316&p=DevEx,5031.1\", \"thumbnailUrl\": \"https:\\/\\/tse1.mm.bing.net\\/th?id=OIP.vCNLuNTHB6qkMOlbQbUtcgD6D6&pid=Api\", \"datePublished\": \"2017-08-01T04:38:00\", \"contentUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=JYw2srS1C57vLgo06kbM_kGorTD561etYs1cZBv20iA&v=1&r=http%3a%2f%2fmodagroup.ru%2fimage%2fcache%2fdata%2flarge%2520%252834%2529-1500x1500.jpg&p=DevEx,5033.1\", \"hostPageUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=mCr-iliEDFGkdqo3rJnybIg5_LQHOnro_YPbMpfPgkI&v=1&r=http%3a%2f%2fmodagroup.ru%2f%25D1%2581%25D1%2582%25D0%25BE%25D0%25BA%2f%25D0%25B8%25D0%25B7%25D1%258F%25D1%2589%25D0%25BD%25D0%25BE%25D0%25B5-%25D0%25BF%25D0%25BB%25D0%25B0%25D1%2582%25D1%258C%25D0%25B5-nds20.html&p=DevEx,5032.1\", \"contentSize\": \"461087 B\", \"encodingFormat\": \"jpeg\", \"hostPageDisplayUrl\": \"modagroup.ru\\/сток\\/изящное-платье-nds20.html\", \"width\": 1500, \"height\": 1500, \"thumbnail\": {\"width\": 250, \"height\": 250}, \"imageInsightsToken\": \"ccid_vCNLuNTH*mid_784B887E80A8EB3EA6070FA4E43A4E7C7ABCE6E7*simid_607987257145821316\", \"imageId\": \"784B887E80A8EB3EA6070FA4E43A4E7C7ABCE6E7\", \"accentColor\": \"331B03\"}, {\"name\": \"Изящное платье с кружевом, размеры 36-40 ...\", \"webSearchUrl\": \"https:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=QSOtP0E60LUAiHuXaWcci4VjzZyKycAcT8WaOH33Oqk&v=1&r=https%3a%2f%2fwww.bing.com%2fimages%2fsearch%3fview%3ddetailv2%26FORM%3dOIIRPO%26q%3d%25d0%2598%25d0%25b7%25d1%258f%25d1%2589%25d0%25bd%25d0%25be%25d0%25b5%2b%25d0%25bf%25d0%25bb%25d0%25b0%25d1%2582%25d1%258c%25d0%25b5%26id%3d82EB67EBC81B8246514321188AB67199DC231E7F%26simid%3d73545042327&p=DevEx,5037.1\", \"thumbnailUrl\": \"https:\\/\\/tse4.mm.bing.net\\/th?id=OIF.rvo4CBRq21WFGNjHbU6GBw&pid=Api\", \"datePublished\": \"2017-09-24T00:26:00\", \"contentUrl\": \"https:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=XZcE1adBrI67KvHkikr7Ux1-ZDlb8di2dsoz23AJeG0&v=1&r=https%3a%2f%2fcdn.svitstyle.com.ua%2fUserFiles%2fpr%2fs2363%2fp%2f2363_4353952_1506193367-390x500.jpg&p=DevEx,5039.1\", \"hostPageUrl\": \"https:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=Sk6CzOcN0Ox1G4Z2yTTUGOdm7PQWfzniH1Eh-iAAKn0&v=1&r=https%3a%2f%2fwww.svitstyle.com.ua%2f588_ua_4353952prod.html&p=DevEx,5038.1\", \"contentSize\": \"25351 B\", \"encodingFormat\": \"jpeg\", \"hostPageDisplayUrl\": \"https:\\/\\/www.svitstyle.com.ua\\/588_ua_4353952prod.html\", \"width\": 333, \"height\": 500, \"thumbnail\": {\"width\": 199, \"height\": 300}, \"imageInsightsToken\": \"ccid_xPJQxe6E*mid_82EB67EBC81B8246514321188AB67199DC231E7F*simid_73545042327\", \"imageId\": \"82EB67EBC81B8246514321188AB67199DC231E7F\", \"accentColor\": \"576674\"}, {\"name\": \"Изящное платье marks & spencer Marks & Spencer, цена - 199 ...\", \"webSearchUrl\": \"https:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=Sa7upJFkzI8xl-zQ49Qg-BBwyUuzgULNaN8m1QNxNpU&v=1&r=https%3a%2f%2fwww.bing.com%2fimages%2fsearch%3fview%3ddetailv2%26FORM%3dOIIRPO%26q%3d%25d0%2598%25d0%25b7%25d1%258f%25d1%2589%25d0%25bd%25d0%25be%25d0%25b5%2b%25d0%25bf%25d0%25bb%25d0%25b0%25d1%2582%25d1%258c%25d0%25b5%26id%3d48A397E200FB8C3121CB2B30249E047D23BC3E94%26simid%3d608002010351665662&p=DevEx,5043.1\", \"thumbnailUrl\": \"https:\\/\\/tse3.mm.bing.net\\/th?id=OIP.uUphwi_-RnCrx6LVbDqe_gCxEs&pid=Api\", \"datePublished\": \"2015-07-27T23:28:00\", \"contentUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=I00OI_dHD8UdwwKsX49wQisG1MhetAMCLfh3cuVj4fc&v=1&r=http%3a%2f%2fstatic1.shafa.com.ua%2f3db3%2fbaeb%2f066a%2f434857-1.jpg&p=DevEx,5045.1\", \"hostPageUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=SvWwS2M6RZcLHCrW5bwe0oz_Zgvdy1GbtcBKZuPIlR4&v=1&r=http%3a%2f%2fshafa.ua%2fwomen%2fplatya%2fmidi%2f434857-izyashnoe-plate-marks-spencer&p=DevEx,5044.1\", \"contentSize\": \"34780 B\", \"encodingFormat\": \"jpeg\", \"hostPageDisplayUrl\": \"shafa.ua\\/women\\/platya\\/midi\\/434857-izyashnoe-plate-marks-spencer\", \"width\": 474, \"height\": 800, \"thumbnail\": {\"width\": 177, \"height\": 300}, \"imageInsightsToken\": \"ccid_uUphwi\\/+*mid_48A397E200FB8C3121CB2B30249E047D23BC3E94*simid_608002010351665662\", \"imageId\": \"48A397E200FB8C3121CB2B30249E047D23BC3E94\", \"accentColor\": \"416785\"}, {\"name\": \"Изящное платье в пол - Sugarlife\", \"webSearchUrl\": \"https:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=mfTwuKqOK6Gh2J_iAXhErfTDTI9aiVbYby-1U54ehWw&v=1&r=https%3a%2f%2fwww.bing.com%2fimages%2fsearch%3fview%3ddetailv2%26FORM%3dOIIRPO%26q%3d%25d0%2598%25d0%25b7%25d1%258f%25d1%2589%25d0%25bd%25d0%25be%25d0%25b5%2b%25d0%25bf%25d0%25bb%25d0%25b0%25d1%2582%25d1%258c%25d0%25b5%26id%3d7FD4E0C54CC724856BD08195AB51C4A355F741D7%26simid%3d608033436622392753&p=DevEx,5049.1\", \"thumbnailUrl\": \"https:\\/\\/tse1.mm.bing.net\\/th?id=OIP.cPWpOlW5izfHcO3ozhA7FQDMEy&pid=Api\", \"datePublished\": \"2015-08-23T21:15:00\", \"contentUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=R9jJIp8pIAln514YvqDBRz1p42wAsoaBGm_zXywOBM0&v=1&r=http%3a%2f%2fsugarbrand.ru%2fsys%2fdata%2fcatalog%2fimages%2fgall1379.jpg&p=DevEx,5051.1\", \"hostPageUrl\": \"http:\\/\\/www.bing.com\\/cr?IG=AB2C4F0F6D5F4A0F87D5E4AB17FA0FB2&CID=28D31451A40A6C7339241F5BA5AC6DC1&rd=1&h=eiZYmwRn5ADDujrjj7t8JwpehfOaeeDxZjwCw-Kvviw&v=1&r=http%3a%2f%2fsugarbrand.ru%2fcatalog%2fdetail%2f325%2f&p=DevEx,5050.1\", \"contentSize\": \"467421 B\", \"encodingFormat\": \"jpeg\", \"hostPageDisplayUrl\": \"sugarbrand.ru\\/catalog\\/detail\\/325\", \"width\": 600, \"height\": 900, \"thumbnail\": {\"width\": 204, \"height\": 306}, \"imageInsightsToken\": \"ccid_cPWpOlW5*mid_7FD4E0C54CC724856BD08195AB51C4A355F741D7*simid_608033436622392753\", \"imageId\": \"7FD4E0C54CC724856BD08195AB51C4A355F741D7\", \"accentColor\": \"3E3228\"}], \"nextOffsetAddCount\": 0, \"pivotSuggestions\": [{\"pivot\": \"Изящное\", \"suggestions\": []}, {\"pivot\": \"платье\", \"suggestions\": []}], \"displayShoppingSourcesBadges\": false, \"displayRecipeSourcesBadges\": true}";
                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<BingResponse>(res);
                if (response == null)
                {
                    return ErrorResult("Возникла непредвиденная ошибка. Проверьте настройки Bing Search API.");
                }

                if (response.Errors != null)
                {
                    return ErrorResult(response.Errors);
                }

                if (response.Value == null || response.Value.Count == 0)
                {
                    return ErrorResult("Поиск не дал результатов. Попробуйте переименовать товар.");
                }

                //response.items = response.items.Where(x => FileHelpers.CheckFileExtension(x.link, EAdvantShopFileTypes.Image)).ToList();

                return Json(new BingResponseDto
                {
                    Items = response.Value.Select(x => new BingImageDto
                    {
                        Name = x.Name,
                        ContentUrl = GetParam(x.ContentUrl),
                        Height = x.Height,
                        Width = x.Width,
                        ThumbnailUrl = x.ThumbnailUrl
                    }).ToList()
                });
            }
        }

        private string GetParam(string url)
        {
            Uri myUri = new Uri(url);
            string param = HttpUtility.ParseQueryString(myUri.Query).Get("r");
            return param;
        }

        public JsonResult SearchImagesById(int objId, PhotoType type, int page = 0)
        {
            string term = string.Empty;

            switch (type)
            {
                case PhotoType.Product:
                    var product = ProductService.GetProduct(objId);
                    term = product != null ? product.Name : null;
                    break;
                case PhotoType.CategoryIcon:
                case PhotoType.CategorySmall:
                case PhotoType.CategoryBig:
                    var category = CategoryService.GetCategory(objId);
                    term = category != null ? category.Name : null;
                    break;

                case PhotoType.Brand:
                    var brand = BrandService.GetBrandById(objId);
                    term = brand != null ? brand.Name : null;
                    break;
                case PhotoType.News:
                    var news = News.NewsService.GetNewsById(objId);
                    term = news != null ? news.Title : null;
                    break;
                default:
                    Debug.Log.Error("Wrong type for module GoogleImageSearch");
                    return ErrorResult("Wrong type for module GoogleImageSearch");
            }

            return SearchImages(term, page);
        }

        private JsonResult ErrorResult(List<BingError> errors)
        {
            return Json(new BingResponseDto { Errors = errors });
        }

        private JsonResult ErrorResult(string message)
        {
            var err = new List<BingError>() { new BingError { Message = message } };
            return Json(new BingResponseDto { Errors = err });
        }
    }
}
