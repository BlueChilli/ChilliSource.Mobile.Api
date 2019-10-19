#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChilliSource.Mobile.Api;

namespace Api
{
	public class FakeConnectivity : IConnectivity
	{
		readonly bool mockConnected;

		public FakeConnectivity(bool mockConnected)
		{
			this.mockConnected = mockConnected;
		}

		public bool IsConnected
		{
			get
			{
				return mockConnected;
			}
		}

	}
}
