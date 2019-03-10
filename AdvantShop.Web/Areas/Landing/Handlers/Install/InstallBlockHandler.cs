using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdvantShop.App.Landing.Extensions;
using AdvantShop.Catalog;
using AdvantShop.Core.Caching;
using AdvantShop.App.Landing.Domain;
using AdvantShop.App.Landing.Domain.SubBlocks;
using Newtonsoft.Json;

namespace AdvantShop.App.Landing.Handlers.Install
{
    public class InstallBlockResult
    {
        public bool Result { get; set; }
        public int BlockId { get; set; }
    }

    public class InstallBlockHandler
    {
        private string _blockName;
        private string _template;
        private int _landingId;
        private int _sortOrder;
        private LpConfiguration _configuration;
        private LpBlockService _lpBlockService;
        private Product _product;

        private readonly List<string> _simpleTypes = new List<string>() {"html", "text"};

        public InstallBlockHandler(string blockName, string template, int landingId, int sortOrder, LpConfiguration configuration)
        {
            _blockName = blockName;
            _template = template;
            _landingId = landingId;
            _sortOrder = sortOrder;
            _configuration = configuration ?? new LpConfiguration();
            _product = GetProduct();

            _lpBlockService = new LpBlockService();
        }

        public InstallBlockResult Execute()
        {
            if (string.IsNullOrWhiteSpace(_blockName) || string.IsNullOrWhiteSpace(_template) || _landingId == 0)
                return new InstallBlockResult();

            var blockConfigService = new LpBlockConfigService();

            var blockConfig = blockConfigService.Get(_blockName, _template);
            if (blockConfig == null)
                return new InstallBlockResult();

            var block = new LpBlock()
            {
                LandingId = _landingId,
                Name = _blockName,
                Enabled = true,
                Type = blockConfig.Type,
                Settings = blockConfig.Settings != null ? JsonConvert.SerializeObject(blockConfig.Settings) : null,
                SortOrder = _sortOrder
            };

            //if (block.Type == "productsView" && _configuration.Type != LpType.FewProducts)
            //    return new InstallBlockResult();

            PrepareSettings(block);

            var blockId = _lpBlockService.Add(block);


            if (blockConfig.SubBlocks != null)
            {
                var j = 0;
                foreach (var subBlock in blockConfig.SubBlocks)
                {
                    string content = null;

                    if (!string.IsNullOrWhiteSpace(subBlock.Type) && !_simpleTypes.Contains(subBlock.Type))
                    {
                        var specificSubBlock = TryGetSubBlock(subBlock.Type);
                        if (specificSubBlock != null)
                        {
                            content = specificSubBlock.GetContent(_product);
                            subBlock.Settings = specificSubBlock.GetSettings(block, _product, subBlock.Settings);
                        }
                    }

                    _lpBlockService.AddSubBlock(new LpSubBlock()
                    {
                        LandingBlockId = blockId,
                        Name = subBlock.Name,
                        Type = subBlock.Type,
                        ContentHtml = content ?? subBlock.Placeholder,
                        Settings = JsonConvert.SerializeObject(subBlock.Settings),
                        SortOrder = j
                    });
                    j += 100;
                }
            }

            return new InstallBlockResult() {Result = true, BlockId = blockId};
        }
        

        private ILpSubBlock TryGetSubBlock(string name)
        {
            var type = GetAllSubBlocksTypes().Find(x => x.Name.ToLower() == name + "subblock");

            return type != null ? (ILpSubBlock) Activator.CreateInstance(type) : null;
        }

        private List<Type> GetAllSubBlocksTypes()
        {
            var subBlockType = typeof(ILpSubBlock);

            return CacheManager.Get("LpSubBlocksAllTypes",
                () => Assembly.GetExecutingAssembly()
                        .GetTypes()
                        .Where(t => t.GetInterface(subBlockType.Name) != null)
                        .ToList());
        }

        private Product GetProduct()
        {
            Product product = null;

            if (_configuration.Type == LpType.OneProduct)
            {
                if (_configuration.Products != null && _configuration.Products.Count > 0)
                    return _configuration.Products[0];
            }

            return product;
        }

        private void PrepareSettings(LpBlock block)
        {
            if (_configuration == null)
                return;

            //if (block.Type == "buyForm")
            //{
            //    if (block.Settings == null)
            //        return;

            //    var currentTemplate = block.TryGetSettingsValue("template_selected");
            //    if (currentTemplate == null)
            //        return;

            //    block.TrySetSettingsValue("template_selected",
            //        _configuration.Type == LpType.FewProducts ? "compact" : "standart");

            //    if (_configuration.Type == LpType.Universal)
            //    {
            //        block.TrySetSettingsValue("show_price", false);
            //    }
            //}
        }

    }
}
