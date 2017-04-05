#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using Newtonsoft.Json;

namespace ChilliSource.Mobile.Api
{
	public static class ObjectExtensions
	{
		public static string ToJson(this object o, JsonSerializerSettings settings)
		{
			return JsonConvert.SerializeObject(o, Formatting.None, settings);
		}

		public static T FromJson<T>(this string o, JsonSerializerSettings settings)
		{
			return JsonConvert.DeserializeObject<T>(o, settings);
		}
	}
}
