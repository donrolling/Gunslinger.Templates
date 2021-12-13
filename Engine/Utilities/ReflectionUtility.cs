using Gunslinger.Models;
using Gunslinger.Models.Reflection;
using Gunslinger.Types;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Gunslinger.Utilities
{
    public class ReflectionUtility
    {
        private static readonly List<string> _invalidReturnType = new List<string> {
            "IHttpActionResult"
        };

        public static DataTypeInfo GetReturnTypeInfoFromCustomAttributesWhenNeeded(MethodInfo methodInfo, Template template)
        {
            if (!_invalidReturnType.Contains(methodInfo.ReturnType.Name))
            {
                return ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(methodInfo.ReturnType, template);
            }
            // prefer return types that are not in the list above
            var customAttributes = methodInfo.GetCustomAttributes(true);
            foreach (var customAttribute in customAttributes)
            {
                var customAttributeType = customAttribute.GetType();
                if (customAttributeType.Name != "ResponseTypeAttribute")
                {
                    continue;
                }
                var responseTypePropertyInfo = customAttributeType.GetProperty("ResponseType");
                var value = responseTypePropertyInfo.GetValue(customAttribute);
                if (value == null)
                {
                    continue;
                }
                return ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(value as Type, template);
            }
            // take what we can get if the above returns nothing
            return ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(methodInfo.ReturnType, template);
        }

        public static List<DataTypeInfo> GetInputParameters(MethodInfo methodInfo, Template template)
        {
            var result = new List<DataTypeInfo>();
            var parameters = methodInfo.GetParameters();
            foreach (var param in parameters)
            {
                var dataTypeInfo = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(param.ParameterType, template);
                result.Add(dataTypeInfo);
            }
            return result;
        }
    }
}
