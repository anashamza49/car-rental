using Authentification.JWT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentification.JWT.DAL.Repositories
{
    public interface ICarRepository
    {
        Task<Car> AddAsync(Car car);
        Task<Car?> GetByIdAsync(int id);
        Task<List<Car>> GetByOwnerIdAsync(int ownerId);
        Task<bool> DeleteAsync(int id);
        Task<Car> UpdateAsync(Car car);
    }
}
