using ApiSample.Configurations;
using ApiSample.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiSample.Application.Features.Commands
{
    public class LoginResult
    {
        public string AccessToken { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime ExpirationTime { get; set; }

    }

    public class LoginCommand : IRequest<ApiResponse<LoginResult>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<LoginResult>>
    {
        private readonly JwtConfig _TokenConfig;
        private readonly LoginPropertiesConfig _loginPropertiesConfig;

        public LoginCommandHandler(IOptions<JwtConfig> options, IOptions<LoginPropertiesConfig> loginPropertiesConfig)
        {
            _TokenConfig = options.Value;
            _loginPropertiesConfig = loginPropertiesConfig.Value;
        }


        public async Task<ApiResponse<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (_loginPropertiesConfig.Username != request.Username || _loginPropertiesConfig.Password != request.Password)
            {
                return new ApiResponse<LoginResult>(false, "Invalid username and or password", 400);
            }

            var token = GetJwtToken(_TokenConfig);
            var loginresult = new LoginResult
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpirationTime = token.ValidTo,
                TimeCreated = token.ValidFrom
            };

            return new ApiResponse<LoginResult>(loginresult, true, "Login Successful", 200);
        }


        private JwtSecurityToken GetJwtToken(JwtConfig _TokenConfig)
        {
            var claims = new List<Claim>()
           {
               new Claim("auth_channel", "FidelityMiddleWare"),
               new Claim(JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString()),
               new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
           };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_TokenConfig.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(60));
            var token = new JwtSecurityToken(_TokenConfig.Issuer, _TokenConfig.Audience, claims, expires: expires,
                notBefore: DateTime.Now, signingCredentials: creds);
            return token;
        }
    }
}
