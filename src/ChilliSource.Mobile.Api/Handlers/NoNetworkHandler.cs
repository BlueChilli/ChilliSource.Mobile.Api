#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity.Abstractions;

namespace ChilliSource.Mobile.Api
{
    /// <summary>
    /// A <see cref="DelegatingHandler"/> to check whether there is network connectivity before an API request is sent
    /// </summary>
	public class NoNetworkHandler : DelegatingHandler
	{
		private readonly IConnectivity _connectivity;

        /// <summary>
        /// Creates new instance using a <see cref="IConnectivity"/> implementation and a nested handler
        /// </summary>
        /// <param name="connectivity">Provided from the Connectiviy Plugin (https://github.com/jamesmontemagno/ConnectivityPlugin</param>)
        /// <param name="innerHandler">additional used to chain multiple handlers together</param>
		public NoNetworkHandler(IConnectivity connectivity, HttpMessageHandler innerHandler) : base(innerHandler)
		{
            _connectivity = connectivity ?? throw new ArgumentNullException(nameof(connectivity));
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (_connectivity.IsConnected)
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
