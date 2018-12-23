using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using server.Models;

namespace server.Hubs
{
  public interface IQuestionHub
  {
      Task SendQuestionScoreChange(Question question);
  }

    public class QuestionHub: Hub<IQuestionHub>
    {
        // These methods are not even used when injecting a IHubContext in the controller
        public Task SendQuestionScoreChange(Question question)
        {
            throw new System.NotImplementedException();
            // return Clients.All.SendQuestionScoreChange(question);
        }
    }
}