using Common.Enums;
using System;

namespace Common.Responses
{
    public class OperationResult
    {
        public bool Succeeded { get; set; }

        public bool Failed
        {
            get
            {
                return !Succeeded;
            }
        }

        /// <summary>
        /// Optional message to go with the result.  Can be null.  If the operation was unsuccessful, and there is no exception, this should be the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Optional property to signify if there was a particular property involved. Helps to give clearer errors.
        /// Example: ModelState errors display a lot better when we know which property is involved.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// If the failure was an exception, add the exception instance here, otherwise leave it null.
        /// </summary>
        public Exception Error { get; set; }

        /// <summary>
        /// Use this to help error providers to more easily form friendly errors without having to know a lot about what is going on.
        /// Example: ModelState errors display a lot better when we know which property is involved.
        /// </summary>
        public ErrorTypes ErrorType { get; set; } = ErrorTypes.Unknown;

        public static OperationResult Ok()
        {
            return new OperationResult { Succeeded = true };
        }

        public static OperationResult<U> Ok<U>(U result)
        {
            return new OperationResult<U>
            {
                Succeeded = true,
                Result = result
            };
        }

        public static OperationResult Fail(string message, string propertyName = "", ErrorTypes errorType = ErrorTypes.Unknown)
        {
            return new OperationResult
            {
                Succeeded = false,
                Message = message,
                PropertyName = propertyName,
                ErrorType = errorType
            };
        }

        public static OperationResult<U> Fail<U>(string message, string propertyName = "", ErrorTypes errorType = ErrorTypes.Unknown)
        {
            return new OperationResult<U>
            {
                Succeeded = false,
                Message = message,
                PropertyName = propertyName,
                ErrorType = errorType
            };
        }

        public static OperationResult<U> Fail<U>(string message, U result, string propertyName = "", ErrorTypes errorType = ErrorTypes.Unknown)
        {
            return new OperationResult<U>
            {
                Succeeded = false,
                Message = message,
                Result = result,
                PropertyName = propertyName,
                ErrorType = errorType
            };
        }

        public static OperationResult<U> Fail<U>(OperationResult operationResult)
        {
            return new OperationResult<U>
            {
                Succeeded = false,
                Message = operationResult.Message,
                PropertyName = operationResult.PropertyName,
                ErrorType = operationResult.ErrorType
            };
        }

        public static OperationResult<U> Fail<U>(OperationResult operationResult, U result)
        {
            return new OperationResult<U>
            {
                Succeeded = false,
                Message = operationResult.Message,
                Result = result,
                PropertyName = operationResult.PropertyName,
                ErrorType = operationResult.ErrorType
            };
        }
    }

    public class OperationResult<T> : OperationResult
    {
        /// <summary>
        /// Contains the results of a successful call.  Can be null
        /// </summary>
        public T Result { get; set; }

        public OperationResult()
        {
        }

        public OperationResult(T result)
        {
            Result = result;
        }

        public OperationResult(OperationResult copyFrom, T result)
        {
            foreach (var propInf in copyFrom.GetType().GetProperties())
            {
                propInf.SetValue(this, propInf.GetValue(copyFrom));
            }
            Result = result;
        }
    }
}