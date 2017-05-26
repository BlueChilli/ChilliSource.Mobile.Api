#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Collections.Generic;
using ChilliSource.Core.Extensions;
using Newtonsoft.Json;
using Refit;

namespace ChilliSource.Mobile.Api
{
    /// <summary>
    /// Extensions for <see cref="ApiException"/>
    /// </summary>
    public static class ApiExceptionExtensions
	{
        /// <summary>
        /// Extracts an <see cref="ErrorResult"/> from the provided <paramref name="exception"/>
        /// </summary>
        /// <param name="exception">API exception response</param>
        /// <param name="settings">Serializer settings for parsing the exception message</param>
        /// <returns>A new <see cref="ErrorResult"/> representing the specified <paramref name="exception"/></returns>
		public static ErrorResult GetErrorResult(this ApiHandledException exception, JsonSerializerSettings settings)
		{

			var errorResult = new ErrorResult()
			{
				Errors = new List<string>()
					{
						exception.Message
					}
			};

			exception.ApiException.Match(arg =>
			{
				errorResult = arg.GetErrorResult(settings);
			},
			() => { });

			return errorResult;

		}

        /// <summary>
        /// Extracts an <see cref="ErrorResult"/> from the provided <paramref name="exception"/>
        /// </summary>
        /// <param name="exception">API exception response</param>
        /// <param name="settings">Serializer settings for parsing the exception message</param>
        /// <returns>A new <see cref="ErrorResult"/> representing the specified <paramref name="exception"/></returns>
		public static ErrorResult GetErrorResult(this ApiException exception, JsonSerializerSettings settings)
		{
			try
			{
				if (exception.HasContent)
				{
					return exception.Content.FromJson<ErrorResult>(settings);
				}

				return new ErrorResult()
				{
					ErrorMessage = exception.Message
				};
			}
			catch (Exception)
			{
				return new ErrorResult()
				{
					ErrorMessage = exception.Message
				};
			}
		}
	}
}
