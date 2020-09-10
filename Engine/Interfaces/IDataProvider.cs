using System.Collections.Generic;
using Common.Responses;
using Gunslinger.Models;

namespace Gunslinger.Interfaces
{
    public interface IDataProvider
    {
        /// <summary>
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="template"></param>
        /// <param name="includeTheseEntitiesOnly"></param>
        /// <param name="excludeTheseEntities"></param>
        /// <returns>A dictionary of named models. The key is the model name.</returns>
        OperationResult<Dictionary<string, IProviderModel>> Get(GenerationSettings settings, Template template, List<string> includeTheseEntitiesOnly, List<string> excludeTheseEntities);
    }
}