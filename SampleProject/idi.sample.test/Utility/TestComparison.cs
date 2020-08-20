using Common.Enums;
using Common.Responses;
using System.Collections.Generic;

namespace idi.sample.test.Utility
{
    public static class TestComparison
    {
        public static OperationResult DeepCompare(object x, object y, List<string> ignoreProperties)
        {
            if (ReferenceEquals(x, y)) return OperationResult.Ok();
            if ((x == null) && (y == null)) return OperationResult.Ok();
            if ((x == null) || (y == null))
            {
                if (x == null)
                {
                    return OperationResult.Fail(Status.Failure, "First object is null.");
                }
                if (y == null)
                {
                    return OperationResult.Fail(Status.Failure, "Second object is null.");
                }
            }
            //Compare classes, return false if they are different
            if (x.GetType() != y.GetType()) return OperationResult.Fail(Status.Failure, "Objects are not of the same type.");

            foreach (var property in x.GetType().GetProperties())
            {
                if (ignoreProperties.Contains(property.Name))
                {
                    continue;
                }
                var xval = property.GetValue(x);
                var yval = property.GetValue(y);
                if (!xval.Equals(yval))
                {
                    return OperationResult.Fail(Status.Failure, $"Properties are not equal: { property.Name }. { xval } != { yval }");
                };
            }

            return OperationResult.Ok(Status.Success, "All compared properties are equal.");
        }
    }
}