using Library.Models.Users;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Repository
{
    public interface IAccountRepository
    {
        public Task<Users> GetUserInfoAsync(UserCredentials userCredentials, CancellationToken cancellationToken);

        public Task<bool> CheckIfUserExistAsync(string login, CancellationToken cancellationToken);

        public Task AddUserAsync(UserCredentials userCredentials, CancellationToken cancellationToken);
    }
}
