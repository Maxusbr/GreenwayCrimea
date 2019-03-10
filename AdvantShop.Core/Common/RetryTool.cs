using System;
using System.Collections.Generic;
using System.Threading;

namespace AdvantShop.Core.Common
{
    public static class RetryTool
    {
        /// <summary>
        /// Repeat action ned times before throw exception
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">in seconds</param>
        /// <param name="retryCount">count of repeats</param>
        public static void Do(Action action, int retryInterval = 1, int retryCount = 3)
        {
            Do<object>(() =>
            {
                action();
                return null;
            }, retryInterval, retryCount);
        }

        /// <summary>
        /// Repeat action ned times before throw exception
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">in seconds</param>
        /// <param name="retryCount">count of repeats</param>
        public static T Do<T>(Func<T> action, int retryInterval = 1, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if (retry > 0)
                        Thread.Sleep(retryInterval * 1000);
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }
    }
}
