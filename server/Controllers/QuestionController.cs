using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using server.Hubs;
using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IHubContext<QuestionHub, IQuestionHub> hubContext;
        private static ConcurrentBag<Question> questions = new ConcurrentBag<Question> {
          new Question {
                Id = Guid.Parse("b00c58c0-df00-49ac-ae85-0a135f75e01b"),
                CreatedBy = "terry.pratchett@lspace.com",
                Title = "Welcome",
                Body = "Welcome to the _mini Stack Overflow_ rip-off!\n" +
                       "This application was built as an example on how **SignalR** and **Vue** can play together\n" +
                       " - [Original article in the DotNetCurry magazine](https://www.dotnetcurry.com/aspnet-core/1480/aspnet-core-vuejs-signalr-app)\n" +
                       " - [GitHub source of this app](https://github.com/DaniJG/so-signalr)",
                Answers = new List<Answer>{ new Answer { Body = "Sample answer", CreatedBy = "pierre.lemaitre@gmail.com" }}
            },
          new Question {
                Id = Guid.Parse("eb20d554-80be-429c-8418-5a72245bcaf3"),
                CreatedBy = "terry.pratchett@lspace.com",
                Title = "Welcome Back!",
                Body = "The second iteration enhanced the app adding authentication.\n" +
                       "It includes examples for both **cookie** and **jwt** based authentication integrated with Vue and SignalR.\n" +
                       "While this will be the subject of a new DotNetCurry article, you can Start by checking out these links:\n" +
                       " - [SignalR authentication docs](https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-2.2)\n" +
                       " - [Example with multiple authentication schemes](https://github.com/aspnet/AspNetCore/tree/release/2.2/src/Security/samples/PathSchemeSelection)\n" +
                       " - [JWT examples with ASP.NET Core](https://jasonwatmore.com/post/2018/08/14/aspnet-core-21-jwt-authentication-tutorial-with-example-api)\n" +
                       " - [Securing APIs in ASP.NET Core](https://www.blinkingcaret.com/2018/07/18/secure-an-asp-net-core-web-api-using-cookies/)",
                Answers = new List<Answer>()
            },
        };

        public QuestionController(IHubContext<QuestionHub, IQuestionHub> questionHub)
        {
            this.hubContext = questionHub;
        }

        [HttpGet()]
        public IEnumerable GetQuestions()
        {
            return questions.Where(t => !t.Deleted).Select(q => new {
                Id = q.Id,
                CreatedBy = q.CreatedBy,
                Title = q.Title,
                Body = q.Body,
                Score = q.Score,
                AnswerCount = q.Answers.Count
            });
        }

        [HttpGet("{id}")]
        public ActionResult GetQuestion(Guid id)
        {
            var question = questions.SingleOrDefault(t => t.Id == id && !t.Deleted);
            if (question == null) return NotFound();

            return new JsonResult(question);
        }

        [HttpPost()]
        [Authorize]
        public async Task<Question> AddQuestion([FromBody]Question question)
        {
            question.Id = Guid.NewGuid();
            question.CreatedBy = this.User.Identity.Name;
            question.Deleted = false;
            question.Answers = new List<Answer>();
            questions.Add(question);
            await this.hubContext.Clients.All.QuestionAdded(question);
            return question;
        }

        [HttpPost("{id}/answer")]
        [Authorize]
        public async Task<ActionResult> AddAnswerAsync(Guid id, [FromBody]Answer answer)
        {
            var question = questions.SingleOrDefault(t => t.Id == id && !t.Deleted);
            if (question == null) return NotFound();

            answer.Id = Guid.NewGuid();
            answer.QuestionId = id;
            answer.CreatedBy = this.User.Identity.Name;
            answer.Deleted = false;
            question.Answers.Add(answer);

            // Notify anyone connected to the group for this answer
            await this.hubContext.Clients.Group(id.ToString()).AnswerAdded(answer);
            // Notify every client
            await this.hubContext.Clients.All.AnswerCountChange(question.Id, question.Answers.Count);

            return new JsonResult(answer);
        }

        [HttpPatch("{id}/upvote")]
        [Authorize]
        public async Task<ActionResult> UpvoteQuestionAsync(Guid id)
        {
            var question = questions.SingleOrDefault(t => t.Id == id && !t.Deleted);
            if (question == null) return NotFound();

            // Warning, this isnt really atomic!
            question.Score++;

            // Notify every client
            await this.hubContext.Clients.All.QuestionScoreChange(question.Id, question.Score);

            return new JsonResult(question);
        }

        [HttpPatch("{id}/downvote")]
        [Authorize]
        public async Task<ActionResult> DownvoteQuestionAsync(Guid id)
        {
            var question = questions.SingleOrDefault(t => t.Id == id && !t.Deleted);
            if (question == null) return NotFound();

            // Warning, this isnt really atomic
            question.Score--;

            // Notify every client
            await this.hubContext.Clients.All.QuestionScoreChange(question.Id, question.Score);

            return new JsonResult(question);
        }
    }
}
