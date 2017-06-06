#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Net;
using System.Threading.Tasks;
using ChilliSource.Mobile.Core;
using Refit;
namespace ChilliSource.Mobile.Api
{
    /// <summary>
    /// <see cref="ApiManager{T}"/> extensions
    /// </summary>
	public static class ApiManagerExtenisons
	{		
        /// <summary>
        /// Waits for an API request to complete and return a response
        /// </summary>
        /// <param name="task">A <see cref="Task"/> representing the API request to be awaited</param>
        /// <param name="continueOnCapturedContext">Specifies whether the resonse should return on the same thread as the request.
        /// Setting this value to <c>false</c> can improve performance by avoiding thread context switches.</param>
        /// <returns>A <see cref="ServiceResult"/> instance representing the status of the completed request</returns>
		public static Task<ServiceResult> WaitForResponse(this Task task, bool continueOnCapturedContext = true)
		{
			return WaitForResponse(task, new ApiExceptionHandlerConfig(), continueOnCapturedContext);
		}

        /// <summary>
        /// Waits for an API request to complete and return a response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">A <see cref="Task"/> representing the API request to be awaited</param>
        /// <param name="continueOnCapturedContext">Specifies whether the resonse should return on the same thread as the request.
        /// Setting this value to <c>false</c> can improve performance by avoiding thread context switches.</param>
        /// <returns>A <see cref="ServiceResult"/> instance representing the status of the completed request</returns>
		public static Task<ServiceResult<T>> WaitForResponse<T>(this Task<T> task, bool continueOnCapturedContext = true)
		{
			return WaitForResponse(task, new ApiExceptionHandlerConfig(), continueOnCapturedContext);
		}

        /// <summary>
        /// Waits for an API request to complete and return a response
        /// </summary>
        /// <param name="task">A <see cref="Task"/> representing the API request to be awaited</param>
        /// <param name="config">A <see cref="ApiExceptionHandlerConfig"/> specifying any error handlers and logger to use</param>
        /// <param name="continueOnCapturedContext">Specifies whether the resonse should return on the same thread as the request.
        /// Setting this value to <c>false</c> can improve performance by avoiding thread context switches.</param>
        /// <returns>A <see cref="ServiceResult"/> instance representing the status of the completed request</returns>
		public static async Task<ServiceResult> WaitForResponse(this Task task, ApiExceptionHandlerConfig config, bool continueOnCapturedContext = true)
		{
			try
			{
				await task.ConfigureAwait(continueOnCapturedContext);
				return ServiceResult.AsSuccess();
			}
			catch (ApiException ex)
			{
				HandleException(ex, config);

				return ServiceResult.AsFailure(new ApiHandledException(ex), statusCode: (int)ex.StatusCode);
			}
			catch (Exception ex)
			{
				return ServiceResult.AsFailure(new ApiHandledException(ex));
			}
		}

        /// <summary>
        /// Waits for an API request to complete and return a response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">A <see cref="Task"/> representing the API request to be awaited</param>
        /// <param name="config">A <see cref="ApiExceptionHandlerConfig"/> specifying any error handlers and logger to use</param>
        /// <param name="continueOnCapturedContext">Specifies whether the resonse should return on the same thread as the request.
        /// Setting this value to <c>false</c> can improve performance by avoiding thread context switches.</param>
        /// <returns>A <see cref="ServiceResult"/> instance representing the status of the completed request</returns>
		public static async Task<ServiceResult<T>> WaitForResponse<T>(this Task<T> task, ApiExceptionHandlerConfig config, bool continueOnCapturedContext = true)
		{
			try
			{
				var result = await task.ConfigureAwait(continueOnCapturedContext);
				return ServiceResult<T>.AsSuccess(result);
			}
			catch (ApiException ex)
			{
				HandleException(ex, config);

				return ServiceResult<T>.AsFailure(new ApiHandledException(ex), statusCode: (int)ex.StatusCode);
			}
			catch (Exception ex)
			{
				return ServiceResult<T>.AsFailure(new ApiHandledException(ex));
			}
		}

        /// <summary>
        /// Asynchronously logs an exception if the API request has failed
        /// </summary>
        /// <param name="task">A <see cref="Task"/> representing the API request</param>
        /// <param name="logHandler">The logger to use</param>
        /// <returns></returns>
		public static async Task<ServiceResult> Log(this Task<ServiceResult> task, Action<Exception> logHandler)
		{
			return await task.OnFailureAsync(result =>
			{
				logHandler(result.Exception);
				return Task.Delay(0);
			});
		}

        /// <summary>
        /// Asynchronously logs an exception if the API request has failed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">A <see cref="Task"/> representing the API request</param>
        /// <param name="logHandler">The logger to use</param>
        /// <returns></returns>
        public static async Task<ServiceResult<T>> Log<T>(this Task<ServiceResult<T>> task, Action<Exception> logHandler)
		{
			return await task.OnFailureAsync(result =>
			{
				logHandler(result.Exception);
				return Task.Delay(0);
			});
		}

        internal static void HandleException(ApiException exception, ApiExceptionHandlerConfig config)
        {
            if (IsSessionExpired(exception))
            {
                HandleSessionExpiry(exception, config);
            }
            else if (HasNoNetworkConnectivity(exception, config))
            {
                HandleNoNetworkConnectivity(exception, config);
            }
            else if (!exception.HasContent)
            {
                LogException(exception, "", config.Logger);
            }
        }

        internal static void LogException(Exception exception, string message, ILogger logger = null)
        {
            logger?.Error(exception, message);
        }

        internal static bool IsSessionExpired(ApiException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                return true;
            }

            return false;
        }

        internal static void HandleSessionExpiry(ApiException exception, ApiExceptionHandlerConfig config)
        {
            config.OnSessionExpired?.Invoke(ServiceResult.AsFailure(ErrorMessages.SessionTimedout, (int)exception.StatusCode));
        }

        internal static bool HasNoNetworkConnectivity(ApiException exception, ApiExceptionHandlerConfig config)
        {
            if (exception.HasContent && exception.StatusCode == HttpStatusCode.RequestTimeout)
            {
                try
                {
                    var result = exception.GetErrorResult(ApiConfiguration.DefaultJsonSerializationSettingsFactory());
                    if (String.Equals(result.ErrorMessages(), ErrorMessages.NoNetWorkError, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                catch (Exception internalException)
                {
                    LogException(internalException, "No network connectivity");
                }
            }

            return false;
        }

        internal static void HandleNoNetworkConnectivity(ApiException exception, ApiExceptionHandlerConfig config)
        {
            var r = exception.GetErrorResult(ApiConfiguration.DefaultJsonSerializationSettingsFactory());
            config.OnNoNetworkConnectivity?.Invoke(ServiceResult.AsFailure(r.ErrorMessages(), ErrorMessages.NoNetworkErrorCode));
        }
    }
}
