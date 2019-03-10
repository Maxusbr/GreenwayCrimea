//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Shipping.Sdek
{
    /// <summary>
    /// for data exchange 
    /// </summary>
    public class SdekStatusAnswer
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Object { get; set; }
    }

    /// <summary>
    /// for added service constant list
    /// </summary>
    public class AddedService
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// pickpoint object
    /// </summary>
    public class SdekPvz
    {
        public SdekPvz()
        {
            WeightLimit = new SdekPvzWeightLimit();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string CityCode { get; set; }
        public string City { get; set; }
        public string WorkTime { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public SdekPvzWeightLimit WeightLimit { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SdekPvzWeightLimit
    {
        public int? WeightMin { get; set; }
        public int? WeightMax { get; set; }
    }
}