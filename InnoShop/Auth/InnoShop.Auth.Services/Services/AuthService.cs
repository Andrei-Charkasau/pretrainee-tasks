using InnoShop.Auth.Services.DtoModels;
using System.Net.Http.Json;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7001"); // порт Auth проекта
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var loginDto = new { Email = email, Password = password };
        var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginDto);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            return result.Token;
        }
        return null;
    }
}