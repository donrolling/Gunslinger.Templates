using Gunslinger.Models;
using Gunslinger.Models.SQL;
using Gunslinger.Types;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Generic;

namespace Gunslinger.Factories.SQL
{
    public class SQLPropertyFactory
    {
        private static List<string> lengthTypes = new List<string> {
            "nvarchar",
            "varchar",
            "binary",
            "char",
            "datetime2",
            "datetimeoffset",
            "decimal",
            "nchar",
            "decimal",
            "numeric",
            "time",
            "varbinary"
        };

        private static List<string> maxTypes = new List<string> {
            "nvarcharmax",
            "varbinarymax"
        };

        public static SQLColumn Create(Name modelName, Column column, Enum.Language language)
        {
            var property = new SQLColumn
            {
                DefaultValue = column.DefaultConstraint?.Text,
                Length = column.DataType.MaximumLength,
                Name = NameFactory.Create(column.Name),
                Nullable = column.Nullable,
                PrimaryKey = column.InPrimaryKey,
                SqlDataTypeEnum = column.DataType.SqlDataType,
                SqlDataType = getDataType(Enum.Language.sql, column.DataType.SqlDataType, column.DataType.MaximumLength),
                Type = getDataType(language, column.DataType.SqlDataType, 0),
                IsInPrimaryKey = column.InPrimaryKey,
                IsForeignKey = column.IsForeignKey,
                ModelName = modelName
            };
            return property;
        }

        private static string getDataType(Enum.Language language, SqlDataType sqlDataType, int maximumLength)
        {
            switch (language)
            {
                case Enum.Language.csharp:
                    return DataTypeConversion.ConvertTo_CSDataType(sqlDataType, false);

                case Enum.Language.sql:
                    var baseType = sqlDataType.ToString().ToLower();
                    if (maxTypes.Contains(baseType))
                    {
                        return $"{ baseType.Replace("max", "") }(max)";
                    }
                    if (lengthTypes.Contains(baseType))
                    {
                        return $"{ baseType }({ maximumLength })";
                    }
                    return baseType;

                default:
                    return "string";
            }
        }
    }
}