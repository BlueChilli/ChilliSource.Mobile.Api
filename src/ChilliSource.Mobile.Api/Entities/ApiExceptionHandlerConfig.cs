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
    /// Request-specific configuration holding exception handlers and an <see cref="ILogger"/> instance
    /// </summary>
	public class ApiExceptionHandlerConfig
	{
        /// <summary>
        /// Initializes instance by setting properties to specified parameters
        /// </summary>
        /// <param name="onSessionExpired"></param>
        /// <param name="onNoNetworkConnectivity"></param>
        /// <param name="logger"></param>
		public ApiExceptionHandlerConfig(Action<ServiceResult> onSessionExpired = null,
								 Action<ServiceResult> onNoNetworkConnectivity = null,
								 ILogger logger = null)
		{
			this.OnSessionExpired = onSessionExpired;
			this.OnNoNetworkConnectivity = onNoNetworkConnectivity;
			this.Logger = logger;
		}

        /// <summary>
        /// Action to be executed when session expires
        /// </summary>
		public Action<ServiceResult> OnSessionExpired { get; }
        
        /// <summary>
        /// Action to be executed when the network connection becomes unavailable
        /// </summary>
		public Action<ServiceResult> OnNoNetworkConnectivity { get; }
        
        /// <summary>
        /// <see cref="ILogger"/> for logging exceptions for the API requested associated with this instance
        /// </summary>
		public ILogger Logger { get; }
	}
}
