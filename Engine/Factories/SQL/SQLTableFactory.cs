using Gunslinger.Models;
using Gunslinger.Models.SQL;
using Gunslinger.Responses;
using Microsoft.SqlServer.Management.Smo;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gunslinger.Factories.SQL
{
    public class SQLTableFactory
    {
        public static IEnumerable<SQLTable> Create(string @namespace, Enum.TemplateLanguage language, IEnumerable<Table> tables)
        {
            // get all the foreign key meta data first
            var sqlForeignKeys = ForeignKeyFactory.Create(tables);
            var sqlTables = new List<SQLTable>();
            foreach (var table in tables)
            {
                var sqlBasicTableCreateResult = createBasicTable(@namespace, language, table);
                if (sqlBasicTableCreateResult.Failure)
                {
                    continue;
                }
                var sqlTable = create(sqlBasicTableCreateResult.Result, sqlForeignKeys);
                sqlTables.Add(sqlTable);
            }
            return sqlTables;
        }

        private static SQLTable create(SQLBasicTable sqlBasicTable, List<SQLForeignKey> allSQLForeignKeys)
        {
            var sqlTable = new SQLTable();
            sqlTable.InjectFrom(sqlBasicTable);
            sqlTable.ForeignKeys = allSQLForeignKeys.Where(a => a.Reference.UniqueName == sqlBasicTable.UniqueName).ToList();
            return sqlTable;
        }

        private static OperationResult<SQLBasicTable> createBasicTable(string @namespace, Enum.TemplateLanguage language, Table table)
        {
            if (string.IsNullOrEmpty(table.Name))
            {
                throw new Exception("Table names must not be empty.");
            }
            var modelName = NameFactory.Create(table.Name);
            var sqlColumns = getSQLColumns(modelName, language, table.Columns);
            var sqlKeys = getSQLKeys(sqlColumns);
            if (!sqlKeys.Any())
            {
                return OperationResult<SQLBasicTable>.Fail();
            }
            var uniqueName = UniqueNameFactory.Create(table.Schema, table.Name);
            // kinda assuming there is only one key for now
            var entity = new SQLTable
            {
                UniqueName = uniqueName,
                Name = NameFactory.Create(table.Name),
                Schema = table.Schema,
                Key = sqlKeys.FirstOrDefault(),
                Keys = sqlKeys,
                Columns = sqlColumns,
                Namespace = @namespace
            };
            return OperationResult<SQLBasicTable>.Ok(entity);
        }

        private static List<SQLKey> getSQLKeys(List<SQLColumn> sqlColumns)
        {
            var result = new List<SQLKey>();
            foreach (var sqlColumn in sqlColumns.Where(a => a.PrimaryKey))
            {
                var key = KeyFactory.Create(sqlColumn.Name.Value, sqlColumn.SqlDataTypeEnum);
                result.Add(key);
            }
            return result;
        }

        private static List<SQLColumn> getSQLColumns(Name modelName, Enum.TemplateLanguage language, ColumnCollection columns)
        {
            var result = new List<SQLColumn>();
            foreach (Column column in columns)
            {
                var property = SQLPropertyFactory.Create(modelName, column, language);
                result.Add(property);
            }
            return result;
        }
    }
}