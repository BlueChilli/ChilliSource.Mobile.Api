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
    /// <summary>
    /// configuration which holds information about handlers and logger 
    /// </summary>
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

        /// <summary>
        /// gets action to be executed when session expires
        /// </summary>
		public Action<ServiceResult> OnSessionExpired { get; }
        /// <summary>
        /// gets action to be executed when there is no network activity
        /// </summary>
		public Action<ServiceResult> OnNoNetworkConnectivity { get; }
        /// <summary>
        /// gets logger
        /// </summary>
		public ILogger Logger { get; }
	}
}
