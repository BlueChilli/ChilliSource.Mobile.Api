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
    /// Represents the application and authentication data to be added to every API request
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

        /// <summary>
        /// API authorization key
        /// </summary>
		public string ApiKey { get; }

        /// <summary>
        /// The unique id of the currently executing app
        /// </summary>
        public string AppId => _environmentInformation.AppId;

        /// <summary>
        /// The version of the currently executing app
        /// </summary>
        public string AppVersion => _environmentInformation.AppVersion;

        /// <summary>
        /// The user's time zone
        /// </summary>
        public string Timezone => _environmentInformation.Timezone;

        /// <summary>
        /// The OS on which the currently executing app is running
        /// </summary>
        public string Platform => _environmentInformation.Platform;
       
        /// <summary>
        /// User authentication key
        /// </summary>
		public string UserKey { get; set; }

        /// <summary>
        /// Returns new <see cref="ApiToken"/> instance including the provided <paramref name="userKey"/>
        /// </summary>
        /// <param name="userKey">User authentication key</param>
        /// <returns>A new <see cref="ApiToken"/></returns>
		public ApiToken WithUserKey(string userKey)
		{
			return new ApiToken(this.ApiKey, _environmentInformation, userKey);
		}

        /// <summary>
        /// Returns new <see cref="ApiToken"/> instance including the provided <paramref name="environmentInformation"/>
        /// </summary>
        /// <param name="environmentInformation"><see cref="IEnvironmentInformation"/> implementation storing app specific meta-data</param>
        /// <returns>A new <see cref="ApiToken"/></returns>
		public ApiToken WithEnvironment(IEnvironmentInformation environmentInformation)
		{
			return new ApiToken(this.ApiKey, environmentInformation, this.UserKey);
		}

        /// <summary>
        /// Returns an empty <see cref="ApiToken"/>
        /// </summary>
		public static ApiToken Empty => new ApiToken(String.Empty, EnvironmentInformation.Empty, String.Empty);

        /// <summary>
        /// Checks that this <see cref="ApiToken"/> instance is empty
        /// </summary>
        /// <returns></returns>
		public bool IsEmpty()
		{
			return String.IsNullOrWhiteSpace(ApiKey)
						 && String.IsNullOrWhiteSpace(AppId)
						 && String.IsNullOrWhiteSpace(AppVersion)
						 && String.IsNullOrWhiteSpace(Timezone)
						 && String.IsNullOrWhiteSpace(Platform)
						 && String.IsNullOrWhiteSpace(UserKey);
		}

        /// <summary>
        /// Checks that this <see cref="ApiToken"/> instance has a <see cref="UserKey"/>
        /// </summary>
        /// <returns></returns>
		public bool HasUserKey()
		{
			return !String.IsNullOrWhiteSpace(UserKey);
		}

	}
}
