using System.Collections.Generic;

namespace Gunslinger.Models
{
    public class GenerationSettings
    {
        public string OutputDirectory { get; set; }
        public string TemplateDirectory { get; set; }
        public bool ProcessTemplateStubs { get; set; } = true;
        
        public List<Resource> Resources { get; set; }

        public List<string> ExcludeTheseEntities { get; set; } = new List<string>();
        public List<string> ExcludeTheseTemplates { get; set; } = new List<string>();
        public List<string> IncludeTheseTablesOnly { get; set; } = new List<string>();
        public List<string> IncludeTheseTemplatesOnly { get; set; } = new List<string>();
    }
}