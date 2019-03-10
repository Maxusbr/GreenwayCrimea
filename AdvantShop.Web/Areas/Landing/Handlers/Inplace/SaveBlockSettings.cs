using System;
using System.Collections.Generic;
using AdvantShop.App.Landing.Domain;
using AdvantShop.App.Landing.Models;
using AdvantShop.Diagnostics;
using Newtonsoft.Json;

namespace AdvantShop.App.Landing.Handlers.Inplace
{
    public class SaveBlockSettings
    {
        private readonly int _blockId;
        private readonly string _settings;
        private readonly LpBlockService _lpBlockService;

        public SaveBlockSettings(int blockId, string settings)
        {
            _blockId = blockId;
            _settings = settings;
            
            _lpBlockService = new LpBlockService();
        }

        public ResultModel Execute()
        {
            if (_blockId == 0 || string.IsNullOrEmpty(_settings))
                return new ResultModel();

            var block = _lpBlockService.Get(_blockId);
            if (block == null)
                return new ResultModel();

            try
            {
                var settingsOld = JsonConvert.DeserializeObject<Dictionary<string, object>>(block.Settings);
                var settingsNew = JsonConvert.DeserializeObject<Dictionary<string, object>>(_settings);

                if (settingsNew == null)
                    return new ResultModel();

                if (settingsOld == null)
                    settingsOld = new Dictionary<string, object>();

                foreach (var key in settingsNew.Keys)
                {
                    if (settingsOld.ContainsKey(key))
                    {
                        settingsOld[key] = settingsNew[key];
                    }
                    else
                    {
                        settingsOld.Add(key, settingsNew[key]);
                    }
                }

                block.Settings = JsonConvert.SerializeObject(settingsOld);

                _lpBlockService.Update(block);

                return new ResultModel() {Result = true};
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return new ResultModel();
        }
    }
}
