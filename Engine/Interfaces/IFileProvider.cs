using Gunslinger.Models;

namespace Gunslinger.Interfaces
{
    public interface IFileProvider
    {
        string Get(GenerationContext context, string filename);

        string Get(string filename);
    }
}