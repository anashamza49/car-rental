using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Authentification.JWT.Service.DTOs;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace Authentification.JWT.Web.Services
{
    public class CarService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorage;

        public CarService(HttpClient httpClient, NavigationManager navigationManager, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorage = localStorage;
        }

        public void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<HttpResponseMessage> AddCarAsync(CarDto carDto)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("JWT token is missing.");
            }

            SetAuthorizationHeader(token);

            var content = new MultipartFormDataContent
            {
                { new StringContent(carDto.Brand), "Brand" },
                { new StringContent(carDto.Model), "Model" },
                { new StringContent(carDto.Year.ToString()), "Year" },
                { new StringContent(carDto.LicensePlate), "LicensePlate" }
            };

            if (!string.IsNullOrEmpty(carDto.ImageUrl))
            {
                content.Add(new StringContent(carDto.ImageUrl), "ImageUrl");
            }

            try
            {
                var response = await _httpClient.PostAsync("api/cars", content);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the car.", ex);
            }
        }

        public async Task<List<CarDto>> GetCarsAsync(string token)
        {
            try
            {
                SetAuthorizationHeader(token);
                var response = await _httpClient.GetFromJsonAsync<List<CarDto>>("api/cars");
                return response ?? new List<CarDto>();
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

        public async Task UpdateCarAsync(CarDto car, string token)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"api/cars/{car.Id}")
            {
                Content = new StringContent(JsonSerializer.Serialize(car), Encoding.UTF8, "application/json")
            };

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Error occurred while updating the car.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the car.", ex);
            }
        }

    }
}
