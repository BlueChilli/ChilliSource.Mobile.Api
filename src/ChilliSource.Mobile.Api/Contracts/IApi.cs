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
	public interface IApi<T>
	{
        /// <summary>
        /// gets Refit Api client proxy interface
        /// </summary>
		T Client { get; }
        /// <summary>
        /// gets json.net serialization settings
        /// </summary>
		JsonSerializerSettings JsonSerializationSettings { get; }
        /// <summary>
        /// gets base url of the api eg: https://api.bluechilli.com/
        /// </summary>
		string BaseUrl { get; }
	}
}
