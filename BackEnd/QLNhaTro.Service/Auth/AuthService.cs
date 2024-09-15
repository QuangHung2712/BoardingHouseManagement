using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QLNhaTro.Commons;
using QLNhaTro.Moddel;
using QLNhaTro.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static QLNhaTro.Commons.CommonEnums;

namespace QLNhaTro.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _appDbContext;
        private readonly AppSettings _appSettings;
        public AuthService(IOptions<AppSettings> appSettings, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _appSettings = appSettings.Value;
        }
        private string GenerateToke(List<FeatureCode> permissions)
        {
            try
            {
                List<Claim> claims = new List<Claim>() {new Claim (ClaimTypes.Role,string.Join(",",permissions)) };
                SymmetricSecurityKey secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.SecretKey));
                SigningCredentials credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
                JwtSecurityToken token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public string GenerateToke()
        {
            return GenerateToke(new List<FeatureCode> { FeatureCode.Commons});
        }
    }
}
