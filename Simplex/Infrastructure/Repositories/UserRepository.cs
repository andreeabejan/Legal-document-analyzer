using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManagementDbContext _dbContext;
        public UserRepository(UserManagementDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Result<User>> LoginAsync(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return Result<User>.Failure("Invalid email or password");
            }


            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
            var passwordVerificationResult = hasher.VerifyHashedPassword(user, user.HashedPassword, password);


            if (passwordVerificationResult != Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
            {
                return Result<User>.Failure("Invalid email or password");
            }

            return Result<User>.Success(user);
        }

        public async Task<Result<Guid>> AddUserAsync(User user)
        {
            try
            {
                var entityEntry = await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return Result<Guid>.Success(entityEntry.Entity.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure($"An error occurred while adding the user: {ex.Message}");
            }
        }

        public async Task<Result<User>> GetUserByIdAsync(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return Result<User>.Failure("User not found");
            }
            return Result<User>.Success(user);
        }

        public Task<Result<Guid>> UpdateUserAsync(User user)
        {
            var existingUser = _dbContext.Users.Find(user.Id);
            if (existingUser == null)
            {
                return Task.FromResult(Result<Guid>.Failure("User not found"));
            }
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.HashedPassword = user.HashedPassword;
            _dbContext.Users.Update(existingUser);
            _dbContext.SaveChanges();
            return Task.FromResult(Result<Guid>.Success(existingUser.Id));
        }

        public Task<Result<Guid>> DeleteUserAsync(Guid id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null)
            {
                return Task.FromResult(Result<Guid>.Failure("User not found"));
            }
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return Task.FromResult(Result<Guid>.Success(id));
        }

        public Task<Result<IEnumerable<User>>> GetAllUsersAsync()
        {
            return Task.FromResult(Result<IEnumerable<User>>.Success(_dbContext.Users.ToList()));
        }
    }
}
