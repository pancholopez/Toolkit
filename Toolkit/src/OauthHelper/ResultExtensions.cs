using System;

namespace OauthHelper
{
    public static class ResultExtensions
    {
        public static Result<T> OnSuccess<T>(this Result<T> result, Action action)
        {
            if (result.IsSuccess) action();
            return result;
        }

        public static Result<T> OnSuccess<T>(this Result<T> result, Action<Result<T>> action)
        {
            if (result.IsFailure) return result;
            action(result);
            return result;
        }

        public static Result<T> OnFailure<T>(this Result<T> result, Action<Result<T>> action)
        {
            if (result.IsSuccess) return result;
            action(result);
            return result;
        }

        public static Result<TResult> Map<T, TResult>(this Result<T> result, Func<T, TResult> func) => 
            result.IsSuccess ? Result.Ok(func(result.Value)) : Result.Fail<TResult>(result.Error);
    }
}