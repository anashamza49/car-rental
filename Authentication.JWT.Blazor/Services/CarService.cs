using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Authentication.JWT.Blazor.DTOs;
using System;

namespace Authentification.JWT.Web.Services
{
    public class CarService
    {
        private readonly HttpClient _httpClient;

        public CarService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // defining authorization header
        public void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<HttpResponseMessage> AddCarAsync(MultipartFormDataContent content)
        {
            try
            {
                var response = await _httpClient.PostAsync("api/cars", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.Error.WriteLine($"Error while adding car: {errorContent}");
                    throw new Exception($"Failed to add car. Status code: {response.StatusCode}, Error: {errorContent}");
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception occurred: {ex.Message}");
                throw new Exception("An unexpected error occurred while adding the car.", ex);
            }
        }

        public async Task<List<CarDto>> GetCarsAsync(string token)
        {
            try
            {
                SetAuthorizationHeader(token);

                var response = await _httpClient.GetFromJsonAsync<List<CarDto>>("api/cars");

                if (response == null)
                {
                    Console.Error.WriteLine("No cars found or there was an error fetching cars.");
                    return new List<CarDto>();
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception occurred while fetching cars: {ex.Message}");
                return new List<CarDto>();
            }
        }
        public async Task<bool> DeleteCarAsync(int carId, string token)
        {
            try
            {
                SetAuthorizationHeader(token);
                var response = await _httpClient.DeleteAsync($"api/cars/{carId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting car: {ex.Message}");
                return false;
            }
        }

    }
}
