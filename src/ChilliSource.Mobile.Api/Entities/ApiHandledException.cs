#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using ChilliSource.Mobile.Core;
using Refit;

namespace ChilliSource.Mobile.Api
{
    /// <summary>
    /// Optional wrapper for Refit's <see cref="ApiException"/>
    /// </summary>
	public class ApiHandledException : Exception
    {
		public ApiHandledException(ApiException exception)
        {
            ApiException = exception;
        }

		public ApiHandledException(Exception exception) : base(exception.Message, exception)
		{
			ApiException = Option<ApiException>.None;
		}

        /// <summary>
        /// The <see cref="ApiException"/> as an optional
        /// </summary>
        public Option<ApiException> ApiException { get;}

        
    }
}
