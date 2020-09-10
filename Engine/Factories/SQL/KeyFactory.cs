using Gunslinger.Models;
using Gunslinger.Models.SQL;
using Gunslinger.Types;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gunslinger.Factories.SQL
{
    public class KeyFactory
    {
        public static SQLKey Create(string name, SqlDataType sqlDataType)
        {
            var _name = NameFactory.Create(name);
            return Create(_name, sqlDataType);
        }

        public static SQLKey Create(Name name, SqlDataType sqlDataType)
        {
            var dataType = SQLDataTypeConversion.ConvertTo_CSDataType(sqlDataType, false);
            var dbType = SQLDataTypeConversion.ConvertTo_CSDbType(sqlDataType);
            return new SQLKey
            {
                Name = name,
                SQLDataType = sqlDataType.ToString(),
                DataType = dataType,
                DbType = dbType
            };
        }
    }
}
