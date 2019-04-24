﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuizRequestService;
using QuizWebHookBot.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace QuizWebHookBot.Controllers
{
    [Route("api/[controller]")]
    public class UpdateController : Controller
    {
        private readonly IBotService botService;
        private readonly IUpdateService updateService;
        private readonly IQuizService quizService;
        private readonly ILogger<UpdateController> logger;

        public UpdateController(IUpdateService updateService, IBotService botService, IQuizService quizService, 
            ILogger<UpdateController> logger)
        {
            this.updateService = updateService;
            this.botService = botService;
            this.quizService = quizService;
            this.logger = logger;
        }

        // POST api/update
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            Chat chat = null;
            switch (update.Type)
            {
                case UpdateType.Message:
                    chat = update.Message.Chat;
                    break;
                case UpdateType.CallbackQuery:
                    chat = update.CallbackQuery.Message.Chat;
                    break;
            }
            var userCommand = updateService.ProcessMessage(update);
            await userCommand.ExecuteAsync(chat, botService.Client, quizService);

            return Ok();
        }
    }
}