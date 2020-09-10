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
        public static ReflectionProperty Convert(Name modelName, PropertyInfo property, TemplateLanguage language)
        {
            switch (language)
            {
                case TemplateLanguage.csharp:
                    var dataTypeInfo = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(property.PropertyType);
                    return new ReflectionProperty
                    {
                        Name = NameFactory.Create(property.Name),
                        ModelName = modelName,
                        Type = dataTypeInfo.Name.Value
                    };
                case TemplateLanguage.javascript:
                    var csDataTypeInfo = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(property.PropertyType);
                    var jsDataTypeInfo = JavascriptDataTypeConversion.Convert_CSDataType_to_JSDataType(csDataTypeInfo);
                    return new ReflectionProperty
                    {
                        Name = NameFactory.Create(property.Name),
                        ModelName = modelName,
                        Type = jsDataTypeInfo.Name.Value
                    };
                case TemplateLanguage.sql:
                    // sql isn't currently supported because you need info that we just don't have here
                    // it is possible, but requires some attention
                    throw new Exception($"ReflectionPropertyFactory.Convert() - Language not supported: { language }");
                case TemplateLanguage.html:
                default:
                    throw new Exception($"ReflectionPropertyFactory.Convert() - Enum type not matched: { language }");
            }
        }
    }
}