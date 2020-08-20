using Gunslinger.Models;
using System.Collections.Generic;

namespace Gunslinger.Interfaces
{
    public interface IGroupProviderModel
    {
        List<string> Imports { get; set; }
        string Namespace { get; set; }
        string Schema { get; set; }

        public IEnumerable<IProviderModel> Models { get; set; }
    }
}