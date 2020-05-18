using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RestuarantWebApi.HubConfig;
using System.Text;
using System.Threading.Tasks;

namespace RestuarantWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IHubContext<LoginHub> _loginHub;

        public LoginController(IHubContext<LoginHub> _loginHub)
        {
            this._loginHub = _loginHub;
        }

        [HttpGet]
        public async Task GetLogin()
        {
            string str = "1";
           await _loginHub.Clients.All.SendAsync("IsUserAlreadyLoggedIn", str);
            Response.Headers.Add("Content-Type", "text/event-stream");
            byte[] dataItemBytes = ASCIIEncoding.ASCII.GetBytes(str);
            await Response.Body.WriteAsync(dataItemBytes, 0, dataItemBytes.Length);
            await Response.Body.FlushAsync();
        }

    }
}