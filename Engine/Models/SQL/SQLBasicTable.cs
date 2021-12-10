using Gunslinger.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gunslinger.Models.SQL
{
    public class SQLBasicTable : ModelBase, IProviderModel
    {
        public string UniqueName { get; set; }
        public SQLKey Key { get; set; }
        public List<SQLKey> Keys { get; set; }
        public List<SQLColumn> Columns { get; set; }
    }
}
