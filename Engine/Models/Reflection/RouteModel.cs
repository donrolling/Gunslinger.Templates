using Gunslinger.Interfaces;
using Gunslinger.Models.SQL;
using System.Collections.Generic;

namespace Gunslinger.Models.Reflection
{
    public class RouteModel : ModelBase, IProviderModel
    {
        public IEnumerable<RouteProperty> Properties { get; set; }
    }
}