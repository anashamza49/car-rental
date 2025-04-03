using Authentification.JWT.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentification.JWT.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Authentification.JWT.DAL.Repositories
{
    public class UserRepository(MyDbContext context) : IUserRepository
    {

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<User> RegisterUserAsync(User user)
        {
            if (await context.Users.AnyAsync(u => u.Username == user.Username || u.Email == user.Email))
            {
                throw new InvalidOperationException("Un utilisateur avec ce nom ou email existe déjà");
            }

            var createdUser = await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return createdUser.Entity;
        }
        // get user by id without tracking
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
