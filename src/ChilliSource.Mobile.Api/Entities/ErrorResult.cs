#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ChilliSource.Mobile.Api
{
    /// <summary>
    /// Represent the error response from api
    /// </summary>
    public class ErrorResult
    {
		public ErrorResult()
		{
			Errors = new List<string>();
		}

        [JsonProperty("errors")]
        public IList<string> Errors { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

		public string ErrorMessages()
		{
			if (!String.IsNullOrWhiteSpace(ErrorMessage))
			{
				return ErrorMessage;
			}

			var builder = new StringBuilder();

			foreach (var e in Errors)
			{
				builder.AppendLine(e);
			}

			return builder.ToString();
		}
    }

}
