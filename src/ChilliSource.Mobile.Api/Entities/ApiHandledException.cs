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
using ChilliSource.Mobile.Core;
using Refit;

namespace ChilliSource.Mobile.Api
{

    /// <summary>
    /// Represent the exception return from api response
    /// </summary>
	public class ApiHandledException : Exception
    {
		public ApiHandledException(ApiException ex)
        {
            ApiException = ex;
        }

		public ApiHandledException(Exception ex) : base(ex.Message, ex)
		{
			ApiException = Option<ApiException>.None;
		}

        public Option<ApiException> ApiException { get;}

        
    }
}
