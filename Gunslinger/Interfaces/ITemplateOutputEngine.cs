using Gunslinger.Responses;

namespace Gunslinger.Interfaces
{
    public interface ITemplateOutputEngine
    {
        OperationResult Write(string path, string output);

        OperationResult Write(string destinationPath, string entityName, string schema, string output);

        OperationResult CleanupOutputDirectory(string contextTemplateDirectory);
    }
}