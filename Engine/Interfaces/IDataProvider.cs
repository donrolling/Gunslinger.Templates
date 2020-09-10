using System.Collections.Generic;
using Gunslinger.Models;

namespace Gunslinger.Interfaces
{
    public interface IDataProvider
    {
        Dictionary<string, IProviderModel> Get(GenerationSettings settings, Template template, List<string> excludedTypes);
    }
}