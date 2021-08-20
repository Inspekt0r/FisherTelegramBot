using System;
using System.Threading.Tasks;
using TelegramAspBot.Models;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Job
{
    public interface IJob
    {
       int TelegramId { get; set; }
       Task DoAction();
       DateTime TimeToDo { get; set; }
       bool IsCompleted { get; set; }
       ApplicationContext DbContext { get; set; }
    }
}
