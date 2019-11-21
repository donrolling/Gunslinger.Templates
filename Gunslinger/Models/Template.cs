using System.Collections.Generic;
using Gunslinger.Enum;

namespace Gunslinger.Models
{
    public class Template
    {
        public string DataProviderName { get; set; }
        public List<string> Imports { get; set; } = new List<string>();
        public bool IsStub { get; set; }
        public Language Language { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string InputRelativePath { get; set; }
        public string OutputRelativePath { get; set; }
        public bool RunOnce { get; set; }
        public string TemplateText { get; set; }
        public TemplateType Type { get; set; }
    }
}