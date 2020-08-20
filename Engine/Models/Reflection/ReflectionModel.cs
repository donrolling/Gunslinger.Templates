using Gunslinger.Interfaces;
using Gunslinger.Models.SQL;
using System.Collections.Generic;

namespace Gunslinger.Models.Reflection
{
    public class ReflectionModel : ModelBase, IProviderModel
    {
        public IEnumerable<ReflectionProperty> Properties { get; set; }
    }
}