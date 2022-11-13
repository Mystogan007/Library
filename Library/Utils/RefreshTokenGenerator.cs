using Library.Models.Authenticate;
using Library.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Library.Utils
{
    public class RefreshTokenGenerator
    {
        public static string GenerateToken(string username)
        {
            var randomnumber = new byte[32];
            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                string RefreshToken = Convert.ToBase64String(randomnumber);
                return RefreshToken;
            }
        }
    }
}
