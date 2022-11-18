using RestSharp;
using System.Text.Json;
using TornLogDownloader.Entities;

namespace TornLogDownloader;

public class TornApiClient
{
  private readonly RestClient _restClient;

  public TornApiClient(RestClient restClient, IConfiguration configuration)
  {
    string apiKey = configuration["TornApiKey"] ?? throw new ArgumentException(
      "Torn API key cannot be null. Please provide one by setting the environment variable TornApiKey and retry.");
    _restClient = restClient;

    _restClient.Options.BaseUrl = new Uri("https://api.torn.com");
    _restClient.DefaultParameters.AddParameter(new QueryParameter("key", apiKey));
  }

  public async Task<List<RawLog>> GetLogsAsync(long? from, long? to, CancellationToken cancellationToken)
  {
    RestRequest request = new("user");
    request.AddQueryParameter("selections", "log");

    if (from != null)
    {
      request.AddQueryParameter("from", from.ToString());
    }

    if (to != null)
    {
      request.AddQueryParameter("to", to.ToString());
    }

    RestResponse response = await _restClient.GetAsync(request, cancellationToken);

    response.ThrowIfError();

    JsonDocument document = JsonDocument.Parse(response.Content!, new JsonDocumentOptions { AllowTrailingCommas = true });

    if (!document.RootElement.TryGetProperty("log", out JsonElement log) || log.ValueKind == JsonValueKind.Null)
    {
      return new List<RawLog>();
    }

    List<RawLog> items = new();

    foreach (JsonProperty logItem in log.EnumerateObject())
    {
      RawLog item = new()
      {
        LogId = logItem.Name,
      };

      if (logItem.Value.TryGetProperty("log", out JsonElement logTypeId) && logTypeId.ValueKind == JsonValueKind.Number)
      {
        item.LogTypeId = logTypeId.GetInt32();
      }

      if (logItem.Value.TryGetProperty("title", out JsonElement title) && title.ValueKind == JsonValueKind.String)
      {
        item.Title = title.GetString();
      }

      if (logItem.Value.TryGetProperty("timestamp", out JsonElement timestamp) && timestamp.ValueKind == JsonValueKind.Number)
      {
        item.Timestamp = timestamp.GetInt64();
      }

      if (logItem.Value.TryGetProperty("category", out JsonElement categoryName) && categoryName.ValueKind == JsonValueKind.String)
      {
        item.CategoryName = categoryName.GetString();
      }

      if (logItem.Value.TryGetProperty("data", out JsonElement data) && data.ValueKind == JsonValueKind.Object)
      {
        item.Data = data.GetRawText();
      }

      if (logItem.Value.TryGetProperty("params", out JsonElement @params) && @params.ValueKind == JsonValueKind.Object)
      {
        item.Params = @params.GetRawText();
      }

      items.Add(item);
    }

    return items;
  }
}



