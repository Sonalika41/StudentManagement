//using ApiCallAdv.Repositories.Interface;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace ApiCallAdv.Repositories.Implementation
//{
//    public class TokenRepository : ITokenRepository
//    {
//        private readonly IConfiguration configuration;
//        public TokenRepository(IConfiguration configuration) => this.configuration = configuration;

//        public string CreateJwtToken(IdentityUser user, List<string> roles, Guid? studentId, Guid? teacherClassId)
//        {
//            var keyString = configuration["Jwt:Key"];
//            if (string.IsNullOrWhiteSpace(keyString))
//                throw new InvalidOperationException("JWT key is not configured.");

//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
//            };

//            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

//            if (studentId.HasValue)
//                claims.Add(new Claim("StudentID", studentId.Value.ToString()));
//            if (teacherClassId.HasValue)
//                claims.Add(new Claim("TeacherClassId", teacherClassId.Value.ToString()));

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
//            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: configuration["Jwt:Issuer"],
//                audience: configuration["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.Now.AddHours(12),
//                signingCredentials: credentials);

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}


using ApiCallAdv.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiCallAdv.Repositories.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;
        public TokenRepository(IConfiguration configuration) => this.configuration = configuration;

        public string CreateJwtToken(IdentityUser user, List<string> roles, Guid? studentId, Guid? teacherClassId)
        {
            var keyString = configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(keyString))
                throw new InvalidOperationException("JWT key is not configured.");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add role claims for API auth and frontend decoding
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); // ASP.NET authorization
                claims.Add(new Claim("role", role));          // frontend decode robustness
            }

            if (studentId.HasValue)
                claims.Add(new Claim("StudentID", studentId.Value.ToString()));
            if (teacherClassId.HasValue)
                claims.Add(new Claim("TeacherClassId", teacherClassId.Value.ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
