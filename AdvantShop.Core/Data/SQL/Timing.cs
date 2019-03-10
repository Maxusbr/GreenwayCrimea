using System.Collections.Generic;

namespace AdvantShop.Core.SQL
{
    public class Profiling
    {
        public Profiling(string command, double time)
        {
            Command = command;
            Time = time;
        }

        public Profiling(string command, List<KeyValuePair<string, object>> parameters, double time)
        {
            Command = command;
            Time = time;
            Parameters = parameters;
        }

        public string Command { get; private set; }
        public List<KeyValuePair<string, object>> Parameters { get; private set; }
        public double Time { get; private set; }

    }
}
