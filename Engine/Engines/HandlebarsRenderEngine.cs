using Common.BaseClasses;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using HandlebarsDotNet;
using Microsoft.Extensions.Logging;

namespace Gunslinger.Engines
{
    public class HandlebarsRenderEngine : LoggingWorker, IRenderEngine
    {
        public HandlebarsRenderEngine(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public string Render(Template template, IProviderModel model)
        {
            model.Imports = template.Imports;
            var handlebarsTemplate = Handlebars.Compile(template.TemplateText);
            var result = handlebarsTemplate(model);
            return result;
        }

        public string Render(Template template, IGroupProviderModel model)
        {
            model.Imports = template.Imports;
            var handlebarsTemplate = Handlebars.Compile(template.TemplateText);
            var result = handlebarsTemplate(model);
            return result;
        }
    }
}