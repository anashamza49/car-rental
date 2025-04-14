using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Authentification.JWT.Service.DTOs;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace Authentification.JWT.Web.Services
{
    public class CarService(HttpClient _httpClient, NavigationManager _navigationManager, ILocalStorageService _localStorage)
    {
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

            var content = new MultipartFormDataContent();
            content.Add(new StringContent(carDto.Brand), "Brand");
            content.Add(new StringContent(carDto.Model), "Model");
            content.Add(new StringContent(carDto.Year.ToString()), "Year");
            content.Add(new StringContent(carDto.LicensePlate), "LicensePlate");

            if (!string.IsNullOrEmpty(carDto.ImageUrl))
            {
                content.Add(new StringContent(carDto.ImageUrl), "imageUrl");
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
