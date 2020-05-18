using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestuarantWebApi.Model;
using RestuarantWebApi.Repository;
using RestuarantWebApi.Utils;
using System;

namespace RestuarantWebApi.Controllers
{
    /// <summary>
    /// User Api to add/ delete/ update User.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _configuration;

        private IUserRepository _userRepository;


        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        /// <param name="userRepository">IUserRepository</param>
        public UserController(IConfiguration configuration, IUserRepository userRepository)
        {
            this._configuration = configuration;
            this._userRepository = userRepository;
        }

        /// <summary>
        ///  Api to Get User from table Based on Username and Password.
        /// </summary>
        /// <param name="u">user BO that has passed username and passowrd.</param>
        /// <returns>return Username, password and token.</returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetUser([FromBody]User u)
        {
            IActionResult msg = null;
            try
            {
                User user = this._userRepository.GetUser(u.username, u.password);
                user.Token = JwtTokenManager.GenerateToken(user.username,user.Uniquekey,_configuration);
                msg = Ok(user);
            }
            catch (Exception ex)
            {
                //msg = HttpContext.Response.StatusCode.
                msg = BadRequest(ex.ToString());
            }
            return msg;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddUser([FromBody]User user)
        {
            IActionResult msg = null;
            try
            {
                user = this._userRepository.AddUser(user);
                msg = Ok(user);
            }
            catch (Exception ex)
            {
                //msg = HttpContext.Response.StatusCode.
                msg = BadRequest(ex.ToString());
            }
            return msg;
        }

        [CustomFilter]
        [HttpGet]
        public IActionResult ValidToken([FromQuery]string xyz)
        {
            return Ok(xyz);
        }
    }


}