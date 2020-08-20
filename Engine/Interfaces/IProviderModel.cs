using Gunslinger.Models;
using System.Collections.Generic;

namespace Gunslinger.Interfaces
{
    public interface IProviderModel
    {
        Name Name { get; set; }
        string Description { get; set; }
        List<string> Imports { get; set; }
        string Namespace { get; set; }
        string Schema { get; set; }
    }
}