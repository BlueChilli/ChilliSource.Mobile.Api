#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Net.Http;
using Newtonsoft.Json;
using Refit;

namespace ChilliSource.Mobile.Api
{
    /// <summary>
    /// Factory class to construct a Refit API proxy client
    /// </summary>
    /// <typeparam name="T">The Refit interface defining the API endpoints</typeparam>
	public class ApiClientFactory<T>
	{
        /// <summary>
        /// Creates a new Refit API client that provides strongly typed methods for accesing the API endpoints
        /// </summary>
        /// <param name="baseAddress">API base URL</param>
        /// <param name="messageHandlersFactory">Function that returns the Http message handler to pass to Refit's internal <see cref="HttpClient"/></param>
        /// <param name="jsonSerializerSettingsFactory">Function that returns the Json serializer settings to use</param>
        /// <returns></returns>
		protected static T CreateClient(string baseAddress, Func<HttpMessageHandler> messageHandlersFactory, Func<JsonSerializerSettings> jsonSerializerSettingsFactory)
		{
			if (messageHandlersFactory == null)
			{
				throw new ArgumentNullException(nameof(messageHandlersFactory), "Please provide factory method for setting up message handlers");
			}

			if (jsonSerializerSettingsFactory == null)
			{
				throw new ArgumentNullException(nameof(jsonSerializerSettingsFactory), "Please provide factory method for getting json.net serialization settings");
			}

			var client = new HttpClient(messageHandlersFactory())
			{
				BaseAddress = new Uri(baseAddress)
			};

			return RestService.For<T>(client, new RefitSettings()
			{
				JsonSerializerSettings = jsonSerializerSettingsFactory()
			});
		}

		internal static T Create(string baseApiUrl, Func<HttpMessageHandler> messageHandlersFactory, Func<JsonSerializerSettings> jsonSerializerSettingsFactory)
		{
			return CreateClient(baseApiUrl, messageHandlersFactory, jsonSerializerSettingsFactory);
		}
	}

}
