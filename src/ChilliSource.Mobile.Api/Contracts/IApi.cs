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
    /// Interface for <see cref="ApiManager{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IApi<T>
	{
        /// <summary>
        /// Refit Api client proxy interface
        /// </summary>
		T Client { get; }
        
        /// <summary>
        /// Json.Net serialization settings
        /// </summary>
		JsonSerializerSettings JsonSerializationSettings { get; }

        /// <summary>
        /// Base url of the Api e.g.: https://api.bluechilli.com/
        /// </summary>
		string BaseUrl { get; }
	}
}
