using Authentification.JWT.DAL.Data;
using Authentification.JWT.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentification.JWT.DAL.Repositories
{
    public class RentalRepository(MyDbContext context) : IRentalRepository
    {
        public async Task AddAsync(Rental rental)
        {
            await context.Rentals.AddAsync(rental);
            await context.SaveChangesAsync();
        }

        public async Task<bool> IsCarRented(int carId, DateTime start, DateTime end)
        {
            return await context.Rentals.AnyAsync(r =>
                r.CarId == carId &&
                r.StartDate < end &&
                r.EndDate > start
            );
        }

        public async Task<List<Rental>> GetAllAsync()
        {
            return await context.Rentals
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
