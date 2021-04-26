using System.Collections.Generic;

namespace Gunslinger.Models
{
    public class GenerationSettings
    {
        public string OutputDirectory { get; set; }
        public string TemplateDirectory { get; set; }
        public bool ProcessTemplateStubs { get; set; } = true;
        public List<Resource> Resources { get; set; }
        public List<string> AuditProperties { get; set; } = new List<string>();
        public List<string> ExcludeTheseEntities { get; set; } = new List<string>();
        public List<string> ExcludeTheseTemplates { get; set; } = new List<string>();
        public List<string> IncludeTheseEntitiesOnly { get; set; } = new List<string>();
        public List<string> IncludeTheseTemplatesOnly { get; set; } = new List<string>();
    }
}