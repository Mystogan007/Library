using Library.Models;
using Library.Models.ApiResponses;
using Library.Models.Authenticate;
using Library.Models.Users;
using Library.Repository;
using Library.Storage;
using Library.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        IAccountRepository _accountRepository;
        IJWTTokenRepository _jwtTokenRepository;
        private IConfiguration _iconfiguration;
        private CancellationTokenSource _cancellationTokenSource;

        public AccountController(ApplicationContext context, IConfiguration configuration)
        {
            _iconfiguration = configuration;
            _accountRepository = new PgAccountRepository(context);
            _jwtTokenRepository = new PgJWTTokenRepository(context);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Authenticate")]
        public async Task<ApiResponseToken> Authenticate([FromBody] UserCredentials usersdata)
        {
            if (usersdata == null)
            {
                return new ApiResponseToken { Status = "Error", Error = "Bad request" };
            }
            if (string.IsNullOrEmpty(usersdata.Login) || string.IsNullOrEmpty(usersdata.Password))
            {
                return new ApiResponseToken { Status = "Error", Error = "Bad request" };
            }
            try
            {
                var user = await _accountRepository.GetUserInfoAsync(usersdata, _cancellationTokenSource.Token);
                if (user == null)
                {
                    return new ApiResponseToken { Status = "Error", Error = "Unauthorized" };
                }
                TokenResponse tokenResponse = new TokenResponse();
                var tokenhandler = new JwtSecurityTokenHandler();
                var tokenkey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                        new Claim(ClaimTypes.Name, user.Login),
                        new Claim(ClaimTypes.Role, user.Role)
                        }
                    ),
                    Expires = DateTime.Now.AddMinutes(20),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
                };
                var token = tokenhandler.CreateToken(tokenDescriptor);
                string finaltoken = tokenhandler.WriteToken(token);

                tokenResponse.JWTToken = finaltoken;
                tokenResponse.RefreshToken = await GetToken(user.Login);

                return new ApiResponseToken { Status = "Error", Error = "Internal server error", Token = tokenResponse};
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponseToken { Status = "Error", Error = "Internal server error" };
            }
        }


        [Route("Refresh")]
        [HttpPost]
        public async Task<ApiResponseToken> Refresh([FromBody] TokenResponse token)
        {
            try
            {
                if (token == null)
                {
                    return new ApiResponseToken { Status = "Error", Error = "Bad request" };
                }
                if (string.IsNullOrEmpty(token.RefreshToken) || string.IsNullOrEmpty(token.JWTToken))
                {
                    return new ApiResponseToken { Status = "Error", Error = "Bad request" };
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token.JWTToken);
                var login = securityToken.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;

                var tokenIsExist = await _jwtTokenRepository.CheckTokenIsExistAsync(login, token.RefreshToken, _cancellationTokenSource.Token);

                if (!tokenIsExist)
                {
                    return new ApiResponseToken { Status = "Error", Error = "Unauthorized" };
                }

                TokenResponse result = await Authenticate(login, securityToken.Claims.ToArray());
                return new ApiResponseToken { Status = "Ok", Error = string.Empty, Token = result };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponseToken { Status = "Error", Error = "Internal server error" };
            }
        }


        public async Task<string> GetToken(string login)
        {
            var token = RefreshTokenGenerator.GenerateToken(login);
            await _jwtTokenRepository.ProcessTokenDataAsync(login, token, _cancellationTokenSource.Token);
            return token;
        }

        [NonAction]
        public async Task<TokenResponse> Authenticate(string login, Claim[] claims)
        {
            TokenResponse tokenResponse = new TokenResponse();
            var tokenkey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Key"]);
            var tokenhandler = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                 signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
                );
            tokenResponse.JWTToken = new JwtSecurityTokenHandler().WriteToken(tokenhandler);
            tokenResponse.RefreshToken = await GetToken(login);
            return tokenResponse;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ApiResponse> Register([FromBody] UserCredentials usersdata)
        {
            if (usersdata == null)
            {
                return new ApiResponse { Status = "Error", Error = "Bad request" };
            }
            if (string.IsNullOrEmpty(usersdata.Login) || string.IsNullOrEmpty(usersdata.Password))
            {
                return new ApiResponse { Status = "Error", Error = "Bad request" };
            }
            try
            {
                var userIsExist = await _accountRepository.CheckIfUserExistAsync(usersdata.Login, _cancellationTokenSource.Token);

                if (!userIsExist)
                {
                    await _accountRepository.AddUserAsync(usersdata, _cancellationTokenSource.Token);
                    return new ApiResponse { Status = "Ok", Error = String.Empty };
                }
                else
                {
                    return new ApiResponse { Status = "Error", Error = "Пользователь с таким логином уже существует" };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponse { Status = "Error", Error = "Internal server error" };
            }
        }
    }
}
