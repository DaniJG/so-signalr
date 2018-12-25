using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using server.Models;

namespace server.Hubs
{
    public interface IQuestionHub
    {
        Task QuestionScoreChange(Guid questionId, int score);
        Task AnswerCountChange(Guid questionId, int answerCount);
        Task AnswerAdded(Answer answer);
    }

    public class QuestionHub: Hub<IQuestionHub>
    {
        // No need to implement here the methods defined by IQuestionHub, their purpose is simply
        // to provide a strongly typed interface.
        // Users of IHubContext still have to decide to whom should the events be sent
        // as in: await this.hubContext.Clients.All.SendQuestionScoreChange(question.Id, question.Score);

        // These 2 methods will be called from the client
        public async Task JoinQuestionGroup(Guid questionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, questionId.ToString());
        }
        public async Task LeaveQuestionGroup(Guid questionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, questionId.ToString());

        }
    }
}