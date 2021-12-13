using Gunslinger.Enum;
using Gunslinger.Models;
using Gunslinger.Models.Javascript;
using Gunslinger.Models.Reflection;
using Gunslinger.Types;
using System;
using System.Reflection;

namespace Gunslinger.Factories.Javascript
{
    public class ReflectionPropertyFactory
    {
        public static ReflectionProperty Convert(Name modelName, PropertyInfo property, Language language, Template template)
        {
            switch (language)
            {
                case Language.csharp:
                    var dataTypeInfo = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(property.PropertyType, template);
                    return new ReflectionProperty
                    {
                        Name = NameFactory.Create(property.Name, template),
                        ModelName = modelName,
                        Type = dataTypeInfo.Name.Value
                    };
                case Language.javascript:
                    var csDataTypeInfo = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(property.PropertyType, template);
                    var jsDataTypeInfo = JavascriptDataTypeConversion.Convert_CSDataType_to_JSDataType(csDataTypeInfo, template);
                    return new ReflectionProperty
                    {
                        Name = NameFactory.Create(property.Name, template),
                        ModelName = modelName,
                        Type = jsDataTypeInfo.Name.Value
                    };
                case Language.sql:
                    // sql isn't currently supported because you need info that we just don't have here
                    // it is possible, but requires some attention
                    throw new Exception($"ReflectionPropertyFactory.Convert() - Language not supported: { language }");
                case Language.html:
                default:
                    throw new Exception($"ReflectionPropertyFactory.Convert() - Enum type not matched: { language }");
            }
        }
    }
}