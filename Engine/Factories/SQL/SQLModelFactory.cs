using Gunslinger.Models.SQL;
using Omu.ValueInjecter;
using System.Collections.Generic;

namespace Gunslinger.Factories.SQL
{
    public class SQLModelFactory
    {
        public static IEnumerable<SQLModel> Create(IEnumerable<SQLTable> sqlTables)
        {
            var result = new List<SQLModel>();
            foreach (var sqlTable in sqlTables)
            {
                result.Add(create(sqlTable));
            }
            return result;
        }
        
        public static SQLModel Create(SQLTable sqlTable)
        {
            return create(sqlTable);
        }

        private static SQLModel create(SQLTable sqlTable)
        {
            var sqlModel = new SQLModel();
            sqlModel.InjectFrom(sqlTable);
            sqlModel.Properties = sqlTable.Columns;
            return sqlModel;
        }
    }
}