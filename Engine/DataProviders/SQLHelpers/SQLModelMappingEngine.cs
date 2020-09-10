using Engine.Factories;
using Engine.Models.SQL;
using Engine.Types;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable PossibleMultipleEnumeration

namespace Engine.DataProviders.SQLHelpers
{
    public static class SQLModelMappingEngine
    {
        private static readonly List<string> ignoredProperties_Update = new List<string> { "CreatedById", "CreatedDate", };

        private static readonly List<string> maxTypes = new List<string> {
            "nvarchar",
            "nvarcharmax",
            "varbinarymax"
        };

        public static SQLModel Map(Table table, SQLModel model, Enum.Language language)
        {
            model.Properties = getProperties(language, table.Columns);
            model.CSharpKeySignature = string.Join(", ",
                (
                    from k in model.KeyProperties
                    select $"{ k.Type } { k.Name.LowerCamelCase }"
                ));
            model.CSharpKeyList = string.Join(", ",
                (
                    from k in model.KeyProperties
                    select $"{ k.Name.LowerCamelCase }"
                ));
            model.CSharpKeyTypeList = string.Join(", ",
                (
                    from k in model.KeyProperties
                    select $"{ k.Type }"
                ));
            model.SQLInsertSignature = getSQLInsertSignature(model.KeyProperties, model.NonKeyProperties);
            model.SQLUpdateSignature = string.Join(", ",
                (
                    from p in model.Properties
                    where !ignoredProperties_Update.Contains(p.Name.Value)
                    select $"@{ p.Name.LowerCamelCase } { getDataType(p) }"
                ));
            model.CSharpInsertCallSignature = getCSharpInsertCallSignature(model.KeyProperties, model.NonKeyProperties);
            model.CSharpUpdateCallSignature = string.Join(", ",
                (
                    from p in model.Properties
                    where !ignoredProperties_Update.Contains(p.Name.Value)
                    select $"@{ p.Name.Value }"
                ));
            model.CSharpKeyCallSignature = string.Join(", ",
                (
                    from p in model.KeyProperties
                    select $"@{ p.Name.LowerCamelCase }"
                ));
            model.SQLPKSignature = getSQLPKSignature(model.Properties);
            model.SQLPKWhere = getSQLPKWhere(model.Properties);
            model.SQLSet = getSQLSet(model.Properties);
            return model;
        }

        private static string getCSharpInsertCallSignature(IEnumerable<SQLProperty> keyProperties, IEnumerable<SQLProperty> properties)
        {
            var isAssociation = keyProperties.Count() > 1;
            if (isAssociation)
            {
                //won't need an output parameter because it is an assoc, we already know the pk, because we're passing it in
                return string.Join(", ",
                    (
                        from k in keyProperties
                        select $"@{ k.Name.Value }"
                    ));
            }

            var x = string.Join(", ",
                (
                    from k in properties
                    select $"@{ k.Name.Value }"
                ));

            var y = string.Join(", ",
                (
                    from k in keyProperties
                    select $"@{ k.Name.Value } OUTPUT"
                ));
            return $"{ x },\r\n\t{ y }";
        }

        private static string getDataType(string language, SqlDataType sqlDataType)
        {
            switch (language)
            {
                case "csharp":
                    return DataTypeConversion.ConvertTo_CSDataType(sqlDataType, false);

                case "sql":
                    return sqlDataType.ToString();

                default:
                    return "string";
            }
        }

        private static string getSQLInsertSignature(IEnumerable<SQLProperty> keyProperties, IEnumerable<SQLProperty> properties)
        {
            var isAssociation = keyProperties.Count() > 1;

            if (isAssociation)
            {
                return string.Join(
                    ",\r\n\t",
                    from p in keyProperties
                    select $"@{ p.Name.LowerCamelCase } { getDataType(p) }"
                );
            }

            var x = string.Join(
                ",\r\n\t",
                from p in properties
                select $"@{ p.Name.LowerCamelCase } { getDataType(p) }"
            );

            var y = string.Join(
                ",\r\n\t",
                from p in keyProperties
                select $"@{ p.Name.LowerCamelCase } { getDataType(p) } OUTPUT"
            );

            return $"{ x },\r\n\t{ y }";
        }

        private static string getSQLPKSignature(IEnumerable<SQLProperty> properties)
        {
            var values = new List<string>();
            foreach (var p in properties.Where(a => a.PrimaryKey))
            {
                var dataType = p.Type.ToLower();
                if (p.Type.ToLower() == "nvarchar")
                {
                    dataType += "(" + p.Length + ")";
                }
                values.Add(string.Concat("@", p.Name.LowerCamelCase, " ", dataType));
            }
            var result = string.Join(", ", values);
            return result;
        }

        private static string getSQLPKWhere(IEnumerable<SQLProperty> properties)
        {
            var values = new List<string>();
            foreach (var p in properties.Where(a => a.PrimaryKey))
            {
                values.Add(string.Concat(p.Name.Value, " = @", p.Name.LowerCamelCase));
            }
            var result = string.Join(" and ", values);
            return result;
        }

        private static string getSQLSet(IEnumerable<SQLProperty> properties)
        {
            var values = new List<string>();
            var deseProps = properties.Where(a => !a.PrimaryKey);
            foreach (var p in deseProps)
            {
                if (ignoredProperties_Update.Contains(p.Name.Value)) { continue; }
                values.Add(string.Concat("[", p.Name.Value, "] = @", p.Name.LowerCamelCase));
            }
            var result = string.Join(",\r\n\t\t", values);
            return result;
        }

        private static string handleMaxTypes(SQLProperty property, string dataType)
        {
            if (dataType.Contains("max"))
            {
                return $"{ dataType.Replace("max", "") }(max)";
            }
            else
            {
                return $"{ dataType }({ property.Length })";
            }
        }

        private static string getDataType(SQLProperty property)
        {
            var dataType = property.Type.ToLower();
            var isMaxType = maxTypes.Contains(dataType);
            if (isMaxType)
            {
                return handleMaxTypes(property, dataType);
            }
            return dataType;
        }

        private static IEnumerable<SQLProperty> getProperties(Enum.Language language, ColumnCollection columns)
        {
            var properties = new List<SQLProperty>();
            foreach (Column column in columns)
            {
                var property = new SQLProperty
                {
                    DefaultValue = column.DefaultConstraint?.Text,
                    Length = column.DataType.MaximumLength,
                    Name = NameFactory.Create(column.Name),
                    Nullable = column.Nullable,
                    PrimaryKey = column.InPrimaryKey,
                    SqlDataType = column.DataType.SqlDataType,
                    Type = getDataType(language, column.DataType.SqlDataType),
                    IsInPrimaryKey = column.InPrimaryKey
                };
                properties.Add(property);
            }
            return properties;
        }

        private static string getDataType(Enum.Language language, SqlDataType sqlDataType)
        {
            switch (language)
            {
                case Enum.Language.csharp:
                    return DataTypeConversion.ConvertTo_CSDataType(sqlDataType, false);

                case Enum.Language.sql:
                    return sqlDataType.ToString();

                default:
                    return "string";
            }
        }
    }
}