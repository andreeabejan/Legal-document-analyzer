using Domain.Interfaces;
using Domain.Common;
using Domain.Entities;

namespace Application.Repositories
{
    public class UserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository) {
            this.userRepository = userRepository; 
        }

        public async Task<Result<User>> LoginAsync(string email, string password)
        {
            return await userRepository.LoginAsync(email, password);
        }

        public IUserRepository UserRepository { get {
                return userRepository; } }

        public async Task<Result<Guid>> AddUserAsync(User user)
        {
            return await userRepository.AddUserAsync(user);
        }

        public async Task<Result<User>> GetUserByIdAsync(Guid id)
        {
            return await userRepository.GetUserByIdAsync(id);
        }

        public async Task<Result<Guid>> UpdateUserAsync(User user)
        {
            return await userRepository.UpdateUserAsync(user);
        }

        public async Task<Result<Guid>> DeleteUserAsync(Guid id)
        {
            return await userRepository.DeleteUserAsync(id);
        }

        public async Task<Result<IEnumerable<User>>> GetAllUsersAsync()
        {
            return await userRepository.GetAllUsersAsync();
        }

        //public async Task<Result<IEnumerable<PastResultsListing>>> GetUserConversationsHistoryAsync(Guid userId)

    }
}
