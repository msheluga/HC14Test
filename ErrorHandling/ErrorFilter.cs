using System.Collections.Concurrent;

namespace HC14Test.ErrorHandling
{
    public class ErrorFilter : IErrorFilter
    {

        private readonly ConcurrentDictionary<string, IError> _errorDictionary = new();
        /// <summary>
        /// OnError is called whenever an error occurred during execution of a query.
        /// </summary>
        /// <param name="error">The error, cannot be null.</param>
        /// <returns>The filtered error</returns>
        public IError OnError(IError error)
        {
            var errorMessage = error.Message;
            if (!_errorDictionary.ContainsKey(errorMessage))
            {
                _errorDictionary.TryAdd(errorMessage, error);
                LogError(error);
                return error;
            }
            return null;

        }

        private void LogError(IError error)
        {
            Console.WriteLine(error.Message);
        }
    }
}
