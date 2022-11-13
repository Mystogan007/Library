using Library.Models.Authenticate;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Repository
{
    public interface IJWTTokenRepository
    {
        public Task ProcessTokenDataAsync(string login, string token, CancellationToken cancellationToken);

        public Task<bool> CheckTokenIsExistAsync(string login, string token, CancellationToken cancellationToken);

    }
}
