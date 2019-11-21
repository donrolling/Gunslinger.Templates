using Gunslinger.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Gunslinger.Models.SQL
{
    public class SQLModel : ModelBase, IProviderModel
    {
        private static readonly IEnumerable<string> _inheritedIdColumnNames = new List<string> { "Id" };

        private static readonly List<string> _auditProperties = new List<string> { "IsActive", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" };

        public SQLKey Key { get; set; }
        public List<SQLKey> Keys { get; internal set; }
        public IEnumerable<SQLColumn> Properties { get; set; }

        public IEnumerable<SQLColumn> AuditProperties
        {
            get
            {
                return this.Properties.Where(a => _auditProperties.Contains(a.Name.Value)).ToList();
            }
        }

        public IEnumerable<SQLColumn> NonAuditProperties
        {
            get
            {
                return this.Properties.Where(a => !_auditProperties.Contains(a.Name.Value)).ToList();
            }
        }

        public IEnumerable<SQLColumn> NonKeyProperties
        {
            get
            {
                return this.Properties.Where(a => !a.IsInPrimaryKey);
            }
        }

        public List<SQLForeignKey> ForeignKeys { get; set; }
    }
}