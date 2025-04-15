using Authentification.JWT.DAL.Data;
using Authentification.JWT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Authentification.JWT.DAL.Repositories
{
    public class CarRepository(MyDbContext context) : ICarRepository
    {
        public async Task<Car> AddAsync(Car car)
        {
            await context.Cars.AddAsync(car);
            await context.SaveChangesAsync();
            return car;
        }

        public async Task<Car?> GetByIdAsync(int id)
            => await context.Cars.FindAsync(id);

        public async Task<List<Car>> GetByOwnerIdAsync(int ownerId)
            => await context.Cars.Where(c => c.OwnerId == ownerId).ToListAsync();

        public async Task<bool> DeleteAsync(int id)
        {
            var car = await context.Cars.FindAsync(id);
            if (car == null) return false;

            context.Cars.Remove(car);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<Car> UpdateAsync(Car car)
        {
            context.Cars.Update(car);
            await context.SaveChangesAsync();
            return car;
        }

    }
}
