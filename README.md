
# .Net API Client

A .NET Core client library for interacting with the Distenka API. This package provides a simple and efficient way to enqueue, requeue, and cancel runs on the Distenka platform.

## Features

- Enqueue Runs: Submit new runs with a configuration.

- Requeue Runs: Requeue existing runs using their IDs.

- Cancel Runs: Cancel running or queued runs.

Flexible Configuration: Supports dynamic organization, environment, and processor settings.

Error Handling: Provides detailed error information for failed requests.


## Installation

Install with Nuget

```bash
  dotnet add package Distenka.Net.Client
```
    
## Configuration

Add the following section to your appsettings.json file with filling data from your account.

```javascript
{
  "Distenka": {
    "OrgName": "your-organization",
    "APIKey": "your-api-key",
    "Env": "your-environment",
    "Processor": "processor-name"
  }
}
```

## Usage
In your ```Startup.cs``` or ```Program.cs``` register the Distenka API client:

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddDistenkaApi(Configuration);
}
```

## Use the Client
Inject ```IDistenkaClient``` into your controllers or services:

```
public class HomeController : ControllerBase
{
    private readonly IDistenkaClient _distenkaClient;

    public HomeController(IDistenkaClient distenkaClient)
    {
        _distenkaClient = distenkaClient;
    }

    //The data to be processed in processor-plugin for the new run.
    //var enqueueRun = await _distenkaClient.EnqueueAsync(data);

    //Requeue a new run using the data from the run with ID.
    //var requeueRun = await _distenkaClient.RequeueAsync(runId);

    //Request the cancellation of the run with ID.
    //var cancelRun = await _distenkaClient.CancelAsync(runId);
}
```

## Error Handling

If a request fails, the client throws a DistenkaHttpRequestException with the following details:

- StatusCode: The HTTP status code returned by the API.

- Method: The HTTP method used for the request.

- RequestUri: The URL of the request.

- Content: The response body (if available).


## Contributing
Contributions are welcome! Please open an issue or submit a pull request.
