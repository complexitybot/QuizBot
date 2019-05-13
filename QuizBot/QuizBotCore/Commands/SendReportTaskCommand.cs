using System.Threading.Tasks;
using QuizBotCore.States;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace QuizBotCore.Commands
{
    public class SendReportTaskCommand : ICommand
    {
        private readonly int messageId;
        private const string ReportContact = "@quibblereport";

        public SendReportTaskCommand(int messageId)
        {
            this.messageId = messageId;
        }

        public async Task ExecuteAsync(Chat chat, TelegramBotClient client, ServiceManager serviceManager)
        {
            var user = serviceManager.userRepository.FindByTelegramId(chat.Id);
            var reportState = user.CurrentState as ReportState;
            var reportUserInfo = $"Report message from: {chat.Id};\n" +
                                 $"UserId: {user.Id};\n" +
                                 $"TopicId: {reportState.TopicDto.Id}" +
                                 $"LevelId: {reportState.LevelDto.Id}";
            await client.SendTextMessageAsync(ReportContact, reportUserInfo);
            await client.ForwardMessageAsync(ReportContact, chat.Id, messageId);
            await client.ForwardMessageAsync(ReportContact, chat.Id, user.MessageId);
            await client.SendTextMessageAsync(chat.Id, DialogMessages.ReportThanks);
            await new SelectTopicCommand().ExecuteAsync(chat, client, serviceManager);
        }
    }
}