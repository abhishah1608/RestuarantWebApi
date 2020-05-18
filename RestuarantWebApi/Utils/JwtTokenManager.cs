using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestuarantWebApi.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestuarantWebApi.Utils
{
    public class JwtTokenManager
    {
        
        /// <summary>
        /// Generate token based on Username.
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>returns Token</returns>
        public static string GenerateToken(string username,string userGuid,IConfiguration configuration)
        {
            string  Secret = configuration["Jwt:Secretekey"].ToString();
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                      new Claim(ClaimTypes.Name, username),new Claim(ClaimTypes.UserData,userGuid),
                new Claim("CreationTime",DateTime.UtcNow.ToString())}),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        /// <summary>
        /// Return principal. 
        /// </summary>
        /// <param name="token">Based on JWT Token it returns Principal.</param>
        /// <returns></returns>
        private static ClaimsPrincipal GetPrincipal(string token,IConfiguration configuration,bool IsRequiredExpiration,ref FaultContract faultContract)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                string Secret = configuration["Jwt:Secretekey"].ToString();
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = IsRequiredExpiration,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = IsRequiredExpiration 
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch(Exception ex) 
            {
                if (ex.Message.Contains("The token is expired."))
                {
                    string str = ex.Message.ToString();
                    faultContract = new FaultContract();
                    faultContract.faultmessage = "The token is expired.";
                    faultContract.faultcode = EfaultCode.TokenExpire;
                    return GetPrincipal(token, configuration, false,ref faultContract);
                }
            }
            return null;
        }

        /// <summary>
        /// Method used to ValidateToken based on token.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="configuration"></param>
        /// <param name="faultContract"></param>
        /// <returns></returns>
        public static TokenInfo ValidateToken(string token,IConfiguration configuration,out FaultContract faultContract)
        {
            string username = null;
            TokenInfo info = new TokenInfo();
            faultContract = null;
            ClaimsPrincipal principal = GetPrincipal(token,configuration,true,ref faultContract);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            Claim userdataClaim = identity.FindFirst(ClaimTypes.UserData);
            Claim creationtime = identity.FindFirst("CreationTime");
            username = usernameClaim.Value;
            info.username = username;
            info.Uniquekey = userdataClaim.Value;
            info.Issuetime = creationtime.Value;
            return info;
        }
    }
}
