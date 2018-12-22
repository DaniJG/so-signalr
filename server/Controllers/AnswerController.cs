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
    public class AnswerController : ControllerBase
    {
        private static ConcurrentBag<Answer> Answers = new ConcurrentBag<Answer> ();

        [HttpGet()]
        public IEnumerable<Answer> GetAnswers()
        {
            return Answers.Where(t => !t.Deleted);
        }

        [HttpPost()]
        public Answer AddAnswer([FromBody]Answer Answer)
        {
            Answer.Id = Guid.NewGuid();
            Answer.Deleted = false;
            Answers.Add(Answer);
            return Answer;
        }

        [HttpDelete("{id}")]
        public ActionResult RemoveAnswer(Guid id)
        {
            var Answer = Answers.SingleOrDefault(t => t.Id == id);
            if (Answer == null) return NotFound();

            Answer.Deleted = true;
            return StatusCode(204);
        }
    }
}
