#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ChilliSource.Mobile.Api
{
    /// <summary>
    ///  A <see cref="DelegatingHandler"/> that responds to an API authentication check 
    ///  and provides the required authentication keys and relevant HTTP headers
    /// </summary>
	public class ApiAuthenticationHandler : DelegatingHandler
	{
		private const string ApiKey = "ApiKey";
		private const string UserKey = "UserKey";
		private const string Timezone = "Timezone";
		private const string AppVersion = "AppVersion";
		private const string Platform = "Platform";
		private const string AppId = "AppId";

		private readonly Func<Task<ApiToken>> _getToken;

        /// <summary>
        /// Initializes new instance with provided parameters
        /// </summary>
        /// <param name="getToken"></param>
        /// <param name="innerHandler"></param>
		public ApiAuthenticationHandler(Func<Task<ApiToken>> getToken, HttpMessageHandler innerHandler) : base(innerHandler)
		{
			_getToken = getToken;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await Task.Run(() => _getToken(), cancellationToken).ConfigureAwait(false);

			if (token != null)
			{
				SetHeader(request, ApiKey, token.ApiKey);
				SetHeader(request, AppVersion, token.AppVersion);
				SetHeader(request, Platform, token.Platform);
				SetHeader(request, Timezone, token.Timezone);
				SetHeader(request, AppId, token.AppId);

				if (!String.IsNullOrWhiteSpace(token.UserKey))
				{
					SetHeader(request, UserKey, token.UserKey);
				}
			}

			return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
		}

		private static void SetHeader(HttpRequestMessage request, string headerName, string headerValue)
		{
			if (!request.Headers.Contains(headerName))
			{
				request.Headers.Add(headerName, headerValue);
			}
		}
	}
}
