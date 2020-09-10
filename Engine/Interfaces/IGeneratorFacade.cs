using Gunslinger.Models;
using Gunslinger.Responses;

namespace Gunslinger.Interfaces
{
    public interface IGeneratorFacade
    {
        GenerationContext Context { get; }

        OperationResult Generate();
    }
}