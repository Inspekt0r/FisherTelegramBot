using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TelegramAspBot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogsController : Controller
    {
        private readonly string _rootDir;
        private readonly ILogger<LogsController> _logger;

        public LogsController(ILogger<LogsController> logger)
        {
            _logger = logger;
            _rootDir = AppDomain.CurrentDomain.BaseDirectory + @"logs\";
        }

        [HttpGet()]
        public async Task Get()
        {
            await WriteList(GetFileNames(""));
        }

        [HttpGet("{id}")]
        public async Task Get(int id)
        {
            var files = GetFileNames("");
            if (files.Length <= id)
            {
                await HttpContext.Response.WriteAsync("oops");
            }

            using StreamReader sr = new StreamReader(_rootDir + files[id]);
            await WriteBase(async () =>
            {
                while (sr.Peek() >= 0)
                {
                    await HttpContext.Response.WriteAsync(sr.ReadLine()+ "<br>");
                }
            });
        }

        private string[] GetFileNames(string pattern) => Directory.GetFiles(_rootDir, pattern).Select(Path.GetFileName).ToArray();

        private async Task WriteBase(Func<Task> body)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            HttpContext.Response.ContentType = "text/html";
            await HttpContext.Response.WriteAsync("<html lang=\"en\"><head><meta charset=utf-8><head><body>\r\n");

            await body();

            await HttpContext.Response.WriteAsync("</body></html>\r\n");
        }

        private async Task WriteList(string[] list)
        {
            await WriteBase(async () =>
            {
                var @ref = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}/";
                for (int i = 0; i < list.Length; i++)
                {
                    await HttpContext.Response.WriteAsync($"<a href={@ref}{i}>{list[i]}</a><br>\r\n");
                }
            });
        }
    }
}
