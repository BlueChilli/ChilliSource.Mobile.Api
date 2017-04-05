#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using ChilliSource.Mobile.Core;
using Newtonsoft.Json;

namespace ChilliSource.Mobile.Api
{
    /// <summary>
    /// Represents the application and authentication data to be added for every api request
    /// </summary>
	public class ApiToken
	{
		private readonly IEnvironmentInformation _environmentInformation;

		[JsonConstructorAttribute]
		public ApiToken(string apiKey,
						   IEnvironmentInformation environmentInformation,
						   string userkey)
		{
			_environmentInformation = environmentInformation;
			ApiKey = apiKey;
			UserKey = userkey;
		}

		public string ApiKey { get; }
		public string AppId => _environmentInformation.AppId;
		public string AppVersion => _environmentInformation.AppVersion;
		public string Timezone => _environmentInformation.Timezone;
		public string Platform => _environmentInformation.Platform;
		public string UserKey { get; set; }


		public ApiToken WithUserKey(string userKey)
		{
			return new ApiToken(this.ApiKey, _environmentInformation, userKey);
		}

		public ApiToken WithEnvironment(IEnvironmentInformation environment)
		{
			return new ApiToken(this.ApiKey, environment, this.UserKey);
		}

		public static ApiToken Empty => new ApiToken(String.Empty, EnvironmentInformation.Empty, String.Empty);

		public bool IsEmpty()
		{
			return String.IsNullOrWhiteSpace(ApiKey)
						 && String.IsNullOrWhiteSpace(AppId)
						 && String.IsNullOrWhiteSpace(AppVersion)
						 && String.IsNullOrWhiteSpace(Timezone)
						 && String.IsNullOrWhiteSpace(Platform)
						 && String.IsNullOrWhiteSpace(UserKey);
		}

		public bool HasUserKey()
		{
			return !String.IsNullOrWhiteSpace(UserKey);
		}

	}
}
