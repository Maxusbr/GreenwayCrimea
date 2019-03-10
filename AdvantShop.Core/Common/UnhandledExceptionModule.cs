//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Text;
using System.Threading;
using System.Web;
using AdvantShop.Diagnostics;

namespace AdvantShop.Core
{
    public class UnhandledExceptionModule : IHttpModule
    {
        static int _unhandledExceptionCount = 0;

        public void Init(HttpApplication app)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
        }

        public void Dispose()
        {
        }

        static void OnUnhandledException(object o, UnhandledExceptionEventArgs e)
        {
            // Let this occur one time for each AppDomain.
            if (Interlocked.Exchange(ref _unhandledExceptionCount, 1) != 0)
                return;

            var message = new StringBuilder("\r\n\r\nUnhandledException logged by UnhandledExceptionModule.dll:\r\n\r\nappId=");

            var appId = (string)AppDomain.CurrentDomain.GetData(".appId");
            if (appId != null)
            {
                message.Append(appId);
            }


            Exception currentException = null;
            for (currentException = (Exception)e.ExceptionObject; currentException != null; currentException = currentException.InnerException)
            {
                message.AppendFormat("\r\n\r\ntype={0}\r\n\r\nmessage={1}\r\n\r\nstack=\r\n{2}\r\n\r\n",
                                     currentException.GetType().FullName,
                                     currentException.Message,
                                     currentException.StackTrace);
                Debug.Log.Error( message.ToString(), currentException);
            }
        }
    }
}