using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PearUp.CommonEntities
{
    public class Result<T> : Result
    {
        public Result(T value, bool isSuccessed, string errorMessage) : base(isSuccessed, errorMessage)
        {
            this.Value = value;
        }

        public T Value { get; set; } = default(T);

    }

    public class Result
    {

        public bool IsSuccessed { get; set; } = false;

        public List<string> Errors { get; set; } = new List<string>();

        public Result(bool isSuccessed, string errorMessage)
        {
            this.IsSuccessed = isSuccessed;
            this.Errors.Add(errorMessage);
        }

        public void AddErrorMessage(string errorMessage)
        {
            this.Errors.Add(errorMessage);
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result Fail(string errorMessage)
        {
            return new Result(false, errorMessage);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static Result<T> Fail<T>(string errorMessage)
        {
            return new Result<T>(default(T), false, errorMessage);
        }

        public string GetErrorString()
        {
            return string.Join(Environment.NewLine, this.Errors.Select(x => x).ToArray());
        }

        /// <summary>
        /// Returns failure which combined from all failures in the <paramref name="results"/> list. Error messages are separated by <paramref name="errorMessagesSeparator"/>. 
        /// If there is no failure returns success.
        /// </summary>
        /// <param name="errorMessagesSeparator">Separator for error messages.</param>
        /// <param name="results">List of results.</param>
        public static Result Combine(string errorMessagesSeparator, params Result[] results)
        {
            List<Result> failedResults = results.Where(x => !x.IsSuccessed).ToList();

            if (!failedResults.Any())
                return Ok();

            string errorMessage = string.Join(errorMessagesSeparator, failedResults.Select(x => x.GetErrorString()));
            return Fail(errorMessage);
        }

        public static Result Combine(params Result[] results)
        {
            return Combine(", ", results);
        }

        public static Result Combine<T>(params Result<T>[] results)
        {
            return Combine(", ", results);
        }

        public static Result Combine<T>(string errorMessagesSeparator, params Result<T>[] results)
        {
            Result[] untyped = results.Select(result => (Result)result).ToArray();
            return Combine(errorMessagesSeparator, untyped);
        }
    }

    public static class ResultExtensions
    {
        public static Task<Result> ToTask(this Result result)
        {
            var taskSource = new TaskCompletionSource<Result>();
            taskSource.SetResult(result);
            return taskSource.Task;
        }

        public static Task<Result<T>> ToTask<T>(this Result<T> result)
        {
            var taskSource = new TaskCompletionSource<Result<T>>();
            taskSource.SetResult(result);
            return taskSource.Task;
        }
    }
}
