using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizWebHookBot.Commands;
using Telegram.Bot;

namespace QuizWebHookBot
{
    public class Bot
    {
        private static TelegramBotClient botClient;
        private static List<ICommand> commandsList;

        public static IReadOnlyList<ICommand> Commands => commandsList.AsReadOnly();

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }

            commandsList = new List<ICommand>();
            commandsList.Add(new Start());
            //TODO: Add more commands

//            botClient = new TelegramBotClient(AppSettings.Key);
//            string hook = string.Format(AppSettings.Url, "api/message/update");
//            await botClient.SetWebhookAsync(hook);
            return botClient;
        }
    }
}