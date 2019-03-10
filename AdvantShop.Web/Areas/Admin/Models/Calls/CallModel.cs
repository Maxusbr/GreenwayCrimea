using System;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony;

namespace AdvantShop.Web.Admin.Models.Calls
{
    public class CallModel
    {
        public int Id { get; set; }
        public string CallId { get; set; }
        public ECallType Type { get; set; }
        public string SrcNum { get; set; }
        public string DstNum { get; set; }
        public string Extension { get; set; }
        public DateTime CallDate { get; set; }
        public DateTime? CallAnswerDate { get; set; }
        public int Duration { get; set; }
        public string RecordLink { get; set; }
        public bool CalledBack { get; set; }
        public ECallHangupStatus HangupStatus { get; set; }
        public EOperatorType OperatorType { get; set; }

        public string TypeString { get { return Type.ToString(); } }
        public string TypeFormatted { get { return Type.Localize(); } }
        public string CallDateFormatted { get { return CallDate.ToString("dd.MM.yyyy HH:mm"); } }
        public string DurationFormatted { get { return TimeSpan.FromSeconds(Duration).ToReadableString(); } }
        public string HangupStatusFormatted { get { return HangupStatus.Localize(); } }
    }
}
