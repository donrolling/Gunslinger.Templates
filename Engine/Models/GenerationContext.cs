using System.Collections.Generic;

namespace Gunslinger.Models
{
    public class GenerationContext : GenerationSettings
    {
        public List<Template> Templates { get; set; } = new List<Template>();
        public List<dynamic> DataProviders { get; set; } = new List<dynamic>();
    }
}