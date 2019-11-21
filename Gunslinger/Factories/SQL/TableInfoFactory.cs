using Gunslinger.Models;
using Gunslinger.Models.SQL;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Generic;

namespace Gunslinger.Factories.SQL
{
    public class TableInfoFactory
    {
        private static List<string> _excludedTypes = new List<string> { "dbo.sysdiagrams" };

        public static IEnumerable<Table> Create(SQLServerInfo sqlServerInfo, GenerationSettings settings, List<string> excludedTypes)
        {
            //can't use linq expression here because TableCollection is gross
            var tables = new List<Table>();
            sqlServerInfo.Database.Refresh();
            foreach (Table table in sqlServerInfo.Database.Tables)
            {
                var uniqueName = UniqueNameFactory.Create(table.Schema, table.Name);
                if (_excludedTypes.Contains(uniqueName))
                {
                    continue;
                }
                if (excludedTypes.Contains(uniqueName))
                {
                    continue;
                }
                tables.Add(table);
            }
            return tables;
        }
    }
}