[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT) ![Built With C#](https://img.shields.io/badge/Built_with-C%23-green.svg)

# ChilliSource.Mobile.Api #

This project is part of the ChilliSource framework developed by [BlueChilli](https://github.com/BlueChilli).

## Summary ##

```ChilliSource.Mobile.Api``` is a wrapper around [Refit](https://github.com/paulcbetts/refit), enhacing and simplifying REST API communication.

## Usage ##

**API Contract**

Create a [Refit](https://github.com/paulcbetts/refit) interface defining the API endpoints you would like to access:

```csharp
public interface IExampleApi
{    
    [Post("/login")]
    Task<AuthenticatedUser> Login([Body] LoginRequest request);

    [Post("/logout")]
    Task<MessageResponse> Logout();
}
```
The classes ```LoginRequest```, ```AuthenticatedUser```, and ```MessageResponse``` represent the Json entities that your API returns or expects to receive.

**API Header Fields**

Provide the header fields that your API requires for authorization and tracking by creating an ```ApiToken```:
```csharp
var info = new EnvironmentInformation(environment, appId, appVersion, 
    timeZone, platform, appName, deviceName);
var token = new ApiToken(apiKey, info, null);
```

Optionally you can also specify a user key in the token, either in the constructor or by setting the token's ```UserKey``` property.

**API Configuration**

Create the ```ApiConfiguration``` using your API URL and the API token:
```csharp
var config = new ApiConfiguration(apiUrl, () =>
{
    return new ApiAuthenticatedHandler(() =>
    {
        return Task.FromResult(token);
    }, new NoNetworkHandler(CrossConnectivity.Current, new NativeMessageHandler
    {
        AutomaticDecompression = System.Net.DecompressionMethods.GZip
    }));
});
```

**API Manager**

Create a new ```ApiManager``` using the configuration and Refit interface you created in the steps above:
```csharp
var manager = new ApiManager<IExampleApi>(config);
```

**Invoking API Endpoints**

Now you can invoke your API's endpoints in a type-safe manner through the ```ApiManager``` instance:

```csharp
var loginResult = await manager.Client
                .Login(new LoginRequest("username", "password"))
                .WaitForResponse(continueOnCapturedContext: true);

if (loginResult.IsFailure)
{
    Console.WriteLine(loginResult.StatusCode);
}
else
{
    var authenticatedUser = loginResult.Result;
    Console.WriteLine("Welcome " + authenticatedUser.Name);
}
```

You can also use a fluent syntax to check for the various result states:

```csharp
var loginResult = await manager.Client
                .Login(new LoginRequest("username", "password"))
                .WaitForResponse(continueOnCapturedContext: true)
                .OnFailure((result) =>
                    {
                        Console.WriteLine(result.StatusCode);
                    })
                .OnCancelled(() =>
                    {
                        Console.WriteLine("Login cancelled");
                    })
                .OnSuccess((result) =>
                    {
                        var authenticatedUser = result.Result;
                        Console.WriteLine("Welcome: " + authenticatedUser.Name);
                        return result;
                    }); 
```

Or asynchronous like this:

```csharp
var loginResult = await manager.Client
                .Login(new LoginRequest("username", "password"))
                .WaitForResponse(continueOnCapturedContext: true)
                .OnFailure(async result =>
                    {
                        await Task.Run(() =>
                        {
                            Console.WriteLine(result.StatusCode);
                        });  
                    })
                .OnCancelled(async result =>
                    {
                        await Task.Run(() =>
                        {
                            Console.WriteLine("Login cancelled");
                        });
                    })
                .OnSuccess(async result =>
                    {
                        await Task.Run(() =>
                        {
                            var authenticatedUser = result.Result;
                            Console.WriteLine("Welcome: " + authenticatedUser.Name);
                        });
                    }); 
```

**Testing if there is no Network Connection**

You can test if there is no network connection like this:
```csharp
    var handler = new ApiExceptionHandlerConfig(onNoNetworkConnectivity: result =>
                    {
                        hasNetwork = false;
                    });

    var result = await manager.Client
            .Login(new LoginRequest("username", "password"))
            .WaitForResponse(handler, continueOnCapturedContext: true)
            .OnFailure((failureResult) =>
                {
                    Console.WriteLine(failureResult.StatusCode);
                });
```

**Testing if the API Session has expired**

You can test if the API session has expired like this:
```csharp
var handler = new ApiExceptionHandlerConfig(onSessionExpired: result =>
                {
                    hasSessionExpired = true;
                });

var result = await manager.Client
            .Login(new LoginRequest("username", "password"))
            .WaitForResponse(handler, continueOnCapturedContext: true)
             .OnFailure((failureResult) =>
                {
                    Console.WriteLine(failureResult.StatusCode);
                });
```

## Installation ##

The library is available via NuGet [here](https://www.nuget.org/packages/ChilliSource.Mobile.Api).

## Releases ##

See the [releases](https://github.com/BlueChilli/ChilliSource.Mobile.Api/releases).

## Contribution ##

Please see the [Contribution Guide](.github/CONTRIBUTING.md).

## License ##

ChilliSource.Mobile is licensed under the [MIT license](LICENSE).

## Feedback and Contact ##

For questions or feedback, please contact [chillisource@bluechilli.com](mailto:chillisource@bluechilli.com).


