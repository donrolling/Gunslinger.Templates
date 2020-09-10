using Gunslinger.Interfaces;
using System.Collections.Generic;

namespace Gunslinger.Models.SQL
{
    public class SQLTable : SQLBasicTable
    {
        public List<SQLForeignKey> ForeignKeys { get; set; }
    }
}