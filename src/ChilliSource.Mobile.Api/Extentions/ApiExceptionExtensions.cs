#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using ChilliSource.Mobile.Core;
using Newtonsoft.Json;
using Refit;

namespace ChilliSource.Mobile.Api
{
	public static class ApiExceptionExtensions
	{
        /// <summary>
        /// method to extract Error Result from exception response
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
		public static ErrorResult GetErrorResults(this ApiHandledException ex, JsonSerializerSettings settings)
		{

			var errorResult = new ErrorResult()
			{
				Errors = new List<string>()
					{
						ex.Message
					}
			};

			ex.ApiException.Match(arg =>
			{
				errorResult = arg.GetErrorResults(settings);
			},
			() => { });

			return errorResult;

		}

        /// <summary>
        /// /method to extract Error Results from exception response
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
		public static ErrorResult GetErrorResults(this ApiException ex, JsonSerializerSettings settings)
		{
			try
			{
				if (ex.HasContent)
				{
					return ex.Content.FromJson<ErrorResult>(settings);
				}

				return new ErrorResult()
				{
					ErrorMessage = ex.Message
				};
			}
			catch (Exception)
			{
				return new ErrorResult()
				{
					ErrorMessage = ex.Message
				};
			}
		}
	}
}
