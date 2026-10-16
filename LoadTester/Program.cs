using System.Diagnostics;

const string baseUrl = "http://localhost:5251"; 
const int requestCount = 50;

using var client = new HttpClient
{
    BaseAddress = new Uri(baseUrl),
    Timeout = TimeSpan.FromMinutes(5)
};

// Warm-up so startup time doesn't pollute the result
await client.GetAsync("/api/demo/async");

await RunTest("sync");
await RunTest("async");

async Task RunTest(string kind)
{
    Console.WriteLine($"Sending {requestCount} requests to /api/demo/{kind} ...");

    var stopwatch = Stopwatch.StartNew();

    var tasks = Enumerable.Range(0, requestCount)
        .Select(_ => client.GetAsync($"/api/demo/{kind}"));

    await Task.WhenAll(tasks);

    stopwatch.Stop();
    Console.WriteLine($"{kind}: all {requestCount} done in {stopwatch.Elapsed.TotalSeconds:F1} seconds");
}