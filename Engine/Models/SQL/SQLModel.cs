using Gunslinger.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Gunslinger.Models.SQL
{
    public class SQLModel : ModelBase, IProviderModel
    {
        /// <summary>
        /// todo: make this configurable
        /// </summary>
        //private static readonly List<string> _auditProperties = new List<string> { "IsActive", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" };

        public SQLKey Key { get; set; }
        public List<SQLKey> Keys { get; internal set; }
        public IEnumerable<SQLColumn> Properties { get; set; }

        //public IEnumerable<SQLColumn> AuditProperties
        //{
        //    get
        //    {
        //        return this.Properties.Where(a => _auditProperties.Contains(a.Name.Value)).ToList();
        //    }
        //}

        //public IEnumerable<SQLColumn> NonAuditProperties
        //{
        //    get
        //    {
        //        return this.Properties.Where(a => !_auditProperties.Contains(a.Name.Value)).ToList();
        //    }
        //}

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