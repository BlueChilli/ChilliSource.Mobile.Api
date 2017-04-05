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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChilliSource.Mobile.Core;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Refit;

namespace ChilliSource.Mobile.Api
{
    /// <summary>
    /// delegation handler to check whether there are network connectivity before the api request
    /// </summary>
	public class NoNetworkHandler : DelegatingHandler
	{
		readonly IConnectivity connectivity;

		public NoNetworkHandler(IConnectivity connectivity, HttpMessageHandler innerHandler) : base(innerHandler)
		{
			if (connectivity == null)
			{
				throw new ArgumentNullException(nameof(connectivity));
			}

			this.connectivity = connectivity;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (connectivity.IsConnected)
			{
				return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
			}


			var errorResult = new ErrorResult()
			{
				ErrorMessage = ErrorMessages.NoNetWorkError
			};
			var content = new StringContent(JsonConvert.SerializeObject(errorResult));

			var response = new HttpResponseMessage(HttpStatusCode.RequestTimeout)
			{
				Content = content
			};

			return await Task.FromResult(response).ConfigureAwait(false);
		}
	}
}
