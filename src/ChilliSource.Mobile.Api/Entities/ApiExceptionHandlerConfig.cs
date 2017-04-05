#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using ChilliSource.Mobile.Core;

namespace ChilliSource.Mobile.Api
{
	public class ApiExceptionHandlerConfig
	{
		public ApiExceptionHandlerConfig(Action<ServiceResult> onSessionExpired = null,
								 Action<ServiceResult> onNoNetworkConnectivity = null,
								 ILogger logger = null)
		{
			this.OnSessionExpired = onSessionExpired;
			this.OnNoNetworkConnectivity = onNoNetworkConnectivity;
			this.Logger = logger;
		}

		public Action<ServiceResult> OnSessionExpired { get; }
		public Action<ServiceResult> OnNoNetworkConnectivity { get; }
		public ILogger Logger { get; }
	}
}
