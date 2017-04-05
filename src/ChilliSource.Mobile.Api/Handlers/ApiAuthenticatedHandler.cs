#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using ChilliSource.Mobile.Core;

namespace ChilliSource.Mobile.Api
{
    /// <summary>
    /// delegation handler to handle api authentication on the request
    /// </summary>
	public class ApiAuthenticatedHandler : DelegatingHandler
	{
		private const string ApiKey = "ApiKey";
		private const string UserKey = "UserKey";
		private const string Timezone = "Timezone";
		private const string AppVersion = "AppVersion";
		private const string Platform = "Platform";
		private const string AppId = "AppId";

		private readonly Func<Task<ApiToken>> _getToken;

		public ApiAuthenticatedHandler(Func<Task<ApiToken>> getToken, HttpMessageHandler innerHandler) : base(innerHandler)
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
