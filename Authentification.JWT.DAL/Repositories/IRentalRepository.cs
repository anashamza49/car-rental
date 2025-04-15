using Authentification.JWT.DAL.Models;

public interface IRentalRepository
{
    Task AddAsync(Rental rental);
    Task<bool> IsCarRented(int carId, DateTime start, DateTime end);
    Task<List<Rental>> GetAllAsync();
}
