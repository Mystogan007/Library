using Library.Models.Users;
using Library.Storage;
using Library.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Repository
{
    public class PgAccountRepository : IAccountRepository
    {
        private readonly ApplicationContext _context;

        public PgAccountRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(UserCredentials userCredentials, CancellationToken cancellationToken)
        {
            Users user = new Users()
            {
                Id = MD5.Create(userCredentials.Login),
                Login = userCredentials.Login,
                Password = userCredentials.Password,
                Role = "user"
            };
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckIfUserExistAsync(string login, CancellationToken cancellationToken)
        {
            string id = MD5.Create(login);
            var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);
            return user != null;
        }

        public async Task<Users> GetUserInfoAsync(UserCredentials userCredentials, CancellationToken cancellationToken)
        {
            string id = MD5.Create(userCredentials.Login);
            var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);
            return user;
        }
    }


}





