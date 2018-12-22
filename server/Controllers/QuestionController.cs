using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private static ConcurrentBag<Question> Questions = new ConcurrentBag<Question> {
            new Question { Id = Guid.NewGuid(), Title = "Welcome", Body = "Welcome to the _mini_ **Stack Overflow** rip-off!\nThis will help showcasing **SignalR** and its integration with **Vue**" }
        };

        [HttpGet()]
        public IEnumerable<Question> GetQuestions()
        {
            return Questions.Where(t => !t.Deleted);
        }

        [HttpPost()]
        public Question AddQuestion([FromBody]Question Question)
        {
            Question.Id = Guid.NewGuid();
            Question.Deleted = false;
            Questions.Add(Question);
            return Question;
        }

        [HttpDelete("{id}")]
        public ActionResult RemoveQuestion(Guid id)
        {
            var Question = Questions.SingleOrDefault(t => t.Id == id);
            if (Question == null) return NotFound();

            Question.Deleted = true;
            return StatusCode(204);
        }
    }
}
