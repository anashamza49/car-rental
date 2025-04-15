using Authentification.JWT.DAL.Models;


namespace Authentification.JWT.DAL.Repositories
{
    public interface ICarRepository
    {
        Task<Car> AddAsync(Car car);
        Task<Car?> GetByIdAsync(int id);
        Task<List<Car>> GetByOwnerIdAsync();

        Task<bool> DeleteAsync(int id);
        Task<Car> UpdateAsync(Car car);
    }
}
