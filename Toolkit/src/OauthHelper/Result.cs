using System;

namespace OauthHelper
{
    /// <summary>
    /// Represents an operation outcome
    /// </summary>
    public class Result
    {
        /// <summary>
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// </summary>
        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && error != string.Empty) throw new InvalidOperationException();
            if (!isSuccess && error == string.Empty) throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Operation Failed
        /// </summary>
        /// <param name="message">Error message</param>
        /// <returns>Failed Result Type</returns>
        public static Result Fail(string message) => new Result(false, message);

        /// <summary>
        /// Operation Failed
        /// </summary>
        /// <typeparam name="T">Error Message</typeparam>
        /// <param name="message">Expected default value</param>
        /// <returns></returns>
        public static Result<T> Fail<T>(string message) => new Result<T>(default(T), false, message);

        /// <summary>
        /// Operation succeed
        /// </summary>
        /// <returns>Result Success type</returns>
        public static Result Ok() => new Result(true, string.Empty);

        /// <summary>
        /// Operation Succeed
        /// </summary>
        /// <typeparam name="T">Expected return type</typeparam>
        /// <param name="value">Expected return value</param>
        /// <returns>Result Success type with return value</returns>
        public static Result<T> Ok<T>(T value) => new Result<T>(value, true, string.Empty);
    }

    /// <inheritdoc />
    public class Result<T> : Result
    {
        private readonly T _value;

        /// <summary>
        /// </summary>
        public T Value
        {
            get
            {
                if (!IsSuccess) throw new InvalidOperationException();
                return _value;
            }
        }

        /// <summary>
        /// </summary>
        protected internal Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            _value = value;
        }
    }
}