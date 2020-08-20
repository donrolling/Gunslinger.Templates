using Gunslinger.Models;

namespace Gunslinger.Interfaces
{
    public interface IRenderEngine
    {
        string Render(Template template, IProviderModel model);
        string Render(Template template, IGroupProviderModel model);
    }
}