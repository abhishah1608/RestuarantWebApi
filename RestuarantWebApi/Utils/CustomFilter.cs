using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestuarantWebApi.Model;
using RestuarantWebApi.Repository;
using System;

namespace RestuarantWebApi.Utils
{
    public class CustomFilter : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// this  will be called for all request that decorated with CustomFilter.
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string authorization = context.HttpContext.Request.Headers["Authorization"];
            IConfiguration configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            IUserRepository _userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
            if (authorization == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            else
            {
                if (authorization.Split(" ")[0] == "Bearer")
                {
                    string token = authorization.Split(" ")[1];
                    FaultContract faultContract = null;
                    TokenInfo info = JwtTokenManager.ValidateToken(token, configuration,out faultContract);
                    if(faultContract != null && faultContract.faultcode == EfaultCode.TokenExpire)
                    {
                        //here token is expired.
                       info.Token = token;
                       string userGuid = _userRepository.GetUserGuId(info);
                        if (!string.IsNullOrEmpty(userGuid))
                        {
                            string Newtoken = JwtTokenManager.GenerateToken(info.username, userGuid, configuration);
                            context.HttpContext.Response.Headers["Token"] = Newtoken;
                            //context.Result
                        }
                        else
                        {
                            // New token Already Assigned someone tried to use old token.
                            context.Result = new UnauthorizedResult();
                            return;
                        }
                    }
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

            }

        }
    }
}
