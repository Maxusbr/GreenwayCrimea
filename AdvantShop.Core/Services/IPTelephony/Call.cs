using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Customers;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.IPTelephony
{
    public enum ECallType
    {
        [Localize("Core.IPTelephony.ECallType.In")]
        In,
        [Localize("Core.IPTelephony.ECallType.Out")]
        Out,
        [Localize("Core.IPTelephony.ECallType.Missed")]
        Missed
    }

    public enum ECallHangupStatus
    {
        [Localize("Core.IPTelephony.ECallHangupStatus.None")]
        None,
        [Localize("Core.IPTelephony.ECallHangupStatus.Answer")]
        Answer,
        [Localize("Core.IPTelephony.ECallHangupStatus.Busy")]
        Busy,
        [Localize("Core.IPTelephony.ECallHangupStatus.Noanswer")]
        Noanswer,
        [Localize("Core.IPTelephony.ECallHangupStatus.Cancel")]
        Cancel,
        [Localize("Core.IPTelephony.ECallHangupStatus.Congestion")]
        Congestion,
        [Localize("Core.IPTelephony.ECallHangupStatus.Chanunavail")]
        Chanunavail
    }

    public class Call : IBizObject
    {
        public int Id { get; set; }
        /// <summary>
        /// Id звонка в системе оператора связи
        /// </summary>
        public string CallId { get; set; }
        public ECallType Type { get; set; }
        /// <summary>
        /// Номер, с которого осуществляется вызов
        /// </summary>
        public string SrcNum { get; set; }
        /// <summary>
        /// Номер, на который осуществляется вызов
        /// </summary>
        public string DstNum { get; set; }
        public string Extension { get; set; }
        public DateTime CallDate { get; set; }
        public DateTime? CallAnswerDate { get; set; }
        public int Duration { get; set; }
        public string RecordLink { get; set; }
        public bool CalledBack { get; set; }
        public ECallHangupStatus HangupStatus { get; set; }
        public EOperatorType OperatorType { get; set; }
        public int? ManagerId { get; set; }
        /// <summary>
        /// Customer's phone
        /// </summary>
        public string Phone { get; set; }
        public bool IsComplete { get; set; }

        private List<Customer> _customers;
        [JsonIgnore]
        public List<Customer> Customers
        {
            get { return _customers ?? (_customers = Phone.IsNotEmpty() ? CustomerService.GetCustomersByPhone(Phone) : new List<Customer>()); }
        }
    }
}
