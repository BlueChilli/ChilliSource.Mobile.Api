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
	public static class ApiManagerExtenisons
	{
		internal static void HandleException(ApiException ex, ApiExceptionHandlerConfig config)
		{
			if (IsSessionExpired(ex))
			{
				HandleSessionExpiry(ex, config);
			}
			else if (HasNoNetworkConnectivity(ex, config))
			{
				HandleNoNetworkConnectivity(ex, config);
			}
			else if (!ex.HasContent)
			{
				LogException(ex,"", config.Logger);
			}
		}

		internal static void LogException(Exception ex, string message, ILogger logger = null)
		{
			logger?.Error(ex, message);
		}

		internal static bool IsSessionExpired(ApiException ex)
		{
			if (ex.StatusCode == HttpStatusCode.Unauthorized)
			{

				return true;
			}

			return false;
		}

		internal static void HandleSessionExpiry(ApiException ex, ApiExceptionHandlerConfig config)
		{
			config.OnSessionExpired?.Invoke(ServiceResult.AsFailure(ErrorMessages.SessionTimedout, (int)ex.StatusCode));
		}

		internal static bool HasNoNetworkConnectivity(ApiException ex, ApiExceptionHandlerConfig config)
		{
			if (ex.HasContent && ex.StatusCode == HttpStatusCode.RequestTimeout)
			{
				try
				{
					var result = ex.GetErrorResults(ApiConfiguration.DefaultJsonSerializationSettingsFactory());
					if (String.Equals(result.ErrorMessages(), ErrorMessages.NoNetWorkError, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}

				}
				catch (Exception exception)
				{
					LogException(exception, "No network connectivity");
				}
			}

			return false;
		}

		internal static void HandleNoNetworkConnectivity(ApiException ex, ApiExceptionHandlerConfig config)
		{
			var r = ex.GetErrorResults(ApiConfiguration.DefaultJsonSerializationSettingsFactory());
			config.OnNoNetworkConnectivity?.Invoke(ServiceResult.AsFailure(r.ErrorMessages(), ErrorMessages.NoNetworkErrorCode));
		}

        /// <summary>
        ///  method to wait for response to come back from api request
        /// </summary>
        /// <param name="task"></param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns></returns>
		public static Task<ServiceResult> WaitForResponse(this Task task, bool continueOnCapturedContext = true)
		{
			return WaitForResponse(task, new ApiExceptionHandlerConfig(), continueOnCapturedContext);
		}
        /// <summary>
        ///  method to wait for response to come back from api request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns></returns>
		public static Task<ServiceResult<T>> WaitForResponse<T>(this Task<T> task, bool continueOnCapturedContext = true)
		{
			return WaitForResponse(task, new ApiExceptionHandlerConfig(), continueOnCapturedContext);
		}
        /// <summary>
        ///  method to wait for response to come back from api request
        /// </summary>
        /// <param name="task"></param>
        /// <param name="config"></param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns></returns>
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
        /// method to wait for response to come back from api request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="config"></param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns></returns>
		public static async Task<ServiceResult<T>> WaitForResponse<T>(this Task<T> task, ApiExceptionHandlerConfig config, bool continueOnCapturedContext = true)
		{
			try
			{
				var r = await task.ConfigureAwait(continueOnCapturedContext);
				return ServiceResult<T>.AsSuccess(r);
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
        /// log on failure result
        /// </summary>
        /// <param name="task"></param>
        /// <param name="logHandler"></param>
        /// <returns></returns>
		public static async Task<ServiceResult> Log(this Task<ServiceResult> task, Action<Exception> logHandler)
		{

			return await task
				.OnFailureAsync(r =>
			{
				logHandler(r.Exception);
				return Task.Delay(0);
			});
		}
        /// <summary>
        /// logging on failure result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="logHandler"></param>
        /// <returns></returns>
		public static async Task<ServiceResult<T>> Log<T>(this Task<ServiceResult<T>> task, Action<Exception> logHandler)
		{
			return await task
				.OnFailureAsync(r =>
			{
				logHandler(r.Exception);
				return Task.Delay(0);
			});
		}
	}
}
