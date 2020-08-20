using Gunslinger.Models;
using Gunslinger.Responses;

namespace Gunslinger.Interfaces
{
    public interface IResourceOutputEngine
    {
        OperationResult Write(GenerationSettings settings);
    }
}