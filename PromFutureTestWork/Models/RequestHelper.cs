using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace PromFutureTestWork.Models;

public sealed class RequestHelper : IRequestHelper
{
    private readonly HttpClient _httpClient;
    private readonly string _token = "pdebbd1b-8aba-434f-9bf6-";
    private readonly string uri = "http://178.57.218.210:198";

    public RequestHelper()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(uri);
    }

    public async Task<TerminalSettings> GetSettings()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}commands/types?token={_token}");
            response.EnsureSuccessStatusCode();

            string responseBody = response.Content.ReadAsStringAsync().Result;
            TerminalSettings settings = JsonConvert.DeserializeObject<TerminalSettings>(responseBody);
            return settings;
        }
        catch (HttpRequestException)
        {
            throw;
        }
    }

    public async Task<ResponseModel> SendCommand(TerminalBindingModel model)
    {
        try
        {
            string message = JsonConvert.SerializeObject(model);
            string adress = $"{_httpClient.BaseAddress}terminals/{model.terminal_id}/commands?token={_token}";

            HttpContent content = new StringContent(message, Encoding.UTF8, "application/json-patch+json");
            var response = await _httpClient.PostAsync(adress, content);
            string responseBody = response.Content.ReadAsStringAsync().Result;
            ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(responseBody);

            if (response.StatusCode == HttpStatusCode.OK)
                return responseModel;
            else
                return null;
        }
        catch (HttpRequestException)
        {
            throw;
        }
        
    }
}
