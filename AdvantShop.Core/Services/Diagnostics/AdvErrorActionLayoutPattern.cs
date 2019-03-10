using AdvantShop.Diagnostics;
using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.Text;

namespace AdvantShop.Core.Services.Diagnostics
{
    public class AdvErrorActionLayoutPattern : PatternLayout
    {
        public AdvErrorActionLayoutPattern() : base()
        {
            AddConverter(new ConverterInfo
            {
                Name = "advobject",
                Type = typeof(AdvErrorConverter)
            }
            );
        }
    }

    public class AdvErrorConverter : PatternConverter
    {
        protected override void Convert(System.IO.TextWriter writer, object state)
        {
            if (state == null)
            {
                writer.Write(SystemInfo.NullText);
                return;
            }

            var loggingEvent = state as LoggingEvent;
            var exception = loggingEvent.ExceptionObject ?? loggingEvent.MessageObject as Exception;
            var mes = loggingEvent.MessageObject != null ? loggingEvent.MessageObject.ToString() : null;
            if (string.IsNullOrWhiteSpace(mes))
            {
                if (exception != null)
                {
                    mes = exception.Message;
                }
                else
                {
                    mes = loggingEvent.RenderedMessage.Length > 200
                                                              ? loggingEvent.RenderedMessage.Substring(1, 200)
                                                              : loggingEvent.RenderedMessage;
                }
            }
            var mLog = new StringBuilder();
            mLog.Append(StringToCsvCell(mes));
            mLog.Append(";");
            if (exception != null)
            {
                mLog.AppendFormat("{0};", StringToCsvCell(exception.Message));
                var currError = new AdvException(exception);
                mLog.AppendFormat("{0}", StringToCsvCell(currError.ToJsonString()));
            }
            else
            {
                mLog.Append("none;none");
            }
            writer.Write(mLog.ToString());
        }

        private static string StringToCsvCell(string str)
        {
            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n") || str.Contains(";"));
            if (!mustQuote) return str;
            var sb = new StringBuilder();
            sb.Append("\"");
            foreach (char nextChar in str)
            {
                sb.Append(nextChar);
                if (nextChar == '"')
                    sb.Append("\"");
            }
            sb.Append("\"");
            return sb.ToString();
        }
    }
}
