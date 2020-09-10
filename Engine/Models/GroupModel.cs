using System.Collections.Generic;
using Gunslinger.Interfaces;
using Gunslinger.Models.SQL;

namespace Gunslinger.Models
{
    public class GroupModel : IGroupProviderModel
    {
        public List<string> Imports { get; set; }
        public string Namespace { get; set; }
        public string Schema { get; set; }
        public IEnumerable<IProviderModel> Models { get; set; }
    }
}