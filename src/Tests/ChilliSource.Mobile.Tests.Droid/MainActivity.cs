#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xunit.Sdk;
using Xunit.Runners.UI;

namespace ChilliSource.Mobile.Tests.Droid
{
    [Activity(Label = "xUnit Android Runner", MainLauncher = true, Name="com.xunit.runneractivity", Theme= "@android:style/Theme.Material.Light")]
    public class MainActivity : RunnerActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            // tests can be inside the main assembly
            AddTestAssembly(Assembly.GetExecutingAssembly());

            AddExecutionAssembly(typeof(ExtensibilityPointFactory).Assembly);
            // or in any reference assemblies			

            //AddTestAssembly(typeof(PortableTests).Assembly);
            // or in any assembly that you load (since JIT is available)
            AutoStart = true;
#if __TEST__
            TerminateAfterExecution = false;
#endif
#if false
			// you can use the default or set your own custom writer (e.g. save to web site and tweet it ;-)
			Writer = new TcpTextWriter ("10.0.1.2", 16384);
			// start running the test suites as soon as the application is loaded
			AutoStart = true;
			// crash the application (to ensure it's ended) and return to springboard
			TerminateAfterExecution = true;
#endif
            // you cannot add more assemblies once calling base
            base.OnCreate(bundle);
        }
    }
}

