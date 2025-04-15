using System.Net.Http.Json;
using System.Net.Http.Headers;
using Authentication.Shared.DTOs;

public class RentalService
{
    private readonly HttpClient _http;

    public RentalService(HttpClient http)
    {
        _http = http;
    }

    public async Task<HttpResponseMessage> RentCar(RentalDto dto, string token)
    {
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await _http.PostAsJsonAsync("api/Rental/rent", dto);
    }
}
