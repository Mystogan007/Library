using Library.Models.Authenticate;
using Library.Storage;
using Library.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using MD5 = Library.Utils.MD5;

namespace Library.Repository
{
    public class PgJWTTokenRepository : IJWTTokenRepository
    {
        private ApplicationContext _context;

        public PgJWTTokenRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckTokenIsExistAsync(string login, string token, CancellationToken cancellationToken)
        {
            var id = MD5.Create(login);
            var tokenData = await _context.Tokens.FindAsync(new object[] { id }, cancellationToken);
            return tokenData != null;
        }

        public async Task ProcessTokenDataAsync(string login, string token, CancellationToken cancellationToken)
        {
            var id = MD5.Create(login);
            var tokenData = await _context.Tokens.FindAsync(new object[] { id }, cancellationToken);
           

            if (tokenData != null)
            {
                tokenData.RefreshToken = token;
                await _context.SaveChangesAsync();
            }
            else
            {
                UserTokenData tblRefreshtoken = new UserTokenData()
                {
                    Id = MD5.Create(login),
                    Login = login,
                    TokenId = new Random().Next().ToString(),
                    RefreshToken = token,
                    IsActive = true
                };
                await _context.Tokens.AddAsync(tblRefreshtoken);
            }
            await _context.SaveChangesAsync();
        }

    }
}
