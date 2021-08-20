using Microsoft.AspNetCore.Mvc;

namespace TelegramAspBot.Controllers
{
    public class TestController : Controller
    {
        [Route(@"test")]
        [HttpGet]
        public string Get()
        {
            return "Awesome Test";
        }
    }
}