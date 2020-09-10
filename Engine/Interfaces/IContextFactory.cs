using Gunslinger.Models;

namespace Gunslinger.Interfaces
{
    public interface IContextFactory
    {
        GenerationContext Create();
    }
}