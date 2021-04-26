using Gunslinger.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Gunslinger.Models.SQL
{
    public class SQLModel : ModelBase, IProviderModel
    {
        public SQLKey Key { get; set; }

        public List<SQLKey> Keys { get; set; }

        public IEnumerable<SQLColumn> Properties { get; set; }

        public IEnumerable<SQLColumn> AuditNonKeyProperties { get; set; }
        
        public IEnumerable<SQLColumn> NonAuditNonKeyProperties { get; set; }

        public IEnumerable<SQLColumn> KeyProperties
        {
            get
            {
                return this.Properties.Where(a => a.IsInPrimaryKey);
            }
        }

        public IEnumerable<SQLColumn> NonKeyProperties
        {
            get
            {
                return this.Properties.Where(a => !a.IsInPrimaryKey && !ForeignKeys.Any(b => b.Reference.Name.Value == a.Name.Value));
            }
        }

        public List<SQLForeignKey> ForeignKeys { get; set; } = new List<SQLForeignKey>();
    }
}