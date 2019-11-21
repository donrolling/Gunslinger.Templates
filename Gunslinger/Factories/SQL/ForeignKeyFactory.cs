using System;
using System.Collections.Generic;
using Gunslinger.Models.SQL;
using Microsoft.SqlServer.Management.Smo;

namespace Gunslinger.Factories.SQL
{
    public class ForeignKeyFactory
    {
        public static SQLForeignKey Create(string sourceTableName, string sourceSchemaName, string sourceColumnName, string referenceTableName, string referenceSchemaName, string referenceColumnName, SqlDataType sqlDataType)
        {
            var sourceColumnSource = ColumnSourceFactory.Create(sourceTableName, sourceSchemaName, sourceColumnName, sqlDataType);
            var referenceColumnSource = ColumnSourceFactory.Create(referenceTableName, referenceSchemaName, referenceColumnName, sqlDataType);

            return new Models.SQL.SQLForeignKey
            {
                Reference = sourceColumnSource,
                Source = referenceColumnSource
            };
        }

        public static List<SQLForeignKey> Create(IEnumerable<Table> tables)
        {
            var sqlForeignKeys = new List<SQLForeignKey>();
            foreach (var table in tables)
            {
                foreach (ForeignKey key in table.ForeignKeys)
                {
                    var fkColumn = key.Columns[0];
                    SqlDataType sqlDataType = SqlDataType.BigInt; // just assigning a default value so the compiler doesn't get mad
                    foreach (Column column in table.Columns)
                    {
                        if (column.Name == fkColumn.Name)
                        {
                            sqlDataType = column.DataType.SqlDataType;
                        }
                    }
                    var fk = ForeignKeyFactory.Create(table.Name, table.Schema, fkColumn.Name, key.ReferencedTable, key.ReferencedTableSchema, fkColumn.ReferencedColumn, sqlDataType);
                    sqlForeignKeys.Add(fk);
                }
            }
            return sqlForeignKeys;
        }
    }
}