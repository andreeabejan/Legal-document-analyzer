using Domain.Common;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<Guid>> AddUserAsync(User user);
        Task<Result<User>> GetUserByIdAsync(Guid id);
        Task<Result<Guid>> UpdateUserAsync(User user);
        Task<Result<Guid>> DeleteUserAsync(Guid id);

        Task<Result<IEnumerable<User>>> GetAllUsersAsync();
        Task<Result<User>> LoginAsync(string email, string password);

        //Task<Result<IEnumerable<PastResultsListing>>> GetUserConversationsHistoryAsync(Guid userId);
    }
}
