#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using Newtonsoft.Json;

namespace ChilliSource.Mobile.Api
{
    /// <summary>
    /// Facade for managing the initialization of the API client proxy and making API requests
    /// </summary>
    /// <typeparam name="T">The Refit interface defining a specific API implementation</typeparam>
	public class ApiManager<T> : IApi<T>
	{
		readonly ApiConfiguration _config;
		readonly Lazy<T> _client;

        /// <summary>
        /// Creates new instance by initialzing the Refit API <see cref="Client"/>
        /// </summary>
        /// <param name="config"></param>
		public ApiManager(ApiConfiguration config)
		{
			_config = config;
			_client = new Lazy<T>(() => ApiClientFactory<T>.Create(_config.BaseUrl, _config.HttpHandlerFactory, _config.JsonSerializationSettingsFactory));
		}

        /// <summary>
        /// Refit Api client proxy interface
        /// </summary>
		public T Client => _client.Value;

        /// <summary>
        /// Base url of the Api e.g.: https://api.bluechilli.com/
        /// </summary>
		public string BaseUrl => _config.BaseUrl;

        /// <summary>
        /// Json.Net serialization settings
        /// </summary>
		public JsonSerializerSettings JsonSerializationSettings => _config.JsonSerializationSettings;
	}


}
