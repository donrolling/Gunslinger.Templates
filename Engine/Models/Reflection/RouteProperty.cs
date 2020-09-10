using System.Collections.Generic;

namespace Gunslinger.Models.Reflection
{
    public class RouteProperty
    {
        public Name Name { get; set; }
        public DataTypeInfo ReturnType { get; set; }
        public List<DataTypeInfo> InputParameters { get; set; }
    }
}