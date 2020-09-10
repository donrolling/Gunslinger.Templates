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

        public static DataTypeInfo GetReturnTypeInfoFromCustomAttributesWhenNeeded(MethodInfo methodInfo)
        {
            if (!_invalidReturnType.Contains(methodInfo.ReturnType.Name))
            {
                return ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(methodInfo.ReturnType);
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
                return ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(value as Type);
            }
            // take what we can get if the above returns nothing
            return ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(methodInfo.ReturnType);
        }

        public static List<DataTypeInfo> GetInputParameters(MethodInfo methodInfo)
        {
            var result = new List<DataTypeInfo>();
            var parameters = methodInfo.GetParameters();
            foreach (var param in parameters)
            {
                var dataTypeInfo = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(param.ParameterType);
                result.Add(dataTypeInfo);
            }
            return result;
        }
    }
}
