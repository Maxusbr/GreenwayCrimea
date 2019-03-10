using System;

namespace AdvantShop.Core
{
    //todo this class for bl layer, but we didn't have it yet
    public class BlException: Exception
    {
        public string Property { get; protected set; }
        public BlException(string message, string prop) : base(message)
        {
            Property = prop;
        }

        public BlException(string message) : base(message)
        {
            Property = string.Empty;
        }
    }
}
