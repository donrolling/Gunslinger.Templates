using System.Collections.Generic;
using Gunslinger.Interfaces;
using Gunslinger.Models.SQL;

namespace Gunslinger.Models
{
    public class Model : ModelBase, IProviderModel
    {
        public List<Property> Properties { get; set; } = new List<Property>();
    }
}