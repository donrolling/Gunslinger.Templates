using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Gunslinger.Utilities
{
    public static class CloneExtensions
    {
        public static T Clone<T>(this T type) where T : class
        {
            var serialized = JsonConvert.SerializeObject(type);
            var deserialized = JsonConvert.DeserializeObject<T>(serialized);
            return deserialized;
        }
    }
}