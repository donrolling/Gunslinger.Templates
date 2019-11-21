using Gunslinger.Models.SQL;
using Microsoft.SqlServer.Management.Smo;
using Omu.ValueInjecter;
using Pluralize.NET.Core;

namespace Gunslinger.Factories.SQL
{
    public class ColumnSourceFactory
    {
        public static ColumnSource Create(string tableName, string schemaName, string columnName, SqlDataType sqlDataType)
        {
            var key = KeyFactory.Create(columnName, sqlDataType);

            var columnSource = new ColumnSource
            {
                UniqueName = UniqueNameFactory.Create(schemaName, tableName),
                TablePlural = new Pluralizer().Pluralize(tableName),
                Table = NameFactory.Create(tableName),
                Schema = schemaName
            };

            columnSource.InjectFrom(key);

            return columnSource;
        }
    }
}