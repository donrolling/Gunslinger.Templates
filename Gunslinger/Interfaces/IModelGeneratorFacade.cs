using System.Collections.Generic;
using Gunslinger.Models;
using Gunslinger.Responses;

namespace Gunslinger.Interfaces
{
    public interface IModelGeneratorFacade
    {
        OperationResult Generate(GenerationSettings settings, IEnumerable<Template> templates);
        OperationResult GenerateMany(GenerationSettings settings, Template template);
        OperationResult GenerateOne(GenerationSettings settings, Template template);
    }
}