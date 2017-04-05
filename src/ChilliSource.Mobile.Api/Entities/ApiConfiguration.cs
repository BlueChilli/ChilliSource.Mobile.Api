#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Net.Http;
using ChilliSource.Mobile.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ChilliSource.Mobile.Api
{
    /// <summary>
    /// Represent the configuration class of api where can configure session expiry handler and no network connection handler and json serializer settings
    /// </summary>
	public class ApiConfiguration
	{
		public ApiConfiguration(
			string baseUrl,
			Func<HttpMessageHandler> httpHandlerFactory) :
		this(baseUrl, httpHandlerFactory, DefaultJsonSerializationSettingsFactory)
		{

		}

		public ApiConfiguration(
			string baseUrl,
			Func<HttpMessageHandler> httpHandlerFactory,
			Func<JsonSerializerSettings> jsonSerializationSettingsFactory
			)
		{
			BaseUrl = baseUrl;
			JsonSerializationSettingsFactory = jsonSerializationSettingsFactory;
			HttpHandlerFactory = httpHandlerFactory;
		}

        /// <summary>
        /// handler to be exectued when api returns 401 status code this method is invoked
        /// </summary>
		public Action<ServiceResult> OnSessionExpired { get; set; }
        /// <summary>
        /// handler to be executed when there is no network connectivity at the time api call
        /// </summary>
		public Action<ServiceResult> OnNoNetworkConnectivity { get; set; }
        /// <summary>
        /// function to get json serializer setting
        /// </summary>
		public Func<JsonSerializerSettings> JsonSerializationSettingsFactory { get; }
        /// <summary>
        /// function to create http message handlers
        /// </summary>
		public Func<HttpMessageHandler> HttpHandlerFactory { get; }
        /// <summary>
        /// get base url of the api
        /// </summary>
		public string BaseUrl { get; }

        /// <summary>
        /// default json serializer settings
        /// </summary>
		public static Func<JsonSerializerSettings> DefaultJsonSerializationSettingsFactory = () =>
		{

			var settings = new JsonSerializerSettings()
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = { new StringEnumConverter(), new IsoDateTimeConverter() },
				DateFormatHandling = DateFormatHandling.IsoDateFormat,
				DateTimeZoneHandling = DateTimeZoneHandling.Utc
			};

			return settings;
		};

		public JsonSerializerSettings JsonSerializationSettings
		{
			get
			{
				if (JsonSerializationSettingsFactory != null)
				{
					return JsonSerializationSettingsFactory.Invoke();
				}

				return DefaultJsonSerializationSettingsFactory.Invoke();
			}
		}
	}

}
