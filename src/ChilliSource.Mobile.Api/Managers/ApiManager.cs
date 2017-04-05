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
    /// Manages the api proxy and api configuration
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class ApiManager<T> : IApi<T>
	{
		readonly ApiConfiguration _config;
		readonly Lazy<T> _client;

		public ApiManager(ApiConfiguration config)
		{
			_config = config;
			_client = new Lazy<T>(() => ApiClientFactory<T>.Create(_config.BaseUrl, _config.HttpHandlerFactory, _config.JsonSerializationSettingsFactory));
		}

		public T Client => _client.Value;

		public string BaseUrl => _config.BaseUrl;

		public JsonSerializerSettings JsonSerializationSettings => _config.JsonSerializationSettings;
	}


}
