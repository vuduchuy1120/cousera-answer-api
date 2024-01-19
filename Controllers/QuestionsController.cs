using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cousera.Models;
using Cousera.DTO;

namespace Cousera.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly couseraAutoContext _context;

        public QuestionsController(couseraAutoContext context)
        {
            _context = context;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
          // convert question to question response
          var questions = await _context.Questions
                .Include(q => q.Answers)
                .ToListAsync();
            var questionResponses = questions.Select(q => new QuestionResponse
            {
                Id = q.Id,
                QuestionDescription = q.QuestionDescription,
                Answers = q.Answers.Select(a => new AnswerResponse
                {
                    Id = a.Id,
                    Description = a.Description
                }).ToList()
            }).ToList();
            return Ok(questionResponses);
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
          if (_context.Questions == null)
          {
              return NotFound();
          }
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return question;
        }

        // PUT: api/Questions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
            if (id != question.Id)
            {
                return BadRequest();
            }

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Questions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Question>> AddQuestion(QuestionRequest question)
        {
            // Add question and answers
            var newQuestion = new Question
            {
                QuestionDescription = question.QuestionDescription
            };
            foreach (var answer in question.Answers)
            {
                newQuestion.Answers.Add(new Answer
                {
                    Description = answer.Description
                });
            }
            try
            {
                _context.Questions.Add(newQuestion);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            

            // get question just added to convert to QuestionResponse
            var questionResponse = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == newQuestion.Id);
            QuestionResponse questionResponse1 = new QuestionResponse
            {
                Id = questionResponse.Id,
                QuestionDescription = questionResponse.QuestionDescription,
                Answers = questionResponse.Answers.Select(a => new AnswerResponse
                {
                    Id = a.Id,
                    Description = a.Description
                }).ToList()
            };

            return Ok(questionResponse1);
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            if (_context.Questions == null)
            {
                return NotFound();
            }
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionExists(int id)
        {
            return (_context.Questions?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //Delete All question and answer
        [HttpDelete]
        public async Task<IActionResult> DeleteAllQuestion()
        {
            if (_context.Questions == null)
            {
                return NotFound();
            }
            var questions = await _context.Questions.ToListAsync();
            if (questions == null)
            {
                return NotFound();
            }
            // delete all answer
            foreach (var question in questions)
            {
                var answers = await _context.Answers.Where(a => a.QuestionId == question.Id).ToListAsync();
                _context.Answers.RemoveRange(answers);
            }
            _context.Questions.RemoveRange(questions);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Search question by question description
        [HttpGet("search/{questionDescription}")]
        public async Task<ActionResult<IEnumerable<Question>>> SearchQuestion(string questionDescription)
        {
            if (_context.Questions == null)
            {
                return NotFound();
            }
            var questions = await _context.Questions
                    .Include(q => q.Answers) // Ensure answers are loaded
                    .Where(q => q.QuestionDescription.Contains(questionDescription))
                    .ToListAsync();
            if (questions == null)
            {
                return NotFound();
            }
            // convert question to question response
            var questionResponses = questions.Select(q => new QuestionResponse
            {
                Id = q.Id,
                QuestionDescription = q.QuestionDescription,
                Answers = q.Answers.Select(a => new AnswerResponse
                {
                    Id = a.Id,
                    Description = a.Description
                }).ToList()
            }).ToList();
            return Ok(questionResponses);
        }
    }
}
